namespace BattleOfTanks
{
    public static class GameConfig
    {
        public static int WINDOW_WIDTH = 812;
        public static int WINDOW_HEIGHT = 840;
        public static bool DEBUG { get; set; } = false;
        public static double EPS { get; } = 0.00001;
        public static int TILE_SIZE = 28;
        public static int MAP_WIDTH = 29;
        public static string[] LEVELS = new string[]
        {
            "./levelData/level1.txt",
            "./levelData/level2.txt"
        };
        public static double SHOOT_COOLDOWN = 0.3;
    }
}

