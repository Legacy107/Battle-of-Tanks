using SplashKitSDK;

namespace BattleOfTanks
{
    public class Program
    {
        public static void LoadResources()
        {
            SplashKit.LoadBitmap("Tank", "assets/tank_green42.png");
            SplashKit.LoadBitmap("TankRed", "assets/tank_red.png");
            SplashKit.LoadBitmap("Bullet", "assets/bulletDark2_outline.png");
            SplashKit.LoadBitmap("Wall", "assets/crateWood.png");
            SplashKit.LoadBitmap("Sand", "assets/tileSand1.png");
            SplashKit.LoadBitmap("Steel", "assets/crateMetal.png");
            SplashKit.LoadBitmap("Base", "assets/base.png");
            SplashKit.LoadFont("defaultFont", "assets/FredokaOne-Regular.ttf");
        }

        public static void Main()
        {
            LoadResources();
            Window window = new Window(
                "Battle of Tanks",
                GameConfig.WINDOW_WIDTH,
                GameConfig.WINDOW_HEIGHT
            );
            GameManager gameManager = new GameManager(window);

            gameManager.MainLoop();
        }
    }
}
