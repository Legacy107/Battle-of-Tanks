namespace BattleOfTanks
{
    public class Sand: Area, IMapTile
    {
        public Sand(double x, double y)
            : base("Sand", x, y, 0)
        {
        }

        public override bool IsInside(GameObject obj)
        {
            bool isInside = base.IsInside(obj);

            // TODO: add slow effect
            if (isInside)
            {
            }

            return isInside;
        }

        public bool ObjectCollision(PhysicalObject obj)
        {
            return IsInside(obj);
        }
    }
}

