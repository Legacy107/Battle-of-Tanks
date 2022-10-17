namespace BattleOfTanks
{
    public class Sand: Area, IMapTile
    {
        private EffectBuilder effectBuilder;

        public Sand(double x, double y)
            : base("Sand", x, y, 0)
        {
            effectBuilder = new SlowEffectBuilder();
            effectBuilder.AddScalar(2);
        }

        public override bool IsInside(GameObject obj)
        {
            bool isInside = base.IsInside(obj);

            if (isInside && obj is Tank)
            {
                effectBuilder.AddSubject(obj);
                CommandExecutor.Instance.AddCommand(
                    effectBuilder.GetEffectCommand()
                );
            }

            return isInside;
        }

        public bool ObjectCollision(PhysicalObject obj)
        {
            return IsInside(obj);
        }
    }
}

