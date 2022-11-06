namespace BattleOfTanks
{
    public class Upgrade: Area, IMapTile
    {
        private IRandomWeaponFactory _weaponFactory;

        public Upgrade(double x, double y, IRandomWeaponFactory weaponFactory)
            : base("Upgrade", x, y, 0)
        {
            _weaponFactory = weaponFactory;
        }

        public override bool IsInside(GameObject obj)
        {
            bool isInside = base.IsInside(obj);

            if (isInside && obj is Tank tank)
            {
                tank.Weapon = _weaponFactory.GetWeapon();
                NeedRemoval = true;
            }

            return isInside;
        }

        public bool ObjectCollision(PhysicalObject obj)
        {
            return IsInside(obj);
        }
    }
}

