using System;
namespace BattleOfTanks
{
    public class RandomCannonFactory: IRandomWeaponFactory
    {
        public RandomCannonFactory()
        {
        }

        public Weapon GetWeapon()
        {
            Random random = new Random();
            int cannonId = random.Next(2);

            switch (cannonId)
            {
                case 0:
                    return new Cannon();
                case 1:
                    return new DualCannon();
                default:
                    return new Cannon();
            }
        }
    }
}

