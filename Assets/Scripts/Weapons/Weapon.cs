using Common;
using UnityEngine;

namespace Weapons
{
    public interface IWeapon: ITimer, IUpdateListener
    {
        public string Id { get; }
        void TryFire();
        public void Setup(int hitMask, int damage);
        public void AimTo(Vector2 target);

    }

    public interface IDamage
    {
        void TakeDamage(int damage, Vector2 dir);
    }
}