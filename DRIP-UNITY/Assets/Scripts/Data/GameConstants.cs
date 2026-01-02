namespace Drip.Data
{
    public static class GameConstants
    {
        // Turn Settings
        public const float TURN_TIME_LIMIT_SECONDS = 259200f; // 3 days
        public const int MAX_PLAYERS = 2;

        // Player Settings
        public const int STARTING_BALLOON_HEALTH = 100;
        public const int STARTING_WATER = 0;

        // Drop Phase
        public const int WATER_PER_DROPLET = 1;
        public const float DROP_PHASE_DURATION = 30f;
        public const int DROPLETS_PER_SECOND = 5;

        // Divide Phase - Upgrade Costs
        public const int DRAINAGE_UPGRADE_COST_LEVEL_1 = 10;
        public const int DRAINAGE_UPGRADE_COST_LEVEL_2 = 25;
        public const int DRAINAGE_UPGRADE_COST_LEVEL_3 = 50;
        public const int WATER_GUN_COST = 20;
        public const int AMMO_PER_WATER = 2;

        // Douse Phase
        public const int DAMAGE_PER_SHOT = 5;
        public const float PROJECTILE_SPEED = 10f;

        // Tags
        public const string TAG_BUCKET = "Bucket";
        public const string TAG_GROUND = "Ground";
        public const string TAG_BALLOON = "Balloon";
        public const string TAG_PLAYER = "Player";

        // Layers (use layer indices)
        public const int LAYER_DEFAULT = 0;
        public const int LAYER_WATER = 4;
        public const int LAYER_UI = 5;

        // Save System
        public const string SAVE_FILE_NAME = "drip_save.json";
        public const string PREFS_MUSIC_VOLUME = "MusicVolume";
        public const string PREFS_SFX_VOLUME = "SFXVolume";
        public const string PREFS_MASTER_VOLUME = "MasterVolume";

        // Networking
        public const string SERVER_URL = "https://your-game-server.com";
        public const int MAX_RECONNECT_ATTEMPTS = 3;
        public const float RECONNECT_DELAY = 2f;
    }
}
