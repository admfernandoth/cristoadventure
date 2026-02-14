using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace CristoAdventure.Systems
{
    /// <summary>
    /// Minimap system showing player position and POIs
    /// Inspired by Kingshot's minimap design
    /// </summary>
    public class MinimapSystem : MonoBehaviour
    {
        #region Singleton

        private static MinimapSystem _instance;
        public static MinimapSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_MinimapSystem");
                    _instance = go.AddComponent<MinimapSystem>();
                }
                return _instance;
            }
        }

        #endregion

        #region UI References

        [Header("Minimap UI")]
        [SerializeField] private RawImage _minimapImage;
        [SerializeField] private Transform _playerIndicator;
        [SerializeField] private Transform _poiContainer;
        [SerializeField] private GameObject _poiIconPrefab;

        [Header("Settings")]
        [SerializeField] private float _mapSize = 100f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private bool _rotateWithPlayer = true;
        [SerializeField] private Camera _mapCamera;

        #endregion

        #region State

        private Transform _playerTransform;
        private List<MinimapPOI> _pois = new List<MinimapPOI>();
        private RenderTexture _mapTexture;

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
        }

        private void Start()
        {
            // Find player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }

            // Create render texture for minimap
            CreateMapTexture();

            // Initialize POI icons
            InitializePOIIcons();

            Debug.Log("[MinimapSystem] Initialized");
        }

        private void OnDestroy()
        {
            if (_mapTexture != null)
            {
                _mapTexture.Release();
            }
        }

        private void LateUpdate()
        {
            if (_playerTransform == null) return;

            // Update player indicator position
            UpdatePlayerIndicator();

            // Update minimap rotation
            if (_rotateWithPlayer)
            {
                UpdateMinimapRotation();
            }

            // Update POI icons
            UpdatePOIIcons();
        }

        #endregion

        #region Initialization

        private void CreateMapTexture()
        {
            // Create render texture for the minimap
            _mapTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            _mapTexture.antiAliasing = 2;

            if (_minimapImage != null)
            {
                _minimapImage.texture = _mapTexture;
            }

            // Setup camera for top-down view
            if (_mapCamera != null)
            {
                _mapCamera.targetTexture = _mapTexture;
                _mapCamera.orthographic = true;
                _mapCamera.orthographicSize = _mapSize / 2f;
                _mapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            }
        }

        private void InitializePOIIcons()
        {
            // Clear existing icons
            foreach (Transform child in _poiContainer)
            {
                Destroy(child.gameObject);
            }

            // Find all POIs in the scene
            var pois = FindObjectsOfType<Interactable>();

            foreach (var poi in pois)
            {
                CreatePOIIcon(poi);
            }
        }

        private void CreatePOIIcon(Interactable poi)
        {
            if (_poiIconPrefab == null || _poiContainer == null) return;

            GameObject icon = Instantiate(_poiIconPrefab, _poiContainer);

            var poiData = new MinimapPOI
            {
                transform = poi.transform,
                icon = icon.transform,
                type = GetPOIType(poi),
                isVisited = false
            };

            _pois.Add(poiData);

            // Set icon color based on type
            SetIconColor(poiData);
        }

        #endregion

        #region Updates

        private void UpdatePlayerIndicator()
        {
            if (_playerIndicator == null) return;

            // Position player indicator in center of minimap
            _playerIndicator.localPosition = Vector3.zero;

            // Rotate indicator to match player direction
            Vector3 playerForward = _playerTransform.forward;
            float angle = Mathf.Atan2(playerForward.x, playerForward.z) * Mathf.Rad2Deg;
            _playerIndicator.rotation = Quaternion.Euler(0f, 0f, -angle);
        }

        private void UpdateMinimapRotation()
        {
            if (_poiContainer == null) return;

            // Rotate POI container opposite to player rotation
            Vector3 playerForward = _playerTransform.forward;
            float angle = Mathf.Atan2(playerForward.x, playerForward.z) * Mathf.Rad2Deg;
            _poiContainer.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void UpdatePOIIcons()
        {
            foreach (var poi in _pois)
            {
                if (poi.transform == null || poi.icon == null) continue;

                // Calculate position relative to player
                Vector3 offset = poi.transform.position - _playerTransform.position;

                // Scale to minimap size
                float minimapScale = 100f / _mapSize;
                Vector3 minimapPos = new Vector3(offset.x * minimapScale, offset.z * minimapScale, 0f);

                // Clamp to minimap bounds
                float maxPos = 50f; // Half of minimap size (in pixels)
                minimapPos.x = Mathf.Clamp(minimapPos.x, -maxPos, maxPos);
                minimapPos.y = Mathf.Clamp(minimapPos.y, -maxPos, maxPos);

                poi.icon.localPosition = minimapPos;

                // Hide if too far
                float distance = offset.magnitude;
                poi.icon.gameObject.SetActive(distance < _mapSize);
            }
        }

        #endregion

        #region POI Management

        private POIType GetPOIType(Interactable poi)
        {
            if (poi is InfoPlaque) return POIType.Info;
            if (poi is ReliquaryPOI) return POIType.Relic;
            if (poi is NPCGuidePOI) return POIType.NPC;
            if (poi is PhotoSpotPOI) return POIType.Photo;
            if (poi is VerseMarkerPOI) return POIType.Verse;
            if (poi is PuzzleTriggerPOI) return POIType.Puzzle;
            if (poi is PhaseExit) return POIType.Exit;

            return POIType.Other;
        }

        private void SetIconColor(MinimapPOI poi)
        {
            var image = poi.icon.GetComponent<Image>();
            if (image == null) return;

            Color color = Color.white;

            switch (poi.type)
            {
                case POIType.Info:
                    color = new Color(0.2f, 0.6f, 1f); // Blue
                    break;
                case POIType.Relic:
                    color = new Color(1f, 0.8f, 0.2f); // Gold
                    break;
                case POIType.NPC:
                    color = new Color(0.4f, 0.8f, 0.4f); // Green
                    break;
                case POIType.Photo:
                    color = new Color(0.8f, 0.4f, 0.8f); // Purple
                    break;
                case POIType.Verse:
                    color = new Color(0.6f, 0.3f, 0.6f); // Magenta
                    break;
                case POIType.Puzzle:
                    color = new Color(1f, 0.6f, 0.2f); // Orange
                    break;
                case POIType.Exit:
                    color = new Color(0.4f, 1f, 0.4f); // Bright Green
                    break;
            }

            // Dim visited POIs
            if (poi.isVisited)
            {
                color *= 0.5f;
            }

            image.color = color;
        }

        public void MarkPOIVisited(Interactable poi)
        {
            foreach (var minimapPOI in _pois)
            {
                if (minimapPOI.transform == poi.transform)
                {
                    minimapPOI.isVisited = true;
                    SetIconColor(minimapPOI);
                    break;
                }
            }
        }

        public void RefreshPOIs()
        {
            InitializePOIIcons();
        }

        #endregion

        #region Public API

        public void SetPlayer(Transform player)
        {
            _playerTransform = player;
        }

        public void SetMapSize(float size)
        {
            _mapSize = size;
            if (_mapCamera != null)
            {
                _mapCamera.orthographicSize = size / 2f;
            }
        }

        #endregion

        #region Inner Classes

        private class MinimapPOI
        {
            public Transform transform;
            public Transform icon;
            public POIType type;
            public bool isVisited;
        }

        private enum POIType
        {
            Info,
            Relic,
            NPC,
            Photo,
            Verse,
            Puzzle,
            Exit,
            Other
        }

        #endregion
    }
}
