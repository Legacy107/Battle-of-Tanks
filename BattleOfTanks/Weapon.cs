using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public abstract class Weapon
    {
        public double Damage { get; set; }

        public Weapon(double damage)
        {
            Damage = damage;
        }

        public List<Bullet> Shoot(Point2D anchor, Vector2D direction)
        {
            double angle = SplashKit.VectorAngle(direction);
            List<Bullet> bullets = CreateBullet();

            foreach (Bullet bullet in bullets)
            {
                // Move bullet to the tip of the weapon
                bullet.RotationAngle = SplashKit.VectorAngle(direction);

                // Apply transformation to location
                bullet.Location = SplashKit.MatrixMultiply(
                    SplashKit.RotationMatrix(bullet.RotationAngle),
                    bullet.Location
                );
                bullet.Location = SplashKit.MatrixMultiply(
                    SplashKit.TranslationMatrix(anchor),
                    bullet.Location
                );

                // Apply transformation to offset
                bullet.Offset = SplashKit.MatrixMultiply(
                    SplashKit.RotationMatrix(bullet.RotationAngle),
                    bullet.Offset
                );
                // Apply offset to location
                bullet.Location = SplashKit.PointOffsetBy(
                    bullet.Location,
                    bullet.Offset
                );

                // Apply transform to velocity 
                bullet.Velo = SplashKit.MatrixMultiply(
                    SplashKit.RotationMatrix(bullet.RotationAngle),
                    bullet.Velo
                );
            }

            return bullets;
        }

        public abstract List<Bullet> CreateBullet();
    }
}

