using System.Reflection.Metadata;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Bullet: PhysicalObject
    {
        private Vector2D _offset;

        public Bullet(double offsetX, double offsetY, double angle = 0, double speed = 200)
            : base("Bullet", 0, 0, angle, 200, 500, 50)
        {
            Velo = SplashKit.VectorFromAngle(RotationAngle, speed);
            _offset = SplashKit.VectorTo(offsetX, offsetY);
        } 

        public Vector2D Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
            }
        }
    }
}

