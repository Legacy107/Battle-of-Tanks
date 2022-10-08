namespace BattleOfTanks
{
    public class GameConfigSingleton
    {
        private bool _debug;
        private static GameConfigSingleton? _instance;

        private GameConfigSingleton()
        {
            _debug = false;
        }

        public static GameConfigSingleton Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameConfigSingleton();

                return _instance;
            }
        }

        public bool Debug
        {
            get
            {
                return _debug;
            }
            set
            {
                _debug = value;
            }
        }
    }
}

