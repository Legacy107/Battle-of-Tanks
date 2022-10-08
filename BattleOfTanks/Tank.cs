using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Tank: PhysicalObject
    {
        private Weapon _weapon;
        private double _health;
        private double _shield;

        public Tank(double x, double y, double health = 100, double shield = 0)
            : base("Tank", x, y, 0, 80, 50, 200)
        {
            _weapon = new Cannon();
            _health = health;
            _shield = shield;
        }

        public void MoveForward(double force)
        {
            ApplyForce(SplashKit.VectorFromAngle(RotationAngle, force));
        }

        public void MoveBackward(double force)
        {
            MoveForward(-force);
        }

        public List<Bullet> Shoot(Point2D target)
        {
            return _weapon.Shoot(
                SplashKit.RectangleCenter(BoundingBox),
                SplashKit.VectorInvert(
                    SplashKit.VectorFromPointToRect(target, BoundingBox)
                )
            );
        }

        public void TakeDamage(double damage)
        {
            _shield -= damage;
            // Carry over damage
            _health -= Math.Min(_shield, 0);

            _shield = Math.Max(_shield, 0);
            _health = Math.Max(_health, 0);
        }

        public Weapon Weapon
        {
            get
            {
                return _weapon;
            }
            set
            {
                _weapon = Weapon;
            }
        }

        public double Health
        {
            get
            {
                return _health;
            }
        }

        public double Shield
        {
            get
            {
                return _shield;
            }
            set
            {
                _shield = value;
            }
        }
    }
}

