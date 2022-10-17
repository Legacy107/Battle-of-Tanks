using System;
using System.Xml.Linq;

namespace BattleOfTanks
{
    public class SlowEffect: EffectCommand
    {
        private Tank _tank;
        private double _slowStrength;
        private double _originalMaxSpeed;
        private double _originalFriction;

        public SlowEffect(Tank tank, double slowStrength, double duration = 0.5)
            : base(duration, true)
        {
            _tank = tank;
            _slowStrength = slowStrength;
            _originalMaxSpeed = tank.MaxSpeed;
            _originalFriction = tank.Friction;
        }

        public override void Execute()
        {
            _tank.Friction = _originalFriction * _slowStrength;
            _tank.MaxSpeed = _originalMaxSpeed / _slowStrength;
        }

        public override void OnRemove()
        {
            _tank.MaxSpeed = _originalMaxSpeed;
            _tank.Friction = _originalFriction;
        }

        public override bool Equals(EffectCommand? otherEffect)
        {
            if (otherEffect is SlowEffect other)
                return _tank.Equals(other._tank);

            return false;
        }
    }
}

