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
            if (collided && obj is PhysicalObject physicalObject)
            {
                Line ray = SplashKit.LineFrom(
                    Location,
                    physicalObject.Location
                );
                Vector2D? surfaceNormalVector = null;
                Vector2D? newVelo = null;
                foreach (Line side in Sides)
                {
                    if (SplashKit.LinesIntersect(ray, side))
                        surfaceNormalVector = SplashKit.VectorInvert(SplashKit.LineNormal(side));
                }
                
                if (surfaceNormalVector is Vector2D _surfaceNormalVector)
                {
                    double dotProduct = (
                        (
                            _surfaceNormalVector.X * physicalObject.Velo.X +
                            _surfaceNormalVector.Y * physicalObject.Velo.Y
                        ) /
                        (
                            SplashKit.VectorMagnitude(_surfaceNormalVector) *
                            SplashKit.VectorMagnitude(physicalObject.Velo)
                        )
                    );
                    newVelo = SplashKit.VectorAdd(
                        physicalObject.Velo,
                        SplashKit.VectorInvert(SplashKit.VectorMultiply(
                            _surfaceNormalVector,
                            SplashKit.VectorMagnitude(physicalObject.Velo) *
                            dotProduct  // cos(angle) = dot product
                        ))
                    );
                }

                physicalObject.Location = SplashKit.PointOffsetBy(
                    physicalObject.Location,
                    SplashKit.VectorOutOfRectFromRect(
                        physicalObject.BoundingBox,
                        BoundingBox,
                        physicalObject.Velo
                    )
                );

                if (newVelo is Vector2D newVeloVector)
                    physicalObject.Velo = newVeloVector;
            }

            return collided;
        }
    }
}
