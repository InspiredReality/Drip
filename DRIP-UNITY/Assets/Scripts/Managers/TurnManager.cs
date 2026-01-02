using System;
using UnityEngine;

namespace Drip.Managers
{
    public class TurnManager : MonoBehaviour
    {
        [Header("Turn Settings")]
        [SerializeField] private float turnTimeLimit = 259200f; // 3 days
        
        private DateTime turnStartTime;
        private float remainingTime;
        private bool isTurnActive = false;

        public event Action OnTurnTimeout;
        public event Action<float> OnTurnTimeUpdated;

        public float RemainingTime => remainingTime;
        public DateTime TurnStartTime => turnStartTime;
        public bool IsTurnActive => isTurnActive;

        private void Update()
        {
            if (isTurnActive)
            {
                UpdateTurnTimer();
            }
        }

        public void StartNewTurn()
        {
            turnStartTime = DateTime.Now;
            remainingTime = turnTimeLimit;
            isTurnActive = true;
        }

        private void UpdateTurnTimer()
        {
            TimeSpan elapsed = DateTime.Now - turnStartTime;
            remainingTime = turnTimeLimit - (float)elapsed.TotalSeconds;

            OnTurnTimeUpdated?.Invoke(remainingTime);

            if (remainingTime <= 0)
            {
                HandleTurnTimeout();
            }
        }

        private void HandleTurnTimeout()
        {
            isTurnActive = false;
            OnTurnTimeout?.Invoke();
            
            // Auto-end turn on timeout
            GameManager.Instance.EndTurn();
        }

        public void EndTurn()
        {
            isTurnActive = false;
        }

        public string GetRemainingTimeFormatted()
        {
            TimeSpan time = TimeSpan.FromSeconds(remainingTime);
            return $"{time.Days}d {time.Hours}h {time.Minutes}m";
        }
    }
}
