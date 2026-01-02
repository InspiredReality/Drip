using UnityEngine;
using Drip.Managers;
using Drip.Data;

namespace Drip.Phases.Divide
{
    public class DividePhaseController : MonoBehaviour, IPhaseController
    {
        [Header("Upgrade Costs")]
        [SerializeField] private int[] drainageUpgradeCosts = { 10, 25, 50 };
        [SerializeField] private int waterGunCost = 20;
        [SerializeField] private int ammoPerWater = 2; // How much ammo per water spent

        [Header("UI References")]
        [SerializeField] private GameObject divideUI;

        private bool phaseActive = false;
        private PlayerData currentPlayer;

        public void EnterPhase()
        {
            phaseActive = true;
            currentPlayer = GameManager.Instance.CurrentPlayer;
            
            if (divideUI != null)
                divideUI.SetActive(true);
            
            Debug.Log("Entered Divide Phase");
        }

        public void ExitPhase()
        {
            phaseActive = false;
            
            if (divideUI != null)
                divideUI.SetActive(false);
            
            Debug.Log("Exited Divide Phase");
        }

        public bool IsPhaseComplete()
        {
            // Phase is manually completed by player clicking "Done" button
            return false;
        }

        public void UpgradeDrainage()
        {
            if (currentPlayer == null) return;

            int nextLevel = currentPlayer.currentDrainageLevel + 1;
            
            if (nextLevel > drainageUpgradeCosts.Length)
            {
                Debug.Log("Max drainage level reached");
                return;
            }

            int cost = drainageUpgradeCosts[nextLevel - 1];
            
            if (currentPlayer.CanAffordUpgrade(cost))
            {
                currentPlayer.SpendWater(cost);
                currentPlayer.currentDrainageLevel = nextLevel;
                Debug.Log($"Upgraded drainage to level {nextLevel}");
            }
            else
            {
                Debug.Log($"Not enough water. Need {cost}, have {currentPlayer.waterAmount}");
            }
        }

        public void PurchaseWaterGun()
        {
            if (currentPlayer == null) return;

            if (currentPlayer.hasWaterGun)
            {
                Debug.Log("Already have water gun");
                return;
            }

            if (currentPlayer.CanAffordUpgrade(waterGunCost))
            {
                currentPlayer.SpendWater(waterGunCost);
                currentPlayer.hasWaterGun = true;
                Debug.Log("Purchased water gun");
            }
            else
            {
                Debug.Log($"Not enough water. Need {waterGunCost}, have {currentPlayer.waterAmount}");
            }
        }

        public void ConvertWaterToAmmo(int waterAmount)
        {
            if (currentPlayer == null) return;

            if (!currentPlayer.hasWaterGun)
            {
                Debug.Log("Need to purchase water gun first");
                return;
            }

            if (currentPlayer.CanAffordUpgrade(waterAmount))
            {
                currentPlayer.SpendWater(waterAmount);
                int ammoGained = waterAmount * ammoPerWater;
                currentPlayer.waterGunAmmo += ammoGained;
                Debug.Log($"Converted {waterAmount} water to {ammoGained} ammo");
            }
            else
            {
                Debug.Log($"Not enough water. Need {waterAmount}, have {currentPlayer.waterAmount}");
            }
        }

        public void CompleteDividePhase()
        {
            if (phaseActive)
            {
                GameManager.Instance.AdvancePhase();
            }
        }
    }
}
