using System;

namespace BattleOfTanks
{
    public abstract class EffectCommand: IEquatable<EffectCommand>
    {
        public double Duration { get; set; }
        private bool _unique;

        public EffectCommand(double duration = 0, bool unique = false)
        {
            Duration = duration;
            _unique = unique;
        }

        public abstract void Execute();

        public virtual void OnRemove()
        {
        }

        public virtual bool Equals(EffectCommand? other)
        {
            return Object.Equals(this, other);
        }

        public bool Unique
        {
            get
            {
                return _unique;
            }
        }
    }
}

