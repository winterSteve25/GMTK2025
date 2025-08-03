using UnityEngine;

namespace Zones
{
    public class Zone : MonoBehaviour
    {
        public static bool PlayerInZone { get; private set; } = true;
        
        [SerializeField] private float radius;
        [SerializeField] private CircleCollider2D col;

        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                transform.localScale = new Vector3(radius * 2, radius * 2, 1);
            }
        }

        private void OnValidate()
        {
            transform.localScale = new Vector3(radius * 2, radius * 2, 1);
        }

        public bool InZone(Vector2 position)
        {
            return Vector2.Distance(transform.position, position) <= radius;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerInZone = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Bullet"))
            {
                var delta = other.transform.position - transform.position;
                var angle = Mathf.Deg2Rad * (Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg + 180);
                var position = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * this.radius);
                other.transform.position = new Vector3(position.x, position.y, 0) + transform.position;
            }

            if (other.CompareTag("Player"))
            {
                PlayerInZone = false;
            }
        }
    }
}