using SplashKitSDK;

namespace BattleOfTanks
{
    public interface IMapTile
    {
        public void Draw(Window window);

        public bool ObjectCollision(PhysicalObject obj);
    }
}

