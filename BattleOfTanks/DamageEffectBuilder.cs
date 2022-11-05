namespace BattleOfTanks
{
    public class DamageEffectBuilder: EffectBuilder
    {
        private ICanTakeDamage? _subject;

        public override EffectBuilder AddSubject(GameObject subject)
        {
            _subject = subject as ICanTakeDamage;
            return this;
        }

        public override EffectCommand GetEffectCommand()
        {
            if (_subject is null)
                return new NoOpEffect();

            return new DamageEffect(_subject, Scalar);
        }
    }
}

