using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Tank: PhysicalObject
    {
        public Weapon Weapon { get; set; }
        private double _health;
        public double Shield { get; set; }

        public Tank(double x, double y, double health = 100, double shield = 0)
            : base("Tank", x, y, 0, 80, 50, 200)
        {
            Weapon = new Cannon();
            _health = health;
            Shield = shield;
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
            return Weapon.Shoot(
                SplashKit.RectangleCenter(BoundingBox),
                SplashKit.VectorInvert(
                    SplashKit.VectorFromPointToRect(target, BoundingBox)
                )
            );
        }

        public void TakeDamage(double damage)
        {
            Shield -= damage;
            // Carry over damage
            _health -= Math.Min(Shield, 0);

            Shield = Math.Max(Shield, 0);
            _health = Math.Max(_health, 0);
        }

        public double Health
        {
            get
            {
                return _health;
            }
        }
    }
}

