namespace BattleOfTanks
{
    public class DamageEffectBuilder: EffectBuilder
    {
        private ICanTakeDamage? _subject;

        public override void AddSubject(GameObject subject)
        {
            _subject = subject as ICanTakeDamage;
        }

        public override EffectCommand GetEffectCommand()
        {
            if (_subject is null)
                return new NoOpEffect();

            return new DamageEffect(_subject, Scalar);
        }
    }
}

