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
        
        private void Awake()
        {
            Current = this;
        }

        private void Update()
        {
            ProcessInputs();
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
    }
}