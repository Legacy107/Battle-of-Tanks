using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class GameManager: IObserver, IObserverable
    {
        private Window _window;
        private const string _clock = "game clock";
        private double _prevTime; // for calculating delta time
        private const double MOVE_FORCE = 10000;
        private Tank _playerTank;
        private List<Tank> _enemyTanks;
        private List<Bullet> _bullets;
        private Map _map;
        private int _level = 0;
        private GameState _state;
        private Enemy _enemy;
        private double _playerShootCd;
        private double _enemyShootCd;
        private List<IObserver> _observers;
        private int _lives;

        // Disable since Init() is part of the constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public GameManager(Window window)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _observers = new List<IObserver>();

            _state = GameState.RUNNING;
            _window = window;

            SplashKit.CreateTimer(_clock);
            SplashKit.StartTimer(_clock);
            _prevTime = 0;

            _level = 0;
            _bullets = new List<Bullet>();
            _enemyTanks = new List<Tank>();
            Init();
        }

        public void Init()
        {
            CommandExecutor.Instance.Clear();
            _observers.Clear();

            _map = new Map(GameConfig.LEVELS[_level]);
            _map.Base.RegisterObserver(this);

            _playerTank = new Tank(_map.PlayerSpawn.X, _map.PlayerSpawn.Y);
            _lives = 3;

            _bullets.Clear();
            _enemyTanks.Clear();

            switch (_map.Difficulty.ToLower())
            {
                case "easy":
                    _enemy = new EasyEnemy();
                    break;
                case "hard":
                    _enemy = new HardEnemy();
                    break;
                default:
                    throw new InvalidDataException(string.Format(
                        "Unknown difficulty",
                        _map.Difficulty
                    ));
            }
            RegisterObserver(_enemy);

            _playerShootCd = 0;
            _enemyShootCd = 0;
        }

        // return true on exit command
        public bool HandleInputGui()
        {
            if (SplashKit.KeyTyped(KeyCode.EscapeKey) || SplashKit.QuitRequested())
                return true;

            if (SplashKit.KeyTyped(KeyCode.BackquoteKey))
                GameConfig.DEBUG = !GameConfig.DEBUG;

            return false;
        }

        public void HandleInputGame(double delta)
        {
            if (SplashKit.KeyTyped(KeyCode.Num1Key))
                _playerTank.Weapon = new Cannon();
            if (SplashKit.KeyTyped(KeyCode.Num2Key))
                _playerTank.Weapon = new DualCannon();
            if (SplashKit.KeyDown(KeyCode.WKey))
                _playerTank.MoveForward(MOVE_FORCE);
            if (SplashKit.KeyDown(KeyCode.SKey))
                _playerTank.MoveBackward(MOVE_FORCE);

            if (
                _playerShootCd <= 0 &&
                SplashKit.MouseClicked(MouseButton.LeftButton)
            )
            {
                _bullets.AddRange(_playerTank.Shoot(SplashKit.MousePosition()));
                _playerShootCd = GameConfig.SHOOT_COOLDOWN;
            }

            _playerTank.RotateToPoint(SplashKit.MousePosition());

            _enemy.SpawnEnemy(_enemyTanks, _map.EnemySpawn);
            _enemy.MoveTank(_enemyTanks, _playerTank, _map.Base);
            if (_enemyShootCd <= 0)
            {
                _bullets.AddRange(_enemy.Shoot(_enemyTanks, _playerTank, _map.Base));
                _enemyShootCd = GameConfig.SHOOT_COOLDOWN;
            }

            _playerShootCd -= delta;
            _enemyShootCd -= delta;
        }

        public void Update(double delta)
        {
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

            if (_playerTank.NeedRemoval)
            {
                _lives -= 1;
                if (_lives == 0)
                    HandleLose();

                // Respawn player
                _playerTank = new Tank(_map.PlayerSpawn.X, _map.PlayerSpawn.Y);
            }

            NotifyObserver();
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

            DrawHearts();

            if (GameConfig.DEBUG)
            {
                SplashKit.DrawText("Speed: " + SplashKit.VectorMagnitude(_playerTank.Velo), Color.Black, 30, GameConfig.WINDOW_HEIGHT - 80);
                SplashKit.DrawText("Accel: " + SplashKit.VectorMagnitude(_playerTank.Accel), Color.Black, 30, GameConfig.WINDOW_HEIGHT - 70);
                SplashKit.DrawText("Friction: " + _playerTank.Friction, Color.Black, 30, GameConfig.WINDOW_HEIGHT - 60);
                SplashKit.DrawText("Angle: " + _playerTank.RotationAngle, Color.Black, 30, GameConfig.WINDOW_HEIGHT - 50);
                SplashKit.DrawText("Health: " + _playerTank.Health, Color.Black, 30, GameConfig.WINDOW_HEIGHT - 40);
            }

            SplashKit.RefreshScreen(60);
        }

        public void DrawHearts()
        {
            int x = GameConfig.WINDOW_WIDTH - 2 * GameConfig.TILE_SIZE;
            int y = GameConfig.WINDOW_HEIGHT - 2 * GameConfig.TILE_SIZE;
            for (int i = 0; i < _lives; i++)
            {
                SplashKit.DrawBitmapOnWindow(_window, SplashKit.BitmapNamed("Heart"), x, y);
                x -= GameConfig.TILE_SIZE;
            }
        }

        public void DrawEnding(string message)
        {
            SplashKit.DrawText(
                message,
                Color.Black,
                "defaultFont",
                40,
                (GameConfig.WINDOW_WIDTH - SplashKit.TextWidth(
                    message, "defaultFont", 40
                )) / 2.0,
                250
            );
            SplashKit.RefreshScreen(60);
        }

        public void MainLoop()
        {
            while (true)
            {
                SplashKit.ProcessEvents();
                if (HandleInputGui())
                    break;

                switch (_state)
                {
                    case GameState.RUNNING:
                        int curTime = (int)SplashKit.TimerTicks(_clock);
                        double delta = (curTime - _prevTime) / 1000.0; // milisecond to second
                        _prevTime = curTime;

                        HandleInputGame(delta);
                        Update(delta);
                        Draw();

                        if (CheckWin())
                            HandleWin();

                        break;

                    case GameState.LOSE:
                        DrawEnding("You Lose");
                        break;

                    case GameState.WIN:
                        DrawEnding("You Win");
                        break;
                }
            }
        }

        public void HandleLose()
        {
            _state = GameState.LOSE;
            Console.WriteLine("You lose");
        }

        public void HandleWin()
        {
            _level += 1;
            // Switch to next level and reset the game
            if (_level < GameConfig.LEVELS.Count())
            {
                Init();
                return;
            }

            _state = GameState.WIN;
            Console.WriteLine("You win");
        }

        public bool CheckWin()
        {
            return _enemyTanks.Count == 0 && _enemy.NoEnemy == 0;
        }

        // Get notified when the base is destroyed
        public void ObserverUpdate(params object[] list)
        {
            HandleLose();
        }

        public void NotifyObserver()
        {
            List<Tank> removedTanks = new List<Tank>();
            foreach (Tank tank in _enemyTanks)
                if (tank.NeedRemoval)
                    removedTanks.Add(tank);

            // Notify enemy of removed tanks
            foreach (IObserver observer in _observers)
                observer.ObserverUpdate(removedTanks);
        }

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }
    }
}

