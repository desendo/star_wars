using Components;
using UnityEngine;
using Weapons;

namespace Enemy.Agents
{
    public sealed class EnemyAttackAgent : MonoBehaviour
    {
        [SerializeField] private WeaponBase _weaponComponent;
        [SerializeField] private EnemyMoveAgent _moveAgent;
        private float _countdown;
        private GameObject _target;


        private void Update()
        {
            if (!_target) return;

            if (!_target.GetComponent<HitPointsComponent>().IsHitPointsExists())
                return;

            _weaponComponent.DoUpdate(Time.deltaTime);
            _weaponComponent.AimTo(_target.transform.position);
            _weaponComponent.TryFire();
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }
        public void SetDamage(int hitMask, int damage)
        {
            _weaponComponent.Setup(hitMask, damage);
        }
        public void SetFireInterval(float interval)
        {
            _weaponComponent.SetInterval(interval);
        }
    }
}