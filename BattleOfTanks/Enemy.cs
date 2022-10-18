using System.Collections.Generic;
using SplashKitSDK;

namespace BattleOfTanks
{
    public abstract class Enemy: IObserver
    {
        public int NoEnemy { get; set; }

        public Enemy(int noEnemy)
        {
            NoEnemy = noEnemy;
        }

        public abstract void SpawnEnemy(List<Tank> tanks, Point2D spawnPoint);

        public abstract void MoveTank(
            List<Tank> tanks,
            Tank playerTank,
            Base playerBase
        );

        public abstract List<Bullet> Shoot(
            List<Tank> tanks,
            Tank playerTank,
            Base playerBase
        );

        // Get notified when a tank is removed
        public virtual void ObserverUpdate(params object[] list)
        {
        }
    }
}

