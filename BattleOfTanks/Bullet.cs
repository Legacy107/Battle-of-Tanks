using System.Reflection.Metadata;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Bullet: PhysicalObject
    {
        public Vector2D Offset { get; set; }

        public Bullet(double offsetX, double offsetY, double angle = 0, double speed = 200)
            : base("Bullet", 0, 0, angle, 200, 500, 50)
        {
            Velo = SplashKit.VectorFromAngle(RotationAngle, speed);
            Offset = SplashKit.VectorTo(offsetX, offsetY);
        }

        public override bool IsCollided(GameObject obj)
        {
            bool collided = base.IsCollided(obj);

            // TODO: damagable interface
            if (collided && !(obj is Area))
                NeedRemoval = true;

            return collided;
        }
    }
}

