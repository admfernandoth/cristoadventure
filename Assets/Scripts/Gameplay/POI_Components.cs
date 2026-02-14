using UnityEngine;
using UnityEngine.Events;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Informational Plaque - displays educational content about locations
    /// </summary>
    public class InfoPlaque : Interactable
    {
        [Header("Plaque Content")]
        [SerializeField] private string _titleKey = "InfoPlaque.Title";
        [SerializeField, TextArea(5, 10)] private string _contentKey = "InfoPlaque.Content";
        [SerializeField] private Sprite _iconSprite;

        [Header("Optional Article Unlock")]
        [SerializeField] private string _articleUnlockKey;
        [SerializeField] private int _coinReward = 10;

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Show info panel
            UIManager.Instance?.ShowInfoPanel(
                LocalizationManager.Instance?.GetLocalizedString(_titleKey) ?? _titleKey,
                LocalizationManager.Instance?.GetLocalizedString(_contentKey) ?? _contentKey,
                _iconSprite
            );

            // Give rewards
            if (_coinReward > 0)
            {
                GameManager.Instance?.AddCoins(_coinReward);
            }

            // Unlock article if applicable
            if (!string.IsNullOrEmpty(_articleUnlockKey))
            {
                var playerData = GameManager.Instance?.GetPlayerData();
                if (playerData != null)
                {
                    // Add to unlocked articles
                    playerData.InventoryItems.Add(new InventoryItem
                    {
                        ItemId = _articleUnlockKey,
                        ItemType = "Article",
                        AcquiredDate = System.DateTime.UtcNow,
                        IsNew = true
                    });
                }
            }

            Debug.Log($"[InfoPlaque] Displayed: {_titleKey}");

            if (_singleUse)
            {
                MarkAsUsed();
            }
        }

        protected override void ShowHighlight()
        {
            base.ShowHighlight();
            // Show book icon floating above plaque
            CreateFloatingIcon("📖");
        }

        private void CreateFloatingIcon(string emoji)
        {
            // Create a temporary floating icon
            GameObject icon = new GameObject("FloatingIcon");
            icon.transform.position = transform.position + Vector3.up * 2f;

            var textMesh = icon.AddComponent<TextMesh>();
            textMesh.text = emoji;
            textMesh.fontSize = 48;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;

            // Rotate to face camera
            icon.transform.LookAt(Camera.main.transform);

            // Fade animation
            StartCoroutine(AnimateFloatingIcon(icon));
        }

        private System.Collections.IEnumerator AnimateFloatingIcon(GameObject icon)
        {
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                icon.transform.position += Vector3.up * Time.deltaTime * 0.5f;
                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(icon);
        }
    }

    /// <summary>
    /// Reliquary - contains/holds a sacred relic
    /// </summary>
    public class ReliquaryPOI : Interactable
    {
        [Header("Relic Data")]
        [SerializeField] private string _relicNameKey;
        [SerializeField, TextArea(3, 6)] private string _relicDescriptionKey;
        [SerializeField] private Sprite _relicImage;
        [SerializeField] private string _relatedVerseKey;
        [SerializeField] private string _relatedVerseRef;

        [Header("Rewards")]
        [SerializeField] private int _coinReward = 50;
        [SerializeField] private string _stampId;

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Show relic detail view
            UIManager.Instance?.ShowRelicView(
                LocalizationManager.Instance?.GetLocalizedString(_relicNameKey) ?? _relicNameKey,
                LocalizationManager.Instance?.GetLocalizedString(_relicDescriptionKey) ?? _relicDescriptionKey,
                _relicImage,
                _relatedVerseRef
            );

            // Give rewards
            GameManager.Instance?.AddCoins(_coinReward);

            // Add stamp to collection
            if (!string.IsNullOrEmpty(_stampId))
            {
                var playerData = GameManager.Instance?.GetPlayerData();
                if (playerData != null && !playerData.CompletedPhases.Contains(_stampId))
                {
                    playerData.CompletedPhases.Add(_stampId);
                    Debug.Log($"[Reliquary] Stamp collected: {_stampId}");
                }
            }

            // Log analytics
            FirebaseManager.Instance?.LogEvent("relic_viewed", new Firebase.Analytics.Parameter("relic_id", _stampId));

            if (_singleUse)
            {
                MarkAsUsed();
            }
        }

        protected override void ShowHighlight()
        {
            base.ShowHighlight();
            CreateFloatingIcon("⭐");
        }

        private void CreateFloatingIcon(string emoji)
        {
            GameObject icon = new GameObject("FloatingIcon");
            icon.transform.position = transform.position + Vector3.up * 2f;

            var textMesh = icon.AddComponent<TextMesh>();
            textMesh.text = emoji;
            textMesh.fontSize = 48;
            textMesh.anchor = TextAnchor.MiddleCenter;

            icon.transform.LookAt(Camera.main.transform);
            StartCoroutine(AnimateFloatingIcon(icon));
        }

        private System.Collections.IEnumerator AnimateFloatingIcon(GameObject icon)
        {
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                icon.transform.position += Vector3.up * Time.deltaTime * 0.5f;
                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(icon);
        }
    }

    /// <summary>
    /// NPC Guide - provides educational dialogue
    /// </summary>
    public class NPCGuidePOI : Interactable
    {
        [Header("NPC Data")]
        [SerializeField] private string _npcNameKey;
        [SerializeField] private Sprite _npcPortrait;
        [SerializeField] private DialogueNode _startingDialogue;

        [Header("Settings")]
        [SerializeField] private bool _rotateToFacePlayer = true;

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Rotate to face player
            if (_rotateToFacePlayer)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                direction.y = 0;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.3f);
                }
            }

            // Start dialogue
            string localizedName = LocalizationManager.Instance?.GetLocalizedString(_npcNameKey) ?? _npcNameKey;
            DialogueManager.Instance?.StartDialogue(localizedName, _startingDialogue);

            Debug.Log($"[NPCGuide] Starting dialogue with: {_npcNameKey}");
        }

        protected override void ShowHighlight()
        {
            base.ShowHighlight();
            CreateFloatingIcon("💬");
        }

        private void CreateFloatingIcon(string emoji)
        {
            GameObject icon = new GameObject("FloatingIcon");
            icon.transform.position = transform.position + Vector3.up * 2.5f;

            var textMesh = icon.AddComponent<TextMesh>();
            textMesh.text = emoji;
            textMesh.fontSize = 48;
            textMesh.anchor = TextAnchor.MiddleCenter;

            icon.transform.LookAt(Camera.main.transform);
            StartCoroutine(AnimateFloatingIcon(icon));
        }

        private System.Collections.IEnumerator AnimateFloatingIcon(GameObject icon)
        {
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                icon.transform.position += Vector3.up * Time.deltaTime * 0.5f;
                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(icon);
        }
    }

    /// <summary>
    /// Photo Spot - special location for taking photos
    /// </summary>
    public class PhotoSpotPOI : Interactable
    {
        [Header("Photo Settings")]
        [SerializeField] private string _spotNameKey;
        [SerializeField] private Vector3 _photoPosition;
        [SerializeField] private Quaternion _photoRotation;
        [SerializeField] private float _photoFOV = 60f;

        [Header("Photo Subject")]
        [SerializeField] private Transform _photoSubject;

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Enter photo mode
            var cameraController = Camera.main?.GetComponent<CameraController>();
            cameraController?.EnterPhotoMode();

            // Move player to photo position
            var playerController = player.GetComponent<PlayerController>();
            playerController?.Teleport(_photoPosition);

            // Set camera angle and FOV
            Camera.main?.transform.SetPositionAndRotation(_photoPosition, _photoRotation);
            Camera.main.fieldOfView = _photoFOV;

            UIManager.Instance?.ShowPhotoModeUI();

            string spotName = LocalizationManager.Instance?.GetLocalizedString(_spotNameKey) ?? _spotNameKey;
            Debug.Log($"[PhotoSpot] Entering photo mode at: {spotName}");

            // Log analytics
            FirebaseManager.Instance?.LogEvent("photo_spot_entered", new Firebase.Analytics.Parameter("spot_id", _spotNameKey));
        }

        protected override void ShowHighlight()
        {
            base.ShowHighlight();
            CreateFloatingIcon("📷");
        }

        private void CreateFloatingIcon(string emoji)
        {
            GameObject icon = new GameObject("FloatingIcon");
            icon.transform.position = transform.position + Vector3.up * 2f;

            var textMesh = icon.AddComponent<TextMesh>();
            textMesh.text = emoji;
            textMesh.fontSize = 48;
            textMesh.anchor = TextAnchor.MiddleCenter;

            icon.transform.LookAt(Camera.main.transform);
            StartCoroutine(AnimateFloatingIcon(icon));
        }

        private System.Collections.IEnumerator AnimateFloatingIcon(GameObject icon)
        {
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                icon.transform.position += Vector3.up * Time.deltaTime * 0.5f;
                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(icon);
        }

        private void OnDrawGizmosSelected()
        {
            // Draw camera position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_photoPosition, 0.3f);

            // Draw camera direction
            Gizmos.DrawLine(_photoPosition, _photoPosition + _photoRotation * Vector3.forward * 2f);

            // Draw FOV cone
            Gizmos.color = Color.cyan;
            Vector3 forward = _photoRotation * Vector3.forward;
            Vector3 up = _photoRotation * Vector3.up;
            Vector3 right = _photoRotation * Vector3.right;

            float fovRad = _photoFOV * Mathf.Deg2Rad;
            float height = 2f * Mathf.Tan(fovRad / 2f);
            float width = height * Camera.main.aspect;

            Vector3 topLeft = _photoPosition + forward * 2f + up * (height / 2f) - right * (width / 2f);
            Vector3 topRight = _photoPosition + forward * 2f + up * (height / 2f) + right * (width / 2f);
            Vector3 bottomLeft = _photoPosition + forward * 2f - up * (height / 2f) - right * (width / 2f);
            Vector3 bottomRight = _photoPosition + forward * 2f - up * (height / 2f) + right * (width / 2f);

            Gizmos.DrawLine(_photoPosition, topLeft);
            Gizmos.DrawLine(_photoPosition, topRight);
            Gizmos.DrawLine(_photoPosition, bottomLeft);
            Gizmos.DrawLine(_photoPosition, bottomRight);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }

    /// <summary>
    /// Verse Marker - reveals a Bible verse
    /// </summary>
    public class VerseMarkerPOI : Interactable
    {
        [Header("Verse Data")]
        [SerializeField] private string _reference;
        [SerializeField, TextArea(3, 6)] private string _verseTextKey;
        [SerializeField] private string _translation = "NVI";

        [Header("Settings")]
        [SerializeField] private bool _addToLibrary = true;
        [SerializeField] private int _coinReward = 5;

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Show verse
            string verseText = LocalizationManager.Instance?.GetLocalizedString(_verseTextKey) ?? _verseTextKey;
            UIManager.Instance?.ShowVerse(_reference, verseText, _translation);

            // Give rewards
            GameManager.Instance?.AddCoins(_coinReward);

            // Add to library
            if (_addToLibrary)
            {
                var playerData = GameManager.Instance?.GetPlayerData();
                if (playerData != null)
                {
                    playerData.InventoryItems.Add(new InventoryItem
                    {
                        ItemId = _reference,
                        ItemType = "Verse",
                        AcquiredDate = System.DateTime.UtcNow,
                        IsNew = true
                    });
                }
            }

            Debug.Log($"[VerseMarker] Showing verse: {_reference}");

            if (_singleUse)
            {
                MarkAsUsed();
            }
        }

        protected override void ShowHighlight()
        {
            base.ShowHighlight();
            CreateFloatingIcon("✝️");
        }

        private void CreateFloatingIcon(string emoji)
        {
            GameObject icon = new GameObject("FloatingIcon");
            icon.transform.position = transform.position + Vector3.up * 2f;

            var textMesh = icon.AddComponent<TextMesh>();
            textMesh.text = emoji;
            textMesh.fontSize = 48;
            textMesh.anchor = TextAnchor.MiddleCenter;

            icon.transform.LookAt(Camera.main.transform);
            StartCoroutine(AnimateFloatingIcon(icon));
        }

        private System.Collections.IEnumerator AnimateFloatingIcon(GameObject icon)
        {
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                icon.transform.position += Vector3.up * Time.deltaTime * 0.5f;
                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(icon);
        }
    }

    /// <summary>
    /// Puzzle Trigger - starts a puzzle
    /// </summary>
    public class PuzzleTriggerPOI : Interactable
    {
        [Header("Puzzle Data")]
        [SerializeField] private PuzzleDataSO _puzzleData;

        [Header("Settings")]
        [SerializeField] private bool _autoStart = true;

        public PuzzleDataSO PuzzleData => _puzzleData;

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            if (_puzzleData == null)
            {
                Debug.LogError("[PuzzleTrigger] No puzzle data assigned!");
                return;
            }

            // Start puzzle
            PuzzleManager.Instance?.StartPuzzle(_puzzleData.puzzleId, _puzzleData.puzzleType);

            Debug.Log($"[PuzzleTrigger] Starting puzzle: {_puzzleData.puzzleId}");

            if (_singleUse)
            {
                MarkAsUsed();
            }
        }

        protected override void ShowHighlight()
        {
            base.ShowHighlight();
            CreateFloatingIcon("🧩");
        }

        private void CreateFloatingIcon(string emoji)
        {
            GameObject icon = new GameObject("FloatingIcon");
            icon.transform.position = transform.position + Vector3.up * 2f;

            var textMesh = icon.AddComponent<TextMesh>();
            textMesh.text = emoji;
            textMesh.fontSize = 48;
            textMesh.anchor = TextAnchor.MiddleCenter;

            icon.transform.LookAt(Camera.main.transform);
            StartCoroutine(AnimateFloatingIcon(icon));
        }

        private System.Collections.IEnumerator AnimateFloatingIcon(GameObject icon)
        {
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                icon.transform.position += Vector3.up * Time.deltaTime * 0.5f;
                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(icon);
        }
    }

    /// <summary>
    /// Phase Exit - completes the phase and returns to menu or next phase
    /// </summary>
    public class PhaseExit : Interactable
    {
        [Header("Exit Settings")]
        [SerializeField] private string _nextPhaseId;
        [SerializeField] private bool _returnToHub = false;

        [Header("Completion")]
        [SerializeField] private bool _requireAllPOIs = false;

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Check if player has visited all POIs (if required)
            if (_requireAllPOIs)
            {
                var playerData = GameManager.Instance?.GetPlayerData();
                int currentPhaseProgress = playerData?.CompletedPhases.FindAll(p => p.Contains("1.1")).Count ?? 0;

                int requiredPOIs = 6; // Adjust based on phase
                if (currentPhaseProgress < requiredPOIs)
                {
                    UIManager.Instance?.ShowInfoPanel(
                        "Phase Not Complete",
                        $"You haven't explored everything yet! ({currentPhaseProgress}/{requiredPOIs} POIs visited)",
                        null
                    );
                    return;
                }
            }

            // Complete phase
            CompletePhase();
        }

        private void CompletePhase()
        {
            // Calculate completion time
            float playTime = Time.time;

            // Calculate stars based on POIs visited, puzzles solved, etc.
            int stars = CalculateStars();

            // Award rewards
            GameManager.Instance?.CompletePhase("1.1", stars, playTime);

            // Play completion sound
            AudioManager.Instance?.PlayPhaseCompleteStinger();

            // Show completion screen
            ShowCompletionScreen(stars);

            Debug.Log($"[PhaseExit] Phase 1.1 completed with {stars} stars!");
        }

        private int CalculateStars()
        {
            var playerData = GameManager.Instance?.GetPlayerData();
            int poiCount = playerData?.CompletedPhases.FindAll(p => p.Contains("1.1")).Count ?? 0;

            if (poiCount >= 6) return 3;
            if (poiCount >= 4) return 2;
            return 1;
        }

        private void ShowCompletionScreen(int stars)
        {
            // TODO: Create completion screen UI
            UIManager.Instance?.ShowInfoPanel(
                "Phase Complete!",
                $"You completed Phase 1.1: Bethlehem\nStars: {new string('⭐', stars)}\nReturning to menu...",
                null
            );

            // Return to main menu after delay
            Invoke("LoadMainMenu", 3f);
        }

        private void LoadMainMenu()
        {
            GameManager.Instance?.LoadPhase("MainMenu");
        }

        protected override void ShowHighlight()
        {
            base.ShowHighlight();
            CreateFloatingIcon("🚪");
        }

        private void CreateFloatingIcon(string emoji)
        {
            GameObject icon = new GameObject("FloatingIcon");
            icon.transform.position = transform.position + Vector3.up * 2f;

            var textMesh = icon.AddComponent<TextMesh>();
            textMesh.text = emoji;
            textMesh.fontSize = 48;
            textMesh.anchor = TextAnchor.MiddleCenter;

            icon.transform.LookAt(Camera.main.transform);
            StartCoroutine(AnimateFloatingIcon(icon));
        }

        private System.Collections.IEnumerator AnimateFloatingIcon(GameObject icon)
        {
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                icon.transform.position += Vector3.up * Time.deltaTime * 0.5f;
                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(icon);
        }
    }

    /// <summary>
    /// Collectible Item - coins, artifacts, etc.
    /// </summary>
    public class CollectibleItem : Interactable
    {
        [Header("Item Data")]
        [SerializeField] private string _itemId;
        [SerializeField] private CollectibleType _itemType;
        [SerializeField] private Sprite _itemIcon;

        [Header("Rewards")]
        [SerializeField] private int _coinAmount = 10;

        public enum CollectibleType
        {
            Coin,
            Artifact,
            Document,
            Cosmetic
        }

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Give rewards based on type
            switch (_itemType)
            {
                case CollectibleType.Coin:
                    GameManager.Instance?.AddCoins(_coinAmount);
                    break;

                case CollectibleType.Artifact:
                    var playerData = GameManager.Instance?.GetPlayerData();
                    if (playerData != null)
                    {
                        playerData.InventoryItems.Add(new InventoryItem
                        {
                            ItemId = _itemId,
                            ItemType = "Artifact",
                            AcquiredDate = System.DateTime.UtcNow,
                            IsNew = true
                        });
                    }
                    break;

                case CollectibleType.Document:
                    playerData = GameManager.Instance?.GetPlayerData();
                    if (playerData != null)
                    {
                        playerData.InventoryItems.Add(new InventoryItem
                        {
                            ItemId = _itemId,
                            ItemType = "Document",
                            AcquiredDate = System.DateTime.UtcNow,
                            IsNew = true
                        });
                    }
                    break;

                case CollectibleType.Cosmetic:
                    playerData = GameManager.Instance?.GetPlayerData();
                    if (playerData != null)
                    {
                        playerData.UnlockedCosmetics.Add(_itemId);
                    }
                    break;
            }

            // Play pickup sound
            AudioManager.Instance?.PlaySFX(null);

            // Destroy collectible
            Destroy(gameObject);

            Debug.Log($"[Collectible] Collected: {_itemId} ({_itemType})");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Interact(other.gameObject);
            }
        }

        protected override void ShowHighlight()
        {
            // Collectibles have continuous glow effect
            GetComponentInChildren<Light>()?.gameObject.SetActive(true);
        }

        protected override void HideHighlight()
        {
            // Keep glow for collectibles
        }
    }
}
