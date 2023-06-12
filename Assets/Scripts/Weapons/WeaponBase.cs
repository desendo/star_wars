using Common;
using ReactiveExtension;
using UnityEngine;

namespace Weapons
{
    public class WeaponBase : MonoBehaviour, IWeapon
    {
        [SerializeField] protected Color _color;
        [SerializeField] protected Transform _firePoint;
        [SerializeField] protected float _interval;

        public virtual string Id { get; }
        public IReadonlyReactive<float> ProgressNormalized => FireTimer.ProgressNormalized;

        protected readonly Timer FireTimer = new Timer();
        protected int Damage;
        protected int DamageLayerMask;
        public virtual void TryFire()
        {
            var canFire = FireTimer.TimerReached;

            if (canFire)
            {
                OnFire();
                FireTimer.Reset();
            }
        }
        public void SetInterval(float interval)
        {
            FireTimer.SetInterval(interval);
        }
        protected virtual void OnFire()
        {
        }

        public void Setup(int hitMask, int damage)
        {
            FireTimer.SetInterval(_interval);
            DamageLayerMask = hitMask;
            Damage = damage;
        }

        public void AimTo(Vector2 target)
        {
            var dir = (target - (Vector2) _firePoint.position).normalized;
            _firePoint.transform.up = dir;
        }

        public void DoUpdate(float dt)
        {
            FireTimer.Update(dt);
        }
    }
}