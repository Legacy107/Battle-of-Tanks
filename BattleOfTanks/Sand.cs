namespace BattleOfTanks
{
    public class Sand: Area, IMapTile
    {
        private EffectBuilder _effectBuilder;

        public Sand(double x, double y)
            : base("Sand", x, y, 0)
        {
            _effectBuilder = new SlowEffectBuilder();
            _effectBuilder.AddScalar(2);
        }

        public override bool IsInside(GameObject obj)
        {
            bool isInside = base.IsInside(obj);

            if (isInside && obj is Tank)
            {
                _effectBuilder.AddSubject(obj);
                CommandExecutor.Instance.AddCommand(
                    _effectBuilder.GetEffectCommand()
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

