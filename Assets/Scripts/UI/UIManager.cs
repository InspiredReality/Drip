using UnityEngine;
using UnityEngine.UI;
using Drip.Managers;
using Drip.Data;

namespace Drip.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;

        [Header("Gameplay UI")]
        [SerializeField] private Text currentPlayerText;
        [SerializeField] private Text currentPhaseText;
        [SerializeField] private Text waterAmountText;
        [SerializeField] private Text turnTimerText;
        [SerializeField] private Text balloonHealthText;

        [Header("Buttons")]
        [SerializeField] private Button nextPhaseButton;
        [SerializeField] private Button endTurnButton;

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

        private void Start()
        {
            SubscribeToEvents();
            ShowMainMenu();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
                GameManager.Instance.OnPhaseChanged += HandlePhaseChanged;
                GameManager.Instance.OnTurnChanged += HandleTurnChanged;
                GameManager.Instance.OnGameOver += HandleGameOver;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
                GameManager.Instance.OnPhaseChanged -= HandlePhaseChanged;
                GameManager.Instance.OnTurnChanged -= HandleTurnChanged;
                GameManager.Instance.OnGameOver -= HandleGameOver;
            }
        }

        private void Update()
        {
            UpdateGameplayUI();
        }

        private void UpdateGameplayUI()
        {
            if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameState.Playing)
                return;

            PlayerData currentPlayer = GameManager.Instance.CurrentPlayer;
            if (currentPlayer == null) return;

            // Update player info
            if (currentPlayerText != null)
                currentPlayerText.text = $"Player: {currentPlayer.playerName}";

            if (waterAmountText != null)
                waterAmountText.text = $"Water: {currentPlayer.waterAmount}";

            if (balloonHealthText != null)
                balloonHealthText.text = $"Health: {currentPlayer.balloonHealth}";

            // Update phase
            if (currentPhaseText != null)
                currentPhaseText.text = $"Phase: {GameManager.Instance.CurrentPhase}";

            // Update turn timer
            TurnManager turnManager = FindObjectOfType<TurnManager>();
            if (turnManager != null && turnTimerText != null)
            {
                turnTimerText.text = $"Time: {turnManager.GetRemainingTimeFormatted()}";
            }
        }

        private void HandleGameStateChanged(GameState newState)
        {
            HideAllPanels();

            switch (newState)
            {
                case GameState.MainMenu:
                    ShowMainMenu();
                    break;
                case GameState.Playing:
                    ShowGameplay();
                    break;
                case GameState.Paused:
                    ShowPause();
                    break;
                case GameState.GameOver:
                    ShowGameOver();
                    break;
            }
        }

        private void HandlePhaseChanged(GamePhase newPhase)
        {
            Debug.Log($"UI: Phase changed to {newPhase}");
        }

        private void HandleTurnChanged(PlayerData newPlayer)
        {
            Debug.Log($"UI: Turn changed to {newPlayer.playerName}");
        }

        private void HandleGameOver(PlayerData loser)
        {
            // Show game over screen
            ShowGameOver();
        }

        private void HideAllPanels()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (gameplayPanel != null) gameplayPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
        }

        public void ShowMainMenu()
        {
            HideAllPanels();
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
        }

        public void ShowGameplay()
        {
            HideAllPanels();
            if (gameplayPanel != null)
                gameplayPanel.SetActive(true);
        }

        public void ShowPause()
        {
            if (pausePanel != null)
                pausePanel.SetActive(true);
        }

        public void ShowGameOver()
        {
            HideAllPanels();
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }

        // Button handlers
        public void OnStartGame()
        {
            // This would typically open a lobby or setup screen
            // For now, start a quick 2-player game
            GameManager.Instance.StartNewGame(new System.Collections.Generic.List<string> 
            { 
                "Player 1", 
                "Player 2" 
            });
        }

        public void OnPauseGame()
        {
            GameManager.Instance.ChangeGameState(GameState.Paused);
        }

        public void OnResumeGame()
        {
            GameManager.Instance.ChangeGameState(GameState.Playing);
        }

        public void OnQuitGame()
        {
            GameManager.Instance.QuitGame();
        }
    }
}
