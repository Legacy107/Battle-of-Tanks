using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Cannon: Weapon
    {
        public Cannon(double damage)
            : base(damage)
        {
        }

        public Cannon()
            : base(20)
        {
        }

        public override List<Bullet> CreateBullet()
        {
            return new List<Bullet> { new Bullet(22, -6) };
        }
    }
}

