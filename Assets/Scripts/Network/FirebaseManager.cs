using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if FIREBASE_ENABLED
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
#endif

namespace CristoAdventure.Network
{
    /// <summary>
    /// Manages Firebase integration for authentication, cloud saves, and analytics
    /// </summary>
    public class FirebaseManager : MonoBehaviour
    {
        #region Singleton

        private static FirebaseManager _instance;
        public static FirebaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_FirebaseManager");
                    _instance = go.AddComponent<FirebaseManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        #endregion

        #region State

        private bool _isInitialized = false;
        private bool _isInitializing = false;

#if FIREBASE_ENABLED
        private FirebaseAuth _auth;
        private FirebaseFirestore _db;
        private FirebaseUser _user;
#endif

        #endregion

        #region Events

        public event Action OnInitialized;
        public event Action<FirebaseUser> OnUserSignedIn;
        public event Action OnCloudSaveComplete;
        public event Action<string> OnCloudSaveFailed;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region Initialization

        public void Initialize()
        {
#if FIREBASE_ENABLED
            if (_isInitialized || _isInitializing)
                return;

            _isInitializing = true;
            Debug.Log("[FirebaseManager] Initializing Firebase...");

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result != DependencyStatus.Available)
                {
                    Debug.LogError($"[FirebaseManager] Could not resolve Firebase dependencies: {task.Result}");
                    _isInitializing = false;
                    return;
                }

                // Firebase is ready
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // Initialize Auth
                _auth = FirebaseAuth.DefaultInstance;

                // Initialize Firestore
                _db = FirebaseFirestore.DefaultInstance;

                _isInitialized = true;
                _isInitializing = false;

                Debug.Log("[FirebaseManager] Firebase initialized successfully");

                // Check for existing user
                _auth.StateChanged += AuthStateChanged;
                AuthStateChanged(this, null);

                OnInitialized?.Invoke();
            });
#else
            Debug.LogWarning("[FirebaseManager] Firebase is not enabled in this build. Define FIREBASE_ENABLED to use Firebase features.");
            _isInitialized = false;
#endif
        }

#if FIREBASE_ENABLED
        private void AuthStateChanged(object sender, EventArgs eventArgs)
        {
            if (_auth.CurrentUser != _user)
            {
                bool signedIn = _auth.CurrentUser != null && _auth.CurrentUser.IsValid();
                if (!signedIn && _user != null)
                {
                    Debug.Log("[FirebaseManager] Signed out");
                }

                _user = _auth.CurrentUser;

                if (signedIn)
                {
                    Debug.Log($"[FirebaseManager] Signed in as: {_user.DisplayName} ({_user.UserId})");
                    OnUserSignedIn?.Invoke(_user);
                }
            }
        }
#endif

        #endregion

        #region Authentication

#if FIREBASE_ENABLED
        public void SignInAnonymously()
        {
            _auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"[FirebaseManager] Anonymous sign-in failed: {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("[FirebaseManager] Anonymous sign-in successful");
                }
            });
        }

        public void SignInWithEmail(string email, string password)
        {
            _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"[FirebaseManager] Email sign-in failed: {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("[FirebaseManager] Email sign-in successful");
                }
            });
        }

        public void SignUpWithEmail(string email, string password)
        {
            _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"[FirebaseManager] Email sign-up failed: {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("[FirebaseManager] Email sign-up successful");
                }
            });
        }

        public void SignOut()
        {
            if (_auth != null && _user != null)
            {
                _auth.SignOut();
                Debug.Log("[FirebaseManager] Signed out");
            }
        }

        public FirebaseUser GetCurrentUser()
        {
            return _user;
        }

        public bool IsUserSignedIn()
        {
            return _user != null && _user.IsValid();
        }
#endif

        #endregion

        #region Cloud Save

#if FIREBASE_ENABLED
        public void SaveToCloud(PlayerData data, string slotName)
        {
            if (!IsUserSignedIn())
            {
                Debug.LogWarning("[FirebaseManager] Cannot save to cloud - user not signed in");
                OnCloudSaveFailed?.Invoke("User not signed in");
                return;
            }

            string userId = _user.UserId;
            DocumentReference docRef = _db.Collection("users").Document(userId).Collection("saves").Document(slotName);

            // Convert PlayerData to dictionary
            Dictionary<string, object> saveData = SerializePlayerData(data);

            docRef.SetAsync(saveData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"[FirebaseManager] Cloud save failed: {task.Exception}");
                    OnCloudSaveFailed?.Invoke(task.Exception?.Message);
                }
                else
                {
                    Debug.Log($"[FirebaseManager] Cloud save successful: {slotName}");
                    OnCloudSaveComplete?.Invoke();
                }
            });
        }

        public void LoadFromCloud(string slotName, Action<PlayerData> onComplete)
        {
            if (!IsUserSignedIn())
            {
                Debug.LogWarning("[FirebaseManager] Cannot load from cloud - user not signed in");
                onComplete?.Invoke(null);
                return;
            }

            string userId = _user.UserId;
            DocumentReference docRef = _db.Collection("users").Document(userId).Collection("saves").Document(slotName);

            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"[FirebaseManager] Cloud load failed: {task.Exception}");
                    onComplete?.Invoke(null);
                }
                else if (task.IsCompleted)
                {
                    DocumentSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        PlayerData data = DeserializePlayerData(snapshot);
                        Debug.Log($"[FirebaseManager] Cloud load successful: {slotName}");
                        onComplete?.Invoke(data);
                    }
                    else
                    {
                        Debug.LogWarning($"[FirebaseManager] No cloud save found: {slotName}");
                        onComplete?.Invoke(null);
                    }
                }
            });
        }

        public void GetCloudSaveSlots(Action<List<SaveSlotInfo>> onComplete)
        {
            if (!IsUserSignedIn())
            {
                onComplete?.Invoke(new List<SaveSlotInfo>());
                return;
            }

            string userId = _user.UserId;
            CollectionReference savesRef = _db.Collection("users").Document(userId).Collection("saves");

            savesRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                List<SaveSlotInfo> slots = new List<SaveSlotInfo>();

                if (task.IsCompleted && !task.IsFaulted)
                {
                    QuerySnapshot snapshot = task.Result;
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        SaveSlotInfo slotInfo = new SaveSlotInfo
                        {
                            SlotName = doc.Id,
                            PlayerLevel = (long)doc.GetValue<int>("Level"),
                            CurrentPhase = doc.GetValue<string>("CurrentPhase"),
                            LastPlayed = doc.GetValue<DateTime>("LastPlayedDate")
                        };
                        slots.Add(slotInfo);
                    }
                }

                onComplete?.Invoke(slots);
            });
        }
#endif

        #endregion

        #region Serialization

#if FIREBASE_ENABLED
        private Dictionary<string, object> SerializePlayerData(PlayerData data)
        {
            return new Dictionary<string, object>
            {
                { "PlayerId", data.PlayerId },
                { "CreatedDate", data.CreatedDate },
                { "LastPlayedDate", data.LastPlayedDate },
                { "Level", data.Level },
                { "Experience", data.Experience },
                { "Coins", data.Coins },
                { "CurrentPhase", data.CurrentPhase },
                { "CompletedPhases", data.CompletedPhases },
                { "UnlockedCosmetics", data.UnlockedCosmetics },
                { "GameSettings_Language", data.GameSettings.Language },
                { "GameSettings_MusicVolume", data.GameSettings.MusicVolume },
                { "GameSettings_SfxVolume", data.GameSettings.SfxVolume },
                { "GameSettings_ShowSubtitles", data.GameSettings.ShowSubtitles },
                { "TotalPlayTime", data.Statistics.TotalPlayTime }
            };
        }

        private PlayerData DeserializePlayerData(DocumentSnapshot snapshot)
        {
            PlayerData data = new PlayerData
            {
                PlayerId = snapshot.GetValue<string>("PlayerId"),
                CreatedDate = snapshot.GetValue<DateTime>("CreatedDate"),
                LastPlayedDate = snapshot.GetValue<DateTime>("LastPlayedDate"),
                Level = snapshot.GetValue<int>("Level"),
                Experience = snapshot.GetValue<int>("Experience"),
                Coins = snapshot.GetValue<int>("Coins"),
                CurrentPhase = snapshot.GetValue<string>("CurrentPhase"),
                CompletedPhases = new List<string>(snapshot.GetValue<List<object>>("CompletedPhases").ConvertAll(x => x.ToString())),
                UnlockedCosmetics = new List<string>(snapshot.GetValue<List<object>>("UnlockedCosmetics").ConvertAll(x => x.ToString()))
            };

            // Game settings
            data.GameSettings = new GameSettings
            {
                Language = snapshot.GetValue<string>("GameSettings_Language"),
                MusicVolume = (float)snapshot.GetValue<double>("GameSettings_MusicVolume"),
                SfxVolume = (float)snapshot.GetValue<double>("GameSettings_SfxVolume"),
                ShowSubtitles = snapshot.GetValue<bool>("GameSettings_ShowSubtitles")
            };

            // Statistics
            data.Statistics = new PlayerStatistics
            {
                TotalPlayTime = (float)snapshot.GetValue<double>("TotalPlayTime")
            };

            return data;
        }
#endif

        #endregion

        #region Analytics

        public void LogEvent(string eventName, params Parameter[] parameters)
        {
#if FIREBASE_ENABLED
            if (_isInitialized)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parameters);
            }
#endif
        }

        public void LogPhaseStarted(string phaseId)
        {
            LogEvent("phase_started", new Parameter("phase_id", phaseId));
        }

        public void LogPhaseCompleted(string phaseId, int stars, float duration)
        {
            LogEvent("phase_completed",
                new Parameter("phase_id", phaseId),
                new Parameter("stars", stars),
                new Parameter("duration", duration)
            );
        }

        public void LogPuzzleCompleted(string puzzleId, bool success)
        {
            LogEvent("puzzle_completed",
                new Parameter("puzzle_id", puzzleId),
                new Parameter("success", success)
            );
        }

        #endregion

        #region Properties

        public bool IsInitialized => _isInitialized;

        #endregion
    }

    #region Parameter Class for Analytics

#if FIREBASE_ENABLED
    public class Parameter : Firebase.Analytics.FirebaseEventParameter
    {
        public Parameter(string name, object value) : base(name, value) { }
    }
#endif

    #endregion

    #region Forward Declaration for PlayerData

    // Forward reference to SaveManager.PlayerData
    public class PlayerData
    {
        public string PlayerId;
        public DateTime CreatedDate;
        public DateTime LastPlayedDate;
        public int Level;
        public int Experience;
        public int Coins;
        public string CurrentPhase;
        public List<string> CompletedPhases;
        public List<string> UnlockedCosmetics;
        public GameSettings GameSettings;
        public PlayerStatistics Statistics;
    }

    public class GameSettings
    {
        public string Language;
        public float MusicVolume;
        public float SfxVolume;
        public bool ShowSubtitles;
    }

    public class PlayerStatistics
    {
        public float TotalPlayTime;
    }

    public class SaveSlotInfo
    {
        public string SlotName;
        public int PlayerLevel;
        public string CurrentPhase;
        public DateTime LastPlayed;
    }

    #endregion
}
