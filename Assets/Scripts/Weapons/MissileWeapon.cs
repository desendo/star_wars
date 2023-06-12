using DependencyInjection;
using Projectile;
using UnityEngine;

namespace Weapons
{
    public class MissileWeapon : WeaponBase
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _acc;
        [SerializeField] private float _radius;
        private ProjectileManager _projectileManager;
        public override string Id => "Missile";

        [Inject]
        public void Construct(ProjectileManager projectileManager)
        {
            _projectileManager = projectileManager;
        }

        protected override void OnFire()
        {
            _projectileManager.FlyProjectileByArgs(new ProjectileManager.Args
            {
                Type = ProjectileManager.ProjectileType.Missile,
                PhysicsLayer = gameObject.layer,
                Color = _color,
                Direction = _firePoint.up,
                Damage = Damage,
                Position = _firePoint.position,
                Velocity =  _firePoint.up * _speed,
                DamageLayerMask = DamageLayerMask,
                AdditionalArgs = new ProjectileManager.MissileArgs()
                {
                    Radius = _radius,
                    Thrust = _acc
                }
            });
        }
    }
}