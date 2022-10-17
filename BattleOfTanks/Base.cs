using System.Collections.Generic;

namespace BattleOfTanks
{
    public class Base: StaticObject, IMapTile, ICanTakeDamage, IObserverable
    {
        private List<IObserver> _observers;

        public Base(double x, double y)
            : base("Base", x, y, 0)
        {
            _observers = new List<IObserver>();
        }

        public void NotifyObserver()
        {
            foreach (IObserver observer in _observers)
                observer.ObserverUpdate();
        }

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public bool ObjectCollision(PhysicalObject obj)
        {
            return IsCollided(obj);
        }

        public void TakeDamage(double damage)
        {
            NeedRemoval = true;
            NotifyObserver();
        }
    }
}

