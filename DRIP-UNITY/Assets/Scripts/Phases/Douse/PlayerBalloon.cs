using UnityEngine;
using UnityEngine.UI;
using Drip.Data;

namespace Drip.Phases.Douse
{
    public class PlayerBalloon : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Text playerNameText;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Text healthText;

        private PlayerData ownerData;
        private int currentHealth;

        public string OwnerPlayerID => ownerData?.playerID;

        public void Initialize(PlayerData playerData)
        {
            ownerData = playerData;
            currentHealth = playerData.balloonHealth;
            UpdateUI();
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
            UpdateUI();

            if (currentHealth <= 0)
            {
                OnBalloonDestroyed();
            }
        }

        private void UpdateUI()
        {
            if (ownerData == null) return;

            if (playerNameText != null)
                playerNameText.text = ownerData.playerName;

            if (healthBar != null)
            {
                healthBar.maxValue = 100; // Assuming max health is 100
                healthBar.value = currentHealth;
            }

            if (healthText != null)
                healthText.text = $"{currentHealth} HP";
        }

        private void OnBalloonDestroyed()
        {
            Debug.Log($"{ownerData.playerName}'s balloon was destroyed!");
            // Play destruction effect
            // Optionally disable rather than destroy for visual feedback
            gameObject.SetActive(false);
        }

        private void OnMouseDown()
        {
            // Allow clicking balloon to shoot at it
            DousePhaseController controller = FindObjectOfType<DousePhaseController>();
            if (controller != null)
            {
                controller.ShootAtBalloon(this);
            }
        }
    }
}
