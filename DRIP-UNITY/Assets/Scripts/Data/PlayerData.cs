using System;
using UnityEngine;

namespace Drip.Data
{
    [Serializable]
    public class PlayerData
    {
        public string playerID;
        public string playerName;
        public int waterAmount;
        public int balloonHealth;
        public int currentDrainageLevel; // 0 = basic, 1-3 = upgrades
        public bool hasWaterGun;
        public int waterGunAmmo;

        // Statistics
        public int totalWaterCollected;
        public int totalDamageDealt;
        public int totalDamageTaken;

        public PlayerData()
        {
            playerID = Guid.NewGuid().ToString();
            waterAmount = 0;
            balloonHealth = 100;
            currentDrainageLevel = 0;
            hasWaterGun = false;
            waterGunAmmo = 0;
            totalWaterCollected = 0;
            totalDamageDealt = 0;
            totalDamageTaken = 0;
        }

        public bool CanAffordUpgrade(int cost)
        {
            return waterAmount >= cost;
        }

        public void SpendWater(int amount)
        {
            waterAmount -= amount;
            waterAmount = Mathf.Max(0, waterAmount);
        }

        public void CollectWater(int amount)
        {
            waterAmount += amount;
            totalWaterCollected += amount;
        }

        public void TakeDamage(int damage)
        {
            balloonHealth -= damage;
            totalDamageTaken += damage;
            balloonHealth = Mathf.Max(0, balloonHealth);
        }

        public void DealDamage(int damage)
        {
            totalDamageDealt += damage;
        }
    }
}
