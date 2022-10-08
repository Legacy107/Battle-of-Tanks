using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class GameManager
    {
        private Window _window;
        private const string _clock = "game clock";
        private double _prevTime; // for calculating delta time
        private Tank _playerTank;
        private List<Tank> _enemyTanks;
        private List<Bullet> _bullets;

        public GameManager(Window window)
        {
            _window = window;

            SplashKit.CreateTimer(_clock);
            SplashKit.StartTimer(_clock);
            _prevTime = 0;

            _playerTank = new Tank(100, 100);
            _enemyTanks = new List<Tank>();
            _bullets = new List<Bullet>();
        }

        // return true on exit command
        public bool HandleInput()
        {
            SplashKit.ProcessEvents();

            if (SplashKit.KeyTyped(KeyCode.EscapeKey) || SplashKit.QuitRequested())
                return true;

            if (SplashKit.KeyTyped(KeyCode.BackquoteKey))
                GameConfig.DEBUG = !GameConfig.DEBUG;

            const double moveForce = 10000;
            if (SplashKit.KeyDown(KeyCode.DownKey))
                _playerTank.ApplyForce(SplashKit.VectorTo(0, moveForce));
            if (SplashKit.KeyDown(KeyCode.UpKey))
                _playerTank.ApplyForce(SplashKit.VectorTo(0, -moveForce));
            if (SplashKit.KeyDown(KeyCode.LeftKey))
                _playerTank.ApplyForce(SplashKit.VectorTo(-moveForce, 0));
            if (SplashKit.KeyDown(KeyCode.RightKey))
                _playerTank.ApplyForce(SplashKit.VectorTo(moveForce, 0));

            if (SplashKit.KeyDown(KeyCode.WKey))
                _playerTank.MoveForward(moveForce);
            if (SplashKit.KeyDown(KeyCode.SKey))
                _playerTank.MoveBackward(moveForce);

            if (SplashKit.MouseClicked(MouseButton.LeftButton))
                _bullets.AddRange(_playerTank.Shoot(SplashKit.MousePosition()));

            _playerTank.RotateToPoint(SplashKit.MousePosition());

            return false;
        }

        public void Update()
        {
            int curTime = (int)SplashKit.TimerTicks(_clock);
            double delta = (curTime - _prevTime) / 1000.0; // milisecond to second
            _prevTime = curTime;

            _playerTank.Update(delta);

            foreach (Bullet bullet in _bullets)
                bullet.Update(delta);
        }

        public void Draw()
        {
            SplashKit.ClearScreen(Color.White);

            _playerTank.Draw(_window);
            foreach (Bullet bullet in _bullets)
                bullet.Draw(_window);

            if (GameConfig.DEBUG)
            {
                SplashKit.DrawText("Speed: " + SplashKit.VectorMagnitude(_playerTank.Velo), Color.Black, 10, 10);
                SplashKit.DrawText("Angle: " + _playerTank.RotationAngle, Color.Black, 10, 20);
            }

            SplashKit.RefreshScreen(60);
        }

        public void ChangeLevel()
        {
        }

        public void MainLoop()
        {
            while (true)
            {
                if (HandleInput())
                {
                    CleanUp();
                    break;
                }
                Update();
                Draw();
            }
        }

        public void CleanUp()
        {
        }
    }
}

