using UnityEngine;

namespace Drip.Phases.Douse
{
    [RequireComponent(typeof(Rigidbody))]
    public class WaterProjectile : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifetime = 5f;
        [SerializeField] private int damage = 5;

        private Rigidbody rb;
        private float aliveTime = 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            rb.useGravity = false;
        }

        private void Update()
        {
            aliveTime += Time.deltaTime;
            
            if (aliveTime >= lifetime)
            {
                Destroy(gameObject);
            }
        }

        public void Launch(Vector3 direction)
        {
            rb.velocity = direction.normalized * speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerBalloon balloon = other.GetComponent<PlayerBalloon>();
            
            if (balloon != null)
            {
                balloon.TakeDamage(damage);
                
                // Play hit effect
                // AudioManager.Instance?.PlaySound("WaterHit");
                
                Destroy(gameObject);
            }
        }
    }
}
