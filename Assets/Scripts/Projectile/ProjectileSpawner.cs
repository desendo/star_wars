using System;

namespace Projectile
{
    public sealed class ProjectileSpawner
    {
        private readonly BulletPool _bulletPool;
        private readonly MissilePool _missilePool;

        public ProjectileSpawner(BulletPool bulletPool, MissilePool missilePool)
        {
            _missilePool = missilePool;
            _bulletPool = bulletPool;
        }

        public IProjectile SpawnProjectile(ProjectileManager.Args args)
        {
            if (args.Type == ProjectileManager.ProjectileType.Bullet)
            {
                var projectile = _bulletPool.Spawn();
                SetupProjectile(args, projectile);
                return projectile;
            }
            if (args.Type == ProjectileManager.ProjectileType.Missile)
            {
                var projectile = _missilePool.Spawn();
                SetupProjectile(args, projectile);
                if (args.AdditionalArgs is ProjectileManager.MissileArgs missileArgs)
                {
                    projectile.SetThrust(missileArgs.Thrust);
                    projectile.SetRadius(missileArgs.Radius);
                }
                return projectile;
            }
            throw new Exception($"cant handle spawn projectile {args.Type} ");
        }

        private static void SetupProjectile(ProjectileManager.Args args, IProjectile projectile)
        {
            projectile.SetPosition(args.Position);
            projectile.SetColor(args.Color);
            projectile.SetPhysicsLayer(args.PhysicsLayer);
            projectile.Damage = args.Damage;
            projectile.DamageLayerMask = args.DamageLayerMask;
            projectile.SetVelocity(args.Velocity);
            projectile.Transform.up = args.Direction;
        }
    }
}