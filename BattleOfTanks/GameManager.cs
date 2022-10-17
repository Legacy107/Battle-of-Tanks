using System.Collections.Generic;
using System.Linq;
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
        private Map _map;

        public GameManager(Window window)
        {
            _window = window;

            SplashKit.CreateTimer(_clock);
            SplashKit.StartTimer(_clock);
            _prevTime = 0;

            _map = new Map();

            _playerTank = new Tank(_map.PlayerSpawn.X, _map.PlayerSpawn.Y);
            _enemyTanks = new List<Tank>
            {
                new Tank(_map.EnemySpawns)
            };
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

            CommandExecutor.Instance.Execute(delta);

            // Update physic
            _playerTank.Update(delta);
            _map.CheckCollision(_playerTank);

            foreach (Tank tank in _enemyTanks)
            {
                tank.Update(delta);
                _map.CheckCollision(tank);
            }

            foreach (Bullet bullet in _bullets)
            {
                bullet.Update(delta);

                _map.CheckCollision(bullet);

                bullet.IsCollided(_playerTank);
                foreach (Tank tank in _enemyTanks)
                    bullet.IsCollided(tank);
            }

            // Remove objects in reverse order to avoid index issue
            // https://stackoverflow.com/questions/1582285/how-to-remove-elements-from-a-generic-list-while-iterating-over-it
            foreach (Tank tank in _enemyTanks.Reverse<Tank>())
                if (tank.NeedRemoval)
                    _enemyTanks.Remove(tank);

            foreach (Bullet bullet in _bullets.Reverse<Bullet>())
                if (bullet.NeedRemoval)
                    _bullets.Remove(bullet);

            _map.CheckRemoval();
        }

        public void Draw()
        {
            SplashKit.ClearScreen(Color.White);

            _map.Draw(_window);

            _playerTank.Draw(_window);

            foreach (Tank tank in _enemyTanks)
                tank.Draw(_window);

            foreach (Bullet bullet in _bullets)
                bullet.Draw(_window);

            if (GameConfig.DEBUG)
            {
                SplashKit.DrawText("Speed: " + SplashKit.VectorMagnitude(_playerTank.Velo), Color.Black, 30, GameConfig.WINDOW_HEIGHT - 70);
                SplashKit.DrawText("Accel: " + SplashKit.VectorMagnitude(_playerTank.Accel), Color.Black, 30, GameConfig.WINDOW_HEIGHT - 60);
                SplashKit.DrawText("Friction: " + _playerTank.Friction, Color.Black, 30, GameConfig.WINDOW_HEIGHT - 50);
                SplashKit.DrawText("Angle: " + _playerTank.RotationAngle, Color.Black, 30, GameConfig.WINDOW_HEIGHT - 40);
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

