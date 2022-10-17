namespace BattleOfTanks
{
    public class DamageEffect: EffectCommand
    {
        private ICanTakeDamage _subject;
        private double _damage;

        public DamageEffect(ICanTakeDamage subject, double damange, double duration = 0)
            : base(duration)
        {
            _subject = subject;
            _damage = damange;
        }

        public override void Execute()
        {
            _subject.TakeDamage(_damage);
        }
    }
}

