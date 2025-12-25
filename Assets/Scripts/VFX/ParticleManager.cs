using UnityEngine;
using Drip.Utilities;

namespace Drip.VFX
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance { get; private set; }

        [Header("Water Effects")]
        [SerializeField] private ParticleSystem waterSplashPrefab;
        [SerializeField] private ParticleSystem waterCollectionPrefab;
        [SerializeField] private ParticleSystem waterDropletTrail;

        [Header("Combat Effects")]
        [SerializeField] private ParticleSystem balloonHitEffect;
        [SerializeField] private ParticleSystem balloonDestroyEffect;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayWaterSplash(Vector3 position)
        {
            if (waterSplashPrefab != null)
            {
                ParticleSystem splash = Instantiate(waterSplashPrefab, position, Quaternion.identity);
                Destroy(splash.gameObject, splash.main.duration);
            }
        }

        public void PlayWaterCollection(Vector3 position)
        {
            if (waterCollectionPrefab != null)
            {
                ParticleSystem collection = Instantiate(waterCollectionPrefab, position, Quaternion.identity);
                Destroy(collection.gameObject, collection.main.duration);
            }
        }

        public void PlayBalloonHit(Vector3 position)
        {
            if (balloonHitEffect != null)
            {
                ParticleSystem hit = Instantiate(balloonHitEffect, position, Quaternion.identity);
                Destroy(hit.gameObject, hit.main.duration);
            }
        }

        public void PlayBalloonDestroy(Vector3 position)
        {
            if (balloonDestroyEffect != null)
            {
                ParticleSystem destroy = Instantiate(balloonDestroyEffect, position, Quaternion.identity);
                Destroy(destroy.gameObject, destroy.main.duration);
            }
        }
    }
}
