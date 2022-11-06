using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class HardEnemy: Enemy
    {
        private const double MOVE_FORCE = 200000;
        private Dictionary<Tank, double> _tankDirections;
        private const int CHANGE_DIR_CHANCE = 25;
        private const int RANDOM_TURN_CHANCE = 5;
        private const int MOVE_CHANCE = 5;
        private const int SHOOT_CHANCE = 10;
        private const int TARGET_PLAYER_CHANCE = 5;
        private const int SPAWN_CHANCE = 50;

        public HardEnemy(int NoEnemy = 5)
            : base(NoEnemy)
        {
            _tankDirections = new Dictionary<Tank, double>();
        }

        public override void MoveTank
        (
            List<Tank> tanks,
            Tank playerTank,
            Base playerBase
        )
        {
            Random random = new Random();

            foreach (Tank tank in tanks)
            {
                if (!_tankDirections.ContainsKey(tank))
                    _tankDirections.Add(
                        tank,
                        random.Next(360)
                    );

                bool stuck = SplashKit.VectorMagnitude(tank.Velo) == 0;
                bool changeDir = stuck || random.Next(CHANGE_DIR_CHANCE) == 0;
                if (changeDir)
                {
                    bool randomTurn = (
                        stuck || random.Next(RANDOM_TURN_CHANCE) < 3
                    );

                    if (randomTurn)
                        _tankDirections[tank] = random.Next(360);
                    else
                        _tankDirections[tank] = SplashKit.VectorAngle(
                            SplashKit.VectorPointToPoint(
                                tank.Location,
                                playerBase.Location
                            )
                        );
                }

                tank.RotateToPoint(SplashKit.PointOffsetBy(
                    tank.Location,
                    SplashKit.VectorFromAngle(
                        _tankDirections[tank],
                        MOVE_FORCE
                    )
                ));

                bool shouldMove = (
                    SplashKit.VectorMagnitude(tank.Velo) == 0 ||
                    random.Next(MOVE_CHANCE) != 0
                );
                if (shouldMove)
                    tank.ApplyForce(SplashKit.VectorFromAngle(
                        _tankDirections[tank],
                        MOVE_FORCE
                    ));
            }
        }

        public override List<Bullet> Shoot
        (
            List<Tank> tanks,
            Tank playerTank,
            Base playerBase
        )
        {
            List<Bullet> bullets = new List<Bullet>();
            Random random = new Random();

            foreach (Tank tank in tanks)
            {
                bool shouldShoot = random.Next(SHOOT_CHANCE) == 0;
                if (!shouldShoot)
                    continue;

                bool targetPlayer = random.Next(TARGET_PLAYER_CHANCE) == 0;
                Point2D target;
                if (targetPlayer)
                    target = playerTank.Location;
                else
                    target = playerBase.Location;

                tank.RotateToPoint(target);
                bullets.AddRange(tank.Shoot(target));
            }

            return bullets;
        }

        public override void SpawnEnemy(List<Tank> tanks, Point2D spawnPoint)
        {
            Random random = new Random();

            bool shouldSpawnEnemy = NoEnemy > 0 && (
                tanks.Count == 0 ||
                random.Next(SPAWN_CHANCE * (int)(Math.Pow(3, tanks.Count))) == 0
            );

            if (!shouldSpawnEnemy)
                return;

            tanks.Add(new Tank(spawnPoint, "TankRed"));
            NoEnemy -= 1;
        }

        // Receive a list of removed tank
        public override void ObserverUpdate(params object[] list)
        {
            foreach (object obj in list)
                if (obj is Tank tank)
                    _tankDirections.Remove(tank);
        }
    }
}

