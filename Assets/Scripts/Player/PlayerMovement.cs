using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement Current { get; private set; }

        public float moveSpeed;

        [SerializeField] private Rigidbody2D rb;
        private Vector2 _moveDirection;

        [Header("Stretch & Squash Settings")] [SerializeField]
        private float stretchAmount = 0.3f; // How much to stretch/squash (0.3 = 30% change)

        [SerializeField] private float stretchSpeed = 8f; // How fast the stretch/squash animates
        [SerializeField] private float returnSpeed = 12f; // How fast it returns to normal when not moving
        [SerializeField] private float minimumMovementThreshold = 0.1f; // Minimum input needed to trigger effect

        private Vector3 _originalScale;
        private Vector3 _targetScale;
        private Transform _transform;

        private void Awake()
        {
            Current = this;
            _transform = transform;
            _originalScale = _transform.localScale;
            _targetScale = _originalScale;
        }

        private void Update()
        {
            ProcessInputs();
            UpdateStretchSquash();
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        }

        private void ProcessInputs()
        {
            float moveX = PlayerInput.GetAxisRaw("Horizontal");
            float moveY = PlayerInput.GetAxisRaw("Vertical");
            _moveDirection = new Vector2(moveX, moveY).normalized;
        }

        void UpdateStretchSquash()
        {
            // Get raw input for stretch/squash (we want the raw values, not normalized)
            float rawMoveX = Input.GetAxisRaw("Horizontal");
            float rawMoveY = Input.GetAxisRaw("Vertical");

            // Calculate target scale based on movement
            Vector3 newTargetScale = _originalScale;

            // Horizontal movement - stretch horizontally, compress vertically slightly
            if (Mathf.Abs(rawMoveX) > minimumMovementThreshold)
            {
                float horizontalStretch = 1f + (stretchAmount * Mathf.Abs(rawMoveX));
                float verticalCompress = 1f - (stretchAmount * 0.5f * Mathf.Abs(rawMoveX));

                newTargetScale.x = _originalScale.x * horizontalStretch;
                newTargetScale.y = _originalScale.y * verticalCompress;
            }
            // Vertical movement - squash vertically, compress horizontally slightly
            else if (Mathf.Abs(rawMoveY) > minimumMovementThreshold)
            {
                float verticalSquash = 1f - (stretchAmount * Mathf.Abs(rawMoveY));
                float horizontalCompress = 1f - (stretchAmount * 0.5f * Mathf.Abs(rawMoveY));

                newTargetScale.x = _originalScale.x * horizontalCompress;
                newTargetScale.y = _originalScale.y * verticalSquash;
            }

            _targetScale = newTargetScale;

            // Smoothly interpolate to target scale
            float lerpSpeed = (_targetScale == _originalScale) ? returnSpeed : stretchSpeed;
            _transform.localScale = Vector3.Lerp(_transform.localScale, _targetScale, lerpSpeed * Time.deltaTime);
        }

        // Optional: Call this if you want to reset the scale immediately
        public void ResetScale()
        {
            _transform.localScale = _originalScale;
            _targetScale = _originalScale;
        }
    }
}