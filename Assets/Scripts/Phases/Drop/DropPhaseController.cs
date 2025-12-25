using UnityEngine;
using Drip.Managers;

namespace Drip.Phases.Drop
{
    public class DropPhaseController : MonoBehaviour, IPhaseController
    {
        [Header("Drop Settings")]
        [SerializeField] private int dropletsPerSecond = 5;
        [SerializeField] private float phaseDuration = 30f; // How long drops fall
        [SerializeField] private GameObject dropletPrefab;
        [SerializeField] private Transform dropSpawnPoint;

        [Header("Collection")]
        [SerializeField] private int waterPerDroplet = 1;

        private bool phaseActive = false;
        private float phaseTimer = 0f;
        private float dropTimer = 0f;
        private int totalWaterCollected = 0;

        public void EnterPhase()
        {
            phaseActive = true;
            phaseTimer = 0f;
            totalWaterCollected = 0;
            Debug.Log("Entered Drop Phase");
        }

        public void ExitPhase()
        {
            phaseActive = false;
            
            // Add collected water to current player
            if (GameManager.Instance.CurrentPlayer != null)
            {
                GameManager.Instance.AddWaterToPlayer(
                    GameManager.Instance.CurrentPlayer.playerID, 
                    totalWaterCollected
                );
            }
            
            Debug.Log($"Exited Drop Phase - Collected {totalWaterCollected} water");
        }

        public bool IsPhaseComplete()
        {
            return phaseTimer >= phaseDuration;
        }

        private void Update()
        {
            if (!phaseActive) return;

            phaseTimer += Time.deltaTime;
            dropTimer += Time.deltaTime;

            float dropInterval = 1f / dropletsPerSecond;
            
            if (dropTimer >= dropInterval)
            {
                SpawnDroplet();
                dropTimer = 0f;
            }

            if (IsPhaseComplete())
            {
                GameManager.Instance.AdvancePhase();
            }
        }

        private void SpawnDroplet()
        {
            if (dropletPrefab != null && dropSpawnPoint != null)
            {
                // Random horizontal position
                Vector3 spawnPos = dropSpawnPoint.position;
                spawnPos.x += Random.Range(-2f, 2f);
                
                GameObject droplet = Instantiate(dropletPrefab, spawnPos, Quaternion.identity);
                WaterDroplet dropletComponent = droplet.GetComponent<WaterDroplet>();
                
                if (dropletComponent != null)
                {
                    dropletComponent.OnCollected += HandleDropletCollected;
                }
            }
        }

        private void HandleDropletCollected()
        {
            totalWaterCollected += waterPerDroplet;
        }
    }
}
