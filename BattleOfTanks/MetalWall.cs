using SplashKitSDK;

namespace BattleOfTanks
{
    public class MetalWall: StaticObject, IMapTile
    {
        public MetalWall(double x, double y)
            : base("Steel", x, y, 0)
        {
        }

        public bool ObjectCollision(PhysicalObject obj)
        {
            return IsCollided(obj);
        }
    }
}

