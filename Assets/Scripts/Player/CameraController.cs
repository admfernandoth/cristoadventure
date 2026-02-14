using UnityEngine;

namespace CristoAdventure.Player
{
    /// <summary>
    /// Cinematic camera controller for exploration mode
    /// Inspired by Kingshot's smooth camera movements
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        #region Settings

        [Header("Camera Settings")]
        [SerializeField] private float _defaultFOV = 60f;
        [SerializeField] private float _zoomFOV = 40f;
        [SerializeField] private float _zoomSpeed = 5f;

        [Header("Follow Settings")]
        [SerializeField] private Vector3 _offset = new Vector3(0, 3, -6);
        [SerializeField] private float _followSmoothness = 10f;
        [SerializeField] private float _rotationSmoothness = 5f;

        [Header("Cinematic")]
        [SerializeField] private bool _enableCinematicMode = true;
        [SerializeField] private float _cinematicSmoothness = 2f;

        #endregion

        #region State

        private Transform _target;
        private Camera _camera;
        private float _currentFOV;
        private bool _isZoomed;

        // Cinematic mode
        private bool _isCinematicMode;
        private Vector3 _cinematicPosition;
        private Quaternion _cinematicRotation;
        private float _cinematicDuration;
        private float _cinematicTimer;

        // Photo mode
        private bool _isPhotoMode;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if (_camera == null)
            {
                _camera = gameObject.AddComponent<Camera>();
            }

            _currentFOV = _defaultFOV;
        }

        private void Update()
        {
            // Handle zoom
            HandleZoom();

            // Handle cinematic timer
            if (_isCinematicMode)
            {
                _cinematicTimer -= Time.deltaTime;
                if (_cinematicTimer <= 0f)
                {
                    ExitCinematicMode();
                }
            }
        }

        private void LateUpdate()
        {
            if (_isCinematicMode)
            {
                // Cinematic movement
                transform.position = Vector3.Lerp(transform.position, _cinematicPosition, _cinematicSmoothness * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, _cinematicRotation, _cinematicSmoothness * Time.deltaTime);
            }
            else if (_target != null && !_isPhotoMode)
            {
                // Follow target
                FollowTarget();
            }
        }

        #endregion

        #region Target Following

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void FollowTarget()
        {
            // Calculate target position
            Vector3 targetPosition = _target.position + _offset;

            // Smoothly move camera to target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, _followSmoothness * Time.deltaTime);

            // Look at target
            Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSmoothness * Time.deltaTime);
        }

        #endregion

        #region Zoom

        public void SetZoom(bool zoom)
        {
            _isZoomed = zoom;
        }

        private void HandleZoom()
        {
            float targetFOV = _isZoomed ? _zoomFOV : _defaultFOV;
            _currentFOV = Mathf.Lerp(_currentFOV, targetFOV, _zoomSpeed * Time.deltaTime);
            _camera.fieldOfView = _currentFOV;
        }

        #endregion

        #region Cinematic Mode

        public void EnterCinematicMode(Vector3 position, Quaternion rotation, float duration)
        {
            if (!_enableCinematicMode) return;

            _isCinematicMode = true;
            _cinematicPosition = position;
            _cinematicRotation = rotation;
            _cinematicDuration = duration;
            _cinematicTimer = duration;

            Debug.Log($"[CameraController] Entering cinematic mode for {duration} seconds");
        }

        public void ExitCinematicMode()
        {
            _isCinematicMode = false;
            Debug.Log("[CameraController] Exiting cinematic mode");
        }

        #endregion

        #region Photo Mode

        public void EnterPhotoMode()
        {
            _isPhotoMode = true;
            Debug.Log("[CameraController] Entering photo mode");
        }

        public void ExitPhotoMode()
        {
            _isPhotoMode = false;
            Debug.Log("[CameraController] Exiting photo mode");
        }

        public bool IsPhotoMode => _isPhotoMode;

        #endregion

        #region Focus on Point of Interest

        public void FocusOnPoint(Vector3 point, float duration = 2f)
        {
            EnterCinematicMode(
                point + _offset,
                Quaternion.LookRotation(point - (point + _offset)),
                duration
            );
        }

        public void FocusOnTarget(Transform target, float duration = 2f)
        {
            SetTarget(target);
            FocusOnPoint(target.position, duration);
        }

        #endregion

        #region Camera Shake

        public void Shake(float intensity, float duration)
        {
            StartCoroutine(ShakeCoroutine(intensity, duration));
        }

        private System.Collections.IEnumerator ShakeCoroutine(float intensity, float duration)
        {
            float elapsed = 0f;
            Vector3 originalPosition = transform.localPosition;

            while (elapsed < duration)
            {
                float x = UnityEngine.Random.Range(-intensity, intensity) * 0.1f;
                float y = UnityEngine.Random.Range(-intensity, intensity) * 0.1f;
                float z = UnityEngine.Random.Range(-intensity, intensity) * 0.1f;

                transform.localPosition = originalPosition + new Vector3(x, y, z);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPosition;
        }

        #endregion
    }
}
