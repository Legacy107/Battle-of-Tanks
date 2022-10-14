using SplashKitSDK;

namespace BattleOfTanks
{
    public class StaticObject: GameObject
    {
        public StaticObject(string sprite, double x, double y, double angle)
            : base(sprite, x, y, angle)
        {
        }

        public bool IsCollided(GameObject obj)
        {
            bool collided = SplashKit.RectanglesIntersect(BoundingBox, obj.BoundingBox);
            if (collided)
            {
                if (obj is PhysicalObject physicalObject)
                {
                    physicalObject.Location = SplashKit.PointOffsetBy(
                        physicalObject.Location,
                        SplashKit.VectorOutOfRectFromRect(
                            physicalObject.BoundingBox,
                            BoundingBox,
                            physicalObject.Velo
                        )
                    );
                    physicalObject.Velo = SplashKit.VectorTo(0, 0);
                }
            }

            return collided;
        }
    }
}
