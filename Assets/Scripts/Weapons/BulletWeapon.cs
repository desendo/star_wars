using Projectile;
using DependencyInjection;
using UnityEngine;

namespace Weapons
{
    public class BulletWeapon : WeaponBase
    {
        [SerializeField] private float _speed;
        public override string Id => "Cannon";
        private ProjectileManager _projectileManager;
        [Inject]
        public void Construct(ProjectileManager projectileManager)
        {
            _projectileManager = projectileManager;
        }

        protected override void OnFire()
        {
            _projectileManager.FlyProjectileByArgs(new ProjectileManager.Args
            {
                PhysicsLayer = gameObject.layer,
                Color = _color,
                Damage = Damage,
                Direction = _firePoint.up,
                Position = _firePoint.position,
                Velocity =  _firePoint.up * _speed,
                DamageLayerMask = DamageLayerMask,
                Type = ProjectileManager.ProjectileType.Bullet,
            });

        }
    }
}