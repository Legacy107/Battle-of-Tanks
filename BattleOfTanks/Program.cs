using System;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Program
    {
        public static void LoadResources()
        {
            SplashKit.LoadBitmap("Tank", "assets/tank_green.png");
        }

        public static void Main()
        {
            LoadResources();
            Window window = new Window("Custom Program", 800, 600);
            Tank tank = new Tank(100, 100);
            Timer timer = new Timer("clock");
            int prevTime = 0;
            timer.Start();
            while(true)
            {
                int curTime = (int)timer.Ticks;
                double delta = (curTime - prevTime) / 1000.0;
                prevTime = curTime;

                SplashKit.ProcessEvents();

                if (SplashKit.KeyTyped(KeyCode.EscapeKey) || SplashKit.QuitRequested())
                {
                    break;
                }

                const double moveForce = 7000;
                if (SplashKit.KeyDown(KeyCode.DownKey))
                {
                    tank.ApplyForce(SplashKit.VectorTo(0, moveForce));
                }
                if (SplashKit.KeyDown(KeyCode.UpKey))
                {
                    tank.ApplyForce(SplashKit.VectorTo(0, -moveForce));
                }
                if (SplashKit.KeyDown(KeyCode.LeftKey))
                {
                    tank.ApplyForce(SplashKit.VectorTo(-moveForce, 0));
                }
                if (SplashKit.KeyDown(KeyCode.RightKey))
                {
                    tank.ApplyForce(SplashKit.VectorTo(moveForce, 0));
                }
                if (SplashKit.KeyDown(KeyCode.WKey))
                {
                    tank.MoveForward(moveForce);
                }
                if (SplashKit.KeyDown(KeyCode.SKey))
                {
                    tank.MoveBackward(moveForce);
                }
                tank.RotateToPoint(SplashKit.MousePosition());

                tank.Update(delta);

                SplashKit.ClearScreen(Color.White);
                tank.Draw(window);
                SplashKit.DrawText("Speed: " + SplashKit.VectorMagnitude(tank.Velo), Color.Black, 10, 10);

                SplashKit.RefreshScreen(60);
            }
        }
    }
}
