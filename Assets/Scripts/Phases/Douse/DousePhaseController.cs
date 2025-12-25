using UnityEngine;
using Drip.Managers;
using Drip.Data;
using System.Collections.Generic;

namespace Drip.Phases.Douse
{
    public class DousePhaseController : MonoBehaviour, IPhaseController
    {
        [Header("Combat Settings")]
        [SerializeField] private int damagePerShot = 5;
        [SerializeField] private GameObject waterProjectilePrefab;

        [Header("Environment")]
        [SerializeField] private Transform[] balloonPositions;
        [SerializeField] private GameObject balloonPrefab;

        private bool phaseActive = false;
        private PlayerData currentPlayer;
        private List<PlayerBalloon> activeBalloons = new List<PlayerBalloon>();
        private int shotsFired = 0;

        public void EnterPhase()
        {
            phaseActive = true;
            currentPlayer = GameManager.Instance.CurrentPlayer;
            SpawnBalloons();
            shotsFired = 0;
            
            Debug.Log("Entered Douse Phase");
        }

        public void ExitPhase()
        {
            phaseActive = false;
            ClearBalloons();
            
            Debug.Log($"Exited Douse Phase - Fired {shotsFired} shots");
        }

        public bool IsPhaseComplete()
        {
            // Phase is manually completed by player
            return false;
        }

        private void SpawnBalloons()
        {
            ClearBalloons();

            List<PlayerData> players = GameManager.Instance.Players;
            
            for (int i = 0; i < players.Count; i++)
            {
                if (i < balloonPositions.Length)
                {
                    GameObject balloonObj = Instantiate(balloonPrefab, 
                        balloonPositions[i].position, 
                        Quaternion.identity);
                    
                    PlayerBalloon balloon = balloonObj.GetComponent<PlayerBalloon>();
                    if (balloon != null)
                    {
                        balloon.Initialize(players[i]);
                        activeBalloons.Add(balloon);
                    }
                }
            }
        }

        private void ClearBalloons()
        {
            foreach (var balloon in activeBalloons)
            {
                if (balloon != null)
                    Destroy(balloon.gameObject);
            }
            activeBalloons.Clear();
        }

        public void ShootAtBalloon(PlayerBalloon targetBalloon)
        {
            if (!phaseActive || currentPlayer == null) return;

            // Check if player has ammo
            if (currentPlayer.waterGunAmmo <= 0)
            {
                Debug.Log("Out of ammo!");
                return;
            }

            // Consume ammo
            currentPlayer.waterGunAmmo--;
            shotsFired++;

            // Fire projectile
            if (waterProjectilePrefab != null)
            {
                // Spawn projectile and handle in separate script
                // For now, directly apply damage
                DamageBalloon(targetBalloon, damagePerShot);
            }
        }

        private void DamageBalloon(PlayerBalloon balloon, int damage)
        {
            if (balloon != null)
            {
                balloon.TakeDamage(damage);
                currentPlayer.DealDamage(damage);
                
                // Update actual player health
                GameManager.Instance.DamagePlayer(balloon.OwnerPlayerID, damage);
            }
        }

        public void CompleteDousePhase()
        {
            if (phaseActive)
            {
                GameManager.Instance.AdvancePhase();
            }
        }
    }
}
