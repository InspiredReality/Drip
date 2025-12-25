using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drip.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        [SerializeField] private GameState currentState = GameState.MainMenu;
        [SerializeField] private GamePhase currentPhase = GamePhase.Drop;
        [SerializeField] private int currentPlayerIndex = 0;

        [Header("Game Configuration")]
        [SerializeField] private int maxPlayers = 2;
        [SerializeField] private float turnTimeLimit = 259200f; // 3 days in seconds
        [SerializeField] private int startingBalloonHealth = 100;

        [Header("References")]
        [SerializeField] private PhaseManager phaseManager;
        [SerializeField] private TurnManager turnManager;

        // Player data
        private List<PlayerData> players = new List<PlayerData>();
        public PlayerData CurrentPlayer => players.Count > 0 ? players[currentPlayerIndex] : null;
        public List<PlayerData> Players => players;

        // Events
        public event Action<GamePhase> OnPhaseChanged;
        public event Action<PlayerData> OnTurnChanged;
        public event Action<PlayerData> OnGameOver;
        public event Action<GameState> OnGameStateChanged;

        // Properties
        public GameState CurrentState => currentState;
        public GamePhase CurrentPhase => currentPhase;
        public int CurrentPlayerIndex => currentPlayerIndex;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeManagers();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeManagers()
        {
            if (phaseManager == null)
                phaseManager = GetComponent<PhaseManager>();
            
            if (turnManager == null)
                turnManager = GetComponent<TurnManager>();
        }

        public void StartNewGame(List<string> playerNames)
        {
            players.Clear();
            
            for (int i = 0; i < playerNames.Count; i++)
            {
                players.Add(new PlayerData
                {
                    playerID = Guid.NewGuid().ToString(),
                    playerName = playerNames[i],
                    waterAmount = 0,
                    balloonHealth = startingBalloonHealth,
                    currentDrainageLevel = 0,
                    hasWaterGun = false
                });
            }

            currentPlayerIndex = 0;
            currentPhase = GamePhase.Drop;
            ChangeGameState(GameState.Playing);
            
            turnManager.StartNewTurn();
            phaseManager.SetPhase(currentPhase);
        }

        public void AdvancePhase()
        {
            switch (currentPhase)
            {
                case GamePhase.Drop:
                    SetPhase(GamePhase.Divide);
                    break;
                case GamePhase.Divide:
                    SetPhase(GamePhase.Douse);
                    break;
                case GamePhase.Douse:
                    EndTurn();
                    break;
            }
        }

        private void SetPhase(GamePhase newPhase)
        {
            currentPhase = newPhase;
            phaseManager.SetPhase(currentPhase);
            OnPhaseChanged?.Invoke(currentPhase);
        }

        public void EndTurn()
        {
            SaveManager.Instance.SaveGameState();

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            currentPhase = GamePhase.Drop;

            if (CheckGameOver())
                return;

            turnManager.StartNewTurn();
            SetPhase(GamePhase.Drop);
            OnTurnChanged?.Invoke(CurrentPlayer);
        }

        private bool CheckGameOver()
        {
            foreach (var player in players)
            {
                if (player.balloonHealth <= 0)
                {
                    ChangeGameState(GameState.GameOver);
                    OnGameOver?.Invoke(player);
                    return true;
                }
            }
            return false;
        }

        public void ChangeGameState(GameState newState)
        {
            currentState = newState;
            OnGameStateChanged?.Invoke(currentState);
        }

        public void DamagePlayer(string playerID, int damage)
        {
            PlayerData player = players.Find(p => p.playerID == playerID);
            if (player != null)
            {
                player.balloonHealth -= damage;
                player.balloonHealth = Mathf.Max(0, player.balloonHealth);
            }
        }

        public void AddWaterToPlayer(string playerID, int amount)
        {
            PlayerData player = players.Find(p => p.playerID == playerID);
            if (player != null)
            {
                player.waterAmount += amount;
            }
        }

        public void QuitGame()
        {
            SaveManager.Instance.SaveGameState();
            ChangeGameState(GameState.MainMenu);
        }
    }

    public enum GameState
    {
        MainMenu,
        Lobby,
        Playing,
        Paused,
        GameOver
    }

    public enum GamePhase
    {
        Drop,
        Divide,
        Douse
    }
}
