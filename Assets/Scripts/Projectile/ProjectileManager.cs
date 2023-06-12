using System.Collections.Generic;
using Effects;
using Level;
using UnityEngine;
using Weapons;

namespace Projectile
{
    public sealed class ProjectileManager : IFixedUpdateListener, ILevelStartListener, ILevelWonListener
    {
        private readonly ProjectileSpawner _projectileSpawner;
        private readonly LevelBounds _levelBounds;
        private readonly EffectsService _effectsService;
        private readonly BulletPool _bulletPool;
        private readonly MissilePool _missilePool;

        private readonly HashSet<IProjectile> _activeProjectiles = new HashSet<IProjectile>();

        public ProjectileManager(ProjectileSpawner projectileSpawner, BulletPool bulletPool, MissilePool missilePool,
            LevelBounds levelBounds, EffectsService effectsService)
        {
            _projectileSpawner = projectileSpawner;
            _levelBounds = levelBounds;
            _effectsService = effectsService;
            _bulletPool = bulletPool;
            _missilePool = missilePool;
        }

        private void RemoveProjectile(IProjectile projectile)
        {
            if (_activeProjectiles.Remove(projectile))
            {
                projectile.OnCollisionEntered -= OnCollision;
                Unspawn(projectile);
            }
        }

        private void Unspawn(IProjectile projectile)
        {
            if (projectile is Bullet bullet)
                _bulletPool.Unspawn(bullet);
            if (projectile is Missile missile)
                _missilePool.Unspawn(missile);
        }

        public void FlyProjectileByArgs(Args args)
        {
            var spawnProjectile = _projectileSpawner.SpawnProjectile(args);
            if(_activeProjectiles.Add(spawnProjectile))
                spawnProjectile.OnCollisionEntered += OnCollision;
        }

        public void DoFixedUpdate(float dt)
        {
            var projectilesToRemove = new List<IProjectile>();

            foreach (var projectile in _activeProjectiles)
            {
                if (!_levelBounds.InBounds(projectile.Transform.position))
                {
                    projectilesToRemove.Add(projectile);
                }
            }

            foreach (var projectile in projectilesToRemove)
            {
                RemoveProjectile(projectile);
            }
        }

        private void OnCollision(IProjectile projectile, Collision2D collision)
        {
            var other = collision.gameObject;

            if (projectile.DamageLayerMask.value != (projectile.DamageLayerMask.value | (1 << other.layer)))
                return;

            if (other.TryGetComponent(out IDamage damaged))
                damaged.TakeDamage(projectile.Damage, projectile.GetDirection());

            if (projectile is Missile missile)
            {
                Collider2D[] results = new Collider2D[20];
                var count = Physics2D.OverlapCircleNonAlloc(missile.Transform.position, missile.Radius, results, projectile.DamageLayerMask.value);
                foreach (var collider2D in results)
                {
                    if(collider2D == null)
                        continue;

                    var damage = collider2D.GetComponent<IDamage>();
                    if(damage != damaged)
                        damage.TakeDamage(missile.Damage, collider2D.transform.position - missile.Transform.position);

                }

                _effectsService.ShowAoeEffect(missile.Transform.position, missile.Radius);
            }
            ShowHitEffect(collision);
            RemoveProjectile(projectile);
        }

        private void ShowHitEffect(Collision2D collision)
        {
            var midPoint = Vector2.zero;
            var midNormal = Vector2.zero;
            foreach (var contactPoint2D in collision.contacts)
            {
                midPoint += contactPoint2D.point;
                midNormal += contactPoint2D.normal;
            }

            midNormal /= collision.contacts.Length;
            midPoint /= collision.contacts.Length;
            _effectsService.ShowHitEffect(midPoint, midNormal);
        }
        public void LevelStarted(int levelIndex)
        {
            Clear();
        }
        private void Clear()
        {
            foreach (var projectile in _activeProjectiles)
            {
                projectile.OnCollisionEntered -= OnCollision;
                Unspawn(projectile);
            }
            _activeProjectiles.Clear();
        }
        public struct Args
        {
            public ProjectileType Type;
            public Vector2 Position;
            public Vector2 Direction;
            public Vector2 Velocity;
            public Color Color;
            public int PhysicsLayer;
            public int Damage;
            public int DamageLayerMask;
            public object AdditionalArgs;
        }
        public class MissileArgs
        {
            public float Thrust;
            public float Radius;
        }
        public enum ProjectileType
        {
            None = 0,
            Bullet = 1,
            Missile = 2
        }


        public void LevelWon(int levelIndex)
        {
            Clear();
        }
    }
}