using UnityEngine;
using UnityEngine.InputSystem;

namespace CristoAdventure.Player
{
    /// <summary>
    /// Controls player movement and interaction in 3D exploration mode
    /// Inspired by Kingshot's smooth third-person exploration controls
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        #region Components

        private CharacterController _characterController;
        private Camera _mainCamera;
        private Animator _animator;

        #endregion

        #region Movement Settings

        [Header("Movement")]
        [SerializeField] private float _walkSpeed = 3f;
        [SerializeField] private float _runSpeed = 6f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _gravity = -9.81f;

        [Header("Look")]
        [SerializeField] private float _lookSensitivity = 2f;
        [SerializeField] private float _minVerticalAngle = -30f;
        [SerializeField] private float _maxVerticalAngle = 60f;

        [Header("Interaction")]
        [SerializeField] private float _interactionRange = 3f;
        [SerializeField] private LayerMask _interactionLayer;

        #endregion

        #region State

        private Vector3 _velocity;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private bool _isRunning;
        private bool _isInteracting;

        private float _verticalCameraAngle = 0f;

        #endregion

        #region References

        private Interactable _currentInteractable;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing)
                return;

            HandleMovement();
            HandleLook();
            HandleInteraction();
            ApplyGravity();
        }

        #endregion

        #region Input Handling (New Input System)

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            _isRunning = context.performed;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryInteract();
            }
        }

        public void OnOpenBackpack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // Open backpack UI
                GameManager.Instance.SetState(GameManager.GameState.InBackpack);
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameManager.Instance.SetState(GameManager.GameState.Paused);
            }
        }

        #endregion

        #region Movement

        private void HandleMovement()
        {
            // Calculate move direction relative to camera
            Vector3 forward = _mainCamera.transform.forward;
            Vector3 right = _mainCamera.transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * _moveInput.y + right * _moveInput.x).normalized;

            // Apply speed
            float currentSpeed = _isRunning ? _runSpeed : _walkSpeed;
            Vector3 movement = moveDirection * currentSpeed;

            // Apply to character controller
            _characterController.Move(movement * Time.deltaTime);

            // Rotate character to face movement direction
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );

                // Update animator
                if (_animator != null)
                {
                    _animator.SetFloat("MoveSpeed", moveDirection.magnitude);
                    _animator.SetBool("IsRunning", _isRunning);
                }
            }
            else
            {
                if (_animator != null)
                {
                    _animator.SetFloat("MoveSpeed", 0f);
                }
            }
        }

        private void ApplyGravity()
        {
            if (_characterController.isGrounded)
            {
                if (_velocity.y < 0)
                {
                    _velocity.y = -2f; // Small downward force to stay grounded
                }
            }
            else
            {
                _velocity.y += _gravity * Time.deltaTime;
            }

            _characterController.Move(_velocity * Time.deltaTime);
        }

        #endregion

        #region Camera Look

        private void HandleLook()
        {
            // For touch/mouse look in exploration mode
            float lookX = _lookInput.x * _lookSensitivity;
            float lookY = _lookInput.y * _lookSensitivity;

            // Horizontal rotation - rotate camera around player
            if (_mainCamera != null)
            {
                Transform cameraTransform = _mainCamera.transform;

                // Rotate camera around player horizontally
                cameraTransform.RotateAround(transform.position, Vector3.up, lookX);

                // Vertical rotation - clamp angle
                _verticalCameraAngle -= lookY;
                _verticalCameraAngle = Mathf.Clamp(_verticalCameraAngle, _minVerticalAngle, _maxVerticalAngle);

                // Apply vertical rotation
                Vector3 offset = cameraTransform.position - transform.position;
                cameraTransform.position = transform.position + Quaternion.Euler(_verticalCameraAngle, 0, 0) * offset;
                cameraTransform.LookAt(transform.position + Vector3.up * 1.5f); // Look at player's upper body
            }
        }

        #endregion

        #region Interaction

        private void HandleInteraction()
        {
            // Check for nearby interactables
            Collider[] hits = Physics.OverlapSphere(transform.position, _interactionRange, _interactionLayer);

            Interactable nearestInteractable = null;
            float nearestDistance = float.MaxValue;

            foreach (var hit in hits)
            {
                var interactable = hit.GetComponent<Interactable>();
                if (interactable != null && interactable.CanInteract())
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestInteractable = interactable;
                    }
                }
            }

            // Update current interactable
            if (nearestInteractable != _currentInteractable)
            {
                if (_currentInteractable != null)
                {
                    _currentInteractable.OnDeselect();
                }

                _currentInteractable = nearestInteractable;

                if (_currentInteractable != null)
                {
                    _currentInteractable.OnSelect();
                    // Show interaction prompt
                    UIManager.Instance?.ShowInteractionPrompt(_currentInteractable.PromptMessage);
                }
                else
                {
                    UIManager.Instance?.HideInteractionPrompt();
                }
            }
        }

        private void TryInteract()
        {
            if (_currentInteractable != null && _currentInteractable.CanInteract())
            {
                _isInteracting = true;
                _currentInteractable.Interact(gameObject);
                _isInteracting = false;
            }
        }

        #endregion

        #region Public Methods

        public void Teleport(Vector3 position)
        {
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }

        public void SetCanMove(bool canMove)
        {
            enabled = canMove;
        }

        public Vector3 GetPosition() => transform.position;

        #endregion

        #region Debug

        private void OnDrawGizmosSelected()
        {
            // Draw interaction range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _interactionRange);
        }

        #endregion
    }
}
