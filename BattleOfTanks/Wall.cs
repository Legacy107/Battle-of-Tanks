namespace BattleOfTanks
{
    public class Wall: StaticObject, IMapTile
    {
        public Wall(double x, double y)
            : base("Wall", x, y, 0)
        {
        }

        public bool ObjectCollision(PhysicalObject obj)
        {
            return IsCollided(obj);
        }
    }
}
