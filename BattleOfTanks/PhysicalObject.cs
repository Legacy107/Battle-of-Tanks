using SplashKitSDK;

namespace BattleOfTanks
{
    public class PhysicalObject: GameObject
    {
        private Vector2D _velo;
        private Vector2D _accel;
        private double _maxSpeed;
        private double _maxAccel;
        public double Mass { get; set; }
        public double Friction { get; set; }

        public PhysicalObject(string sprite, double x, double y, double angle = 0, double maxSpeed = 10, double maxAccel = 10, double mass = 150, double friction = 1)
            : base(sprite, x, y, angle)
        {
            _maxSpeed = maxSpeed;
            _maxAccel = maxAccel;
            _velo = SplashKit.VectorTo(0, 0);
            _accel = SplashKit.VectorTo(0, 0);
            Mass = mass;
            Friction = friction;
        }

        public void Update(double delta)
        {
            Vector2D frictionForce = SplashKit.VectorMultiply(SplashKit.VectorLimit(_velo, 1), -Friction * Weight);
            ApplyForce(frictionForce);

            _velo = SplashKit.VectorAdd(_velo, SplashKit.VectorMultiply(_accel, delta));
            _velo = SplashKit.VectorLimit(_velo, _maxSpeed);
            if (SplashKit.VectorMagnitude(_velo) < GameConfig.EPS)
                _velo = SplashKit.VectorTo(0, 0);

            Vector2D locationVector = SplashKit.VectorAdd(SplashKit.VectorTo(Location), SplashKit.VectorMultiply(_velo, delta));
            Location = SplashKit.PointAt(locationVector.X, locationVector.Y);

            _accel = SplashKit.VectorTo(0, 0);
        }

        public void ApplyForce(Vector2D force)
        {
            _accel = SplashKit.VectorAdd(_accel, SplashKit.VectorMultiply(force, 1 / Mass));
            _accel = SplashKit.VectorLimit(_accel, _maxAccel);
        }

        public void Turn(double angle)
        {
            RotationAngle += angle;
        }

        public void RotateToPoint(Point2D point)
        {
            RotationAngle = SplashKit.VectorAngle(SplashKit.VectorInvert(
                SplashKit.VectorFromPointToRect(point, BoundingBox))
            );
        }

        public virtual bool IsCollided(GameObject obj)
        {
            return SplashKit.RectanglesIntersect(BoundingBox, obj.BoundingBox);
        }

        public Vector2D Velo
        {
            get
            {
                return _velo;
            }
            set
            {
                _velo = SplashKit.VectorLimit(value, _maxSpeed);
            }
        }

        public Vector2D Accel
        {
            get
            {
                return _accel;
            }
        }

        public double Weight
        {
            get
            {
                return Mass * 10;
            }
        }
    }
}

