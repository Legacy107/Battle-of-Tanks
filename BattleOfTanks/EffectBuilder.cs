namespace BattleOfTanks
{
    public abstract class EffectBuilder
    {
        public double Scalar { get; set; }

        public void AddScalar(double scalar)
        {
            Scalar = scalar;
        }

        public abstract void AddSubject(GameObject subject);

        public abstract EffectCommand GetEffectCommand();
    }
}

