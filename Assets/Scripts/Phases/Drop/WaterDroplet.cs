using System;
using UnityEngine;

namespace Drip.Phases.Drop
{
    [RequireComponent(typeof(Rigidbody))]
    public class WaterDroplet : MonoBehaviour
    {
        [Header("Physics")]
        [SerializeField] private float fallSpeed = 5f;
        [SerializeField] private float lifetime = 10f;

        private Rigidbody rb;
        private float aliveTime = 0f;

        public event Action OnCollected;
        public event Action OnMissed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
        }

        private void Start()
        {
            // Apply initial downward velocity
            rb.velocity = Vector3.down * fallSpeed;
        }

        private void Update()
        {
            aliveTime += Time.deltaTime;
            
            if (aliveTime >= lifetime)
            {
                OnMissed?.Invoke();
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bucket"))
            {
                Collect();
            }
            else if (other.CompareTag("Ground"))
            {
                Miss();
            }
        }

        private void Collect()
        {
            OnCollected?.Invoke();
            
            // Play collection effect
            // AudioManager.Instance?.PlaySound("WaterCollect");
            
            Destroy(gameObject);
        }

        private void Miss()
        {
            OnMissed?.Invoke();
            Destroy(gameObject);
        }
    }
}
