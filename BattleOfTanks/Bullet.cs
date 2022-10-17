using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Bullet: PhysicalObject
    {
        public Vector2D Offset { get; set; }
        private List<EffectBuilder> _effectBuilders;

        public Bullet
        (
            List<EffectBuilder> effectBuilders,
            double offsetX,
            double offsetY,
            double angle = 0,
            double speed = 200
        )
            : base("Bullet", 0, 0, angle, 200, 500, 50)
        {
            _effectBuilders = effectBuilders;
            Velo = SplashKit.VectorFromAngle(RotationAngle, speed);
            Offset = SplashKit.VectorTo(offsetX, offsetY);
        }

        public override bool IsCollided(GameObject obj)
        {
            bool collided = base.IsCollided(obj);

            if (collided)
            {
                if (!(obj is Area))
                    NeedRemoval = true;

                if (obj is ICanTakeDamage)
                    foreach (EffectBuilder effectBuilder in _effectBuilders)
                    {
                        effectBuilder.AddSubject(obj);
                        CommandExecutor.Instance.AddCommand(
                            effectBuilder.GetEffectCommand()
                        );
                    }
            }

            return collided;
        }
    }
}

