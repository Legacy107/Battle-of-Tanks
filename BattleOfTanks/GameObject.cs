﻿using SplashKitSDK;

namespace BattleOfTanks
{
    public class GameObject
    {
        string _sprite;
        Rectangle _rectangle;
        private double _rotationAngle;

        public GameObject(string sprite, double x, double y, double angle)
        {
            _sprite = sprite;
            _rectangle = SplashKit.BitmapNamed(_sprite).BoundingRectangle(x, y);
            // Translate bounding box so that its center is at the coordinate
            SplashKit.RectangleOffsetBy(
                _rectangle,
                SplashKit.VectorPointToPoint(
                    SplashKit.RectangleCenter(_rectangle),
                    SplashKit.PointAt(x, y)
                )
            );
            _rotationAngle = angle;
        }

        public void Draw(Window window)
        {
            SplashKit.DrawRectangle(Color.Red, _rectangle);
            Bitmap bitmap = SplashKit.BitmapNamed(_sprite);
            Point2D bitmapCenter = SplashKit.BitmapCenter(bitmap);
            SplashKit.DrawBitmap(
                bitmap,
                _rectangle.X, _rectangle.Y,
                new DrawingOptions {
                    Dest = window,
                    ScaleX = 1,
                    ScaleY = 1,
                    Angle = (float)_rotationAngle,
                }
            );
        }

        public Point2D Location
        {
            get
            {
                return SplashKit.RectangleCenter(_rectangle);
            }
            set
            {
                _rectangle.X = value.X - _rectangle.Width / 2.0;
                _rectangle.Y = value.Y - _rectangle.Height / 2.0;
            }
        }

        public double Width
        {
            get
            {
                return _rectangle.Width;
            }
            set
            {
                _rectangle.Width = value;
            }
        }

        public double Height
        {
            get
            {
                return _rectangle.Height;
            }
            set
            {
                _rectangle.Height = value;
            }
        }

        public string Sprite
        {
            get
            {
                return _sprite;
            }
            set
            {
                _sprite = value;
            }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return _rectangle;
            }
        }

        public double RotationAngle
        {
            get
            {
                return _rotationAngle;
            }
            set
            {
                _rotationAngle = value;
            }
        }
    }
}

