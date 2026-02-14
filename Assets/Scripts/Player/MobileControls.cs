using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CristoAdventure.Player
{
    /// <summary>
    /// Mobile touch controls for on-screen joystick and buttons
    /// </summary>
    public class MobileControls : MonoBehaviour
    {
        #region Joystick

        [Header("Virtual Joystick")]
        [SerializeField] private RectTransform _joystickBackground;
        [SerializeField] private RectTransform _joystickHandle;
        [SerializeField] private float _joystickRange = 50f;

        private Vector2 _joystickCenter;
        private bool _isDragging = false;
        private Vector2 _moveInput;

        #endregion

        #region Action Buttons

        [Header("Action Buttons")]
        [SerializeField] private Button _interactButton;
        [SerializeField] private Button _runButton;
        [SerializeField] private Button _backpackButton;
        [SerializeField] private Button _pauseButton;

        #endregion

        #region Camera Controls

        [Header("Camera Controls")]
        [SerializeField] private bool _enableTouchLook = true;
        [SerializeField] private float _touchSensitivity = 1f;
        [SerializeField] private float _doubleTapZoomFOV = 40f;

        private Vector2 _touchLookInput;
        private float _defaultFOV;
        private int _touchCount = 0;

        #endregion

        #region References

        private PlayerController _playerController;
        private Camera _mainCamera;

        #endregion

        #region Unity Lifecycle

        private void Start()
        {
            // Find player controller
            _playerController = FindObjectOfType<PlayerController>();
            _mainCamera = Camera.main;

            if (_mainCamera != null)
            {
                _defaultFOV = _mainCamera.fieldOfView;
            }

            // Setup joystick
            InitializeJoystick();

            // Setup buttons
            SetupButtons();

            // Check if mobile platform
            #if !UNITY_ANDROID && !UNITY_IOS
            gameObject.SetActive(false);
            #endif
        }

        private void Update()
        {
            HandleTouchLook();
        }

        #endregion

        #region Joystick

        private void InitializeJoystick()
        {
            if (_joystickBackground != null)
            {
                _joystickCenter = _joystickBackground.position;
            }
        }

        public void OnJoystickPointerDown(BaseEventData data)
        {
            _isDragging = true;
            UpdateJoystickPosition(data);
        }

        public void OnJoystickDrag(BaseEventData data)
        {
            if (_isDragging)
            {
                UpdateJoystickPosition(data);
            }
        }

        public void OnJoystickPointerUp(BaseEventData data)
        {
            _isDragging = false;
            ResetJoystick();
        }

        private void UpdateJoystickPosition(BaseEventData data)
        {
            PointerEventData pointerData = data as PointerEventData;
            if (pointerData == null || _joystickHandle == null) return;

            Vector2 position = pointerData.position;
            Vector2 offset = position - _joystickCenter;

            // Clamp to range
            offset = Vector2.ClampMagnitude(offset, _joystickRange);

            // Update handle position
            _joystickHandle.position = _joystickCenter + offset;

            // Calculate normalized input
            _moveInput = offset / _joystickRange;

            // Send to player controller
            if (_playerController != null)
            {
                // Simulate input action
                // Note: In production, use Unity's new input system properly
            }
        }

        private void ResetJoystick()
        {
            if (_joystickHandle != null)
            {
                _joystickHandle.position = _joystickCenter;
            }
            _moveInput = Vector2.zero;
        }

        #endregion

        #region Camera Controls

        private void HandleTouchLook()
        {
            if (!_enableTouchLook) return;

            // Count touches
            _touchCount = Input.touchCount;

            // Two-finger camera look
            if (_touchCount == 2 && !IsPointerOverUI())
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                // Calculate delta
                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
                    Vector2 delta0 = touch0.deltaPosition;
                    Vector2 delta1 = touch1.deltaPosition;

                    float deltaX = (delta0.x + delta1.x) * 0.5f * _touchSensitivity;
                    float deltaY = (delta0.y + delta1.y) * 0.5f * _touchSensitivity;

                    _touchLookInput = new Vector2(deltaX, deltaY);

                    // Apply camera rotation
                    // CameraController would handle this
                }
            }
        }

        #endregion

        #region Button Handlers

        private void SetupButtons()
        {
            if (_interactButton != null)
                _interactButton.onClick.AddListener(OnInteractButton);

            if (_runButton != null)
                _runButton.onClick.AddListener(OnRunButton);

            if (_backpackButton != null)
                _backpackButton.onClick.AddListener(OnBackpackButton);

            if (_pauseButton != null)
                _pauseButton.onClick.AddListener(OnPauseButton);
        }

        private void OnInteractButton()
        {
            _playerController?.GetComponent<InteractionSystem>()?.TryInteract();
        }

        private void OnRunButton()
        {
            // Toggle run mode
            // PlayerController would handle this
        }

        private void OnBackpackButton()
        {
            GameManager.Instance?.SetState(GameManager.GameState.InBackpack);
        }

        private void OnPauseButton()
        {
            GameManager.Instance?.SetState(GameManager.GameState.Paused);
        }

        #endregion

        #region Utility

        private bool IsPointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        #endregion

        #region Properties

        public Vector2 MoveInput => _moveInput;
        public Vector2 TouchLookInput => _touchLookInput;

        #endregion
    }

    /// <summary>
    /// Simple extension to check for UI pointer
    /// </summary>
    public static class EventSystemExtensions
    {
        public static bool IsPointerOverGameObject()
        {
            return EventSystem.current != null &&
                   EventSystem.current.currentSelectedGameObject != null;
        }
    }
}
