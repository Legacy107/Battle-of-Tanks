using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Tank: PhysicalObject, ICanTakeDamage
    {
        private const int HEALTH_BAR_WIDTH = 42;
        private const int HEALTH_BAR_HEIGHT = 10;
        public Weapon Weapon { get; set; }
        private double _health;
        private double _maxHealth;
        public double Shield { get; set; }

        public Tank
        (
            Point2D point,
            string sprite = "Tank",
            double health = 100,
            double shield = 0
        )
            : this(point.X, point.Y, sprite, health, shield)
        {
        }

        public Tank
        (
            double x,
            double y,
            string sprite = "Tank",
            double health = 100,
            double shield = 0
        )
            : base(sprite, x, y, 0, 80, 50, 200)
        {
            Weapon = new Cannon();
            _health = health;
            _maxHealth = health;
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
            _health += Math.Min(Shield, 0);

            Shield = Math.Max(Shield, 0);
            _health = Math.Max(_health, 0);

            if (_health == 0)
                NeedRemoval = true;
        }

        public override void Draw(Window window)
        {
            base.Draw(window);
            SplashKit.DrawRectangleOnWindow(
                window,
                SplashKit.RGBColor(0.25, 0.24, 0.19),
                SplashKit.RectangleFrom(
                    BoundingBox.X, BoundingBox.Y - HEALTH_BAR_HEIGHT * 2,
                    HEALTH_BAR_WIDTH, HEALTH_BAR_HEIGHT
                )
            );

            double innerLength = _health / _maxHealth * (HEALTH_BAR_WIDTH - 4);
            SplashKit.FillRectangleOnWindow(
                window,
                SplashKit.RGBAColor(0.33, 0.56, 0.78, 0.6),
                SplashKit.RectangleFrom(
                    BoundingBox.X + 2, BoundingBox.Y - HEALTH_BAR_HEIGHT * 2 + 2,
                    innerLength, HEALTH_BAR_HEIGHT - 4
                )
            );
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

