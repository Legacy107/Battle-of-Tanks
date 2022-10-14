using SplashKitSDK;

namespace BattleOfTanks
{
    public class Area: GameObject
    {
        public Area(string sprite, double x, double y, double angle)
            : base(sprite, x, y, angle)
        {
        }

        public virtual bool IsInside(GameObject obj)
        {
            return SplashKit.RectanglesIntersect(BoundingBox, obj.BoundingBox);
        }
    }
}

