using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class DualCannon: Weapon
    {
        public DualCannon(List<EffectBuilder> effectBuilders)
            : base(effectBuilders)
        {
        }

        public DualCannon()
            : base(new List<EffectBuilder> {
                new DamageEffectBuilder().AddScalar(15)
            })
        {
        }

        public override List<Bullet> CreateBullet()
        {
            return new List<Bullet>
            {
                new Bullet
                (
                    EffectBuilders,
                    28.25,
                    -12,
                    -20
                ),
                new Bullet
                (
                    EffectBuilders,
                    28.25,
                    0,
                    20
                )
            };
        }
    }
}

