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
            DamageEffectBuilder damageEffectBuilder = new DamageEffectBuilder();
            damageEffectBuilder.AddScalar(Damage);

            return new List<Bullet>
            {
                new Bullet
                (
                    new List<EffectBuilder> { damageEffectBuilder },
                    28.25,
                    -6
                )
            };
        }
    }
}

