using System;
using System.IO;
using UnityEngine;
using Drip.Data;

namespace Drip.Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private const string SAVE_FILE_NAME = "drip_save.json";
        private string SaveFilePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SaveGameState()
        {
            try
            {
                GameData data = new GameData
                {
                    players = GameManager.Instance.Players,
                    currentPlayerIndex = GameManager.Instance.CurrentPlayerIndex,
                    currentPhase = GameManager.Instance.CurrentPhase.ToString(),
                    turnStartTime = DateTime.Now.ToString("o"),
                    remainingTurnTime = GetComponent<TurnManager>()?.RemainingTime ?? 0f
                };

                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SaveFilePath, json);
                
                Debug.Log($"Game saved successfully to {SaveFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save game: {e.Message}");
            }
        }

        public bool LoadGameState()
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    Debug.Log("No save file found");
                    return false;
                }

                string json = File.ReadAllText(SaveFilePath);
                GameData data = JsonUtility.FromJson<GameData>(json);

                // Apply loaded data to GameManager
                // This would need to be implemented based on your game flow
                Debug.Log("Game loaded successfully");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load game: {e.Message}");
                return false;
            }
        }

        public void DeleteSaveFile()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    File.Delete(SaveFilePath);
                    Debug.Log("Save file deleted");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete save file: {e.Message}");
            }
        }

        public bool HasSaveFile()
        {
            return File.Exists(SaveFilePath);
        }
    }
}
