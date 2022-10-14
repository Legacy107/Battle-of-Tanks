using System;
using System.IO;
using SplashKitSDK;

namespace ShapeDrawer
{
    public static class ExtensionMethods
    {
        public static int ReadInteger(this StreamReader reader)
        {
            return Convert.ToInt32(reader.ReadLine());
        }

        public static double ReadDouble(this StreamReader reader)
        {
            return Convert.ToDouble(reader.ReadLine());
        }

        public static Point2D ReadPoint2D(this StreamReader reader)
        {
            return SplashKit.PointAt(ReadDouble(reader), ReadDouble(reader));
        }
    }
}