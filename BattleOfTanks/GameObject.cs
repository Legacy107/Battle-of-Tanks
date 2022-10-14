using SplashKitSDK;

namespace BattleOfTanks
{
    public class GameObject
    {
        public string Sprite { get; set; }
        Rectangle _boundingBox;
        public double RotationAngle { get; set; }
        public bool NeedRemoval { get; set; }

        public GameObject(string sprite, double x, double y, double angle)
        {
            NeedRemoval = false;
            Sprite = sprite;
            _boundingBox = SplashKit.BitmapNamed(Sprite).BoundingRectangle(x, y);
            // Translate bounding box so that its center is at the coordinate
            SplashKit.RectangleOffsetBy(
                _boundingBox,
                SplashKit.VectorPointToPoint(
                    SplashKit.RectangleCenter(_boundingBox),
                    SplashKit.PointAt(x, y)
                )
            );
            RotationAngle = angle;
        }

        public void Draw(Window window)
        {
            if (GameConfig.DEBUG)
                SplashKit.DrawRectangle(Color.Red, _boundingBox);

            Bitmap bitmap = SplashKit.BitmapNamed(Sprite);
            Point2D bitmapCenter = SplashKit.BitmapCenter(bitmap);
            SplashKit.DrawBitmap(
                bitmap,
                _boundingBox.X, _boundingBox.Y,
                new DrawingOptions {
                    Dest = window,
                    ScaleX = 1,
                    ScaleY = 1,
                    Angle = (float)RotationAngle,
                }
            );
        }

        public Point2D Location
        {
            get
            {
                return SplashKit.RectangleCenter(_boundingBox);
            }
            set
            {
                _boundingBox.X = value.X - _boundingBox.Width / 2.0;
                _boundingBox.Y = value.Y - _boundingBox.Height / 2.0;
            }
        }

        public double Width
        {
            get
            {
                return _boundingBox.Width;
            }
            set
            {
                _boundingBox.Width = value;
            }
        }

        public double Height
        {
            get
            {
                return _boundingBox.Height;
            }
            set
            {
                _boundingBox.Height = value;
            }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return _boundingBox;
            }
        }
    }
}

