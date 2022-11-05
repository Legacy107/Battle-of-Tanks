namespace BattleOfTanks
{
    public abstract class EffectBuilder
    {
        public double Scalar { get; set; }

        public EffectBuilder AddScalar(double scalar)
        {
            Scalar = scalar;
            return this;
        }

        public abstract EffectBuilder AddSubject(GameObject subject);

        public abstract EffectCommand GetEffectCommand();
    }
}

