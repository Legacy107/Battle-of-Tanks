namespace BattleOfTanks
{
    public class SlowEffectBuilder: EffectBuilder
    {
        private Tank? _subject;

        public override void AddSubject(GameObject subject)
        {
            _subject = subject as Tank;
        }

        public override EffectCommand GetEffectCommand()
        {
            if (_subject is null)
                return new NoOpEffect();

            return new SlowEffect(_subject, Scalar);
        }
    }
}

