using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Cannon: Weapon
    {
        public Cannon(List<EffectBuilder> effectBuilders)
            : base(effectBuilders)
        {
        }

        public Cannon()
            : base(new List<EffectBuilder> {
                new DamageEffectBuilder().AddScalar(20)
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
                    -6
                )
            };
        }
    }
}

