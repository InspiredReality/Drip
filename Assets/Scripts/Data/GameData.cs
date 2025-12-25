using System;
using System.Collections.Generic;

namespace Drip.Data
{
    [Serializable]
    public class GameData
    {
        public List<PlayerData> players;
        public int currentPlayerIndex;
        public string currentPhase; // Stored as string for serialization
        public string turnStartTime; // DateTime as string
        public float remainingTurnTime;

        public GameData()
        {
            players = new List<PlayerData>();
            currentPlayerIndex = 0;
            currentPhase = "Drop";
            turnStartTime = DateTime.Now.ToString("o");
            remainingTurnTime = 259200f;
        }
    }
}
