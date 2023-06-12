using System.Collections.Generic;
using Components;
using Config;
using Effects;
using Level;
using UnityEngine;

namespace Obstacles
{
    public class ObstacleManager : ILevelStartListener, IUpdateListener, ILevelWonListener
    {
        private readonly ObstaclesPool _obstaclesPool;

        private readonly HashSet<Obstacle> _activeObstacles = new HashSet<Obstacle>();
        private readonly LevelBounds _levelBounds;
        private readonly EffectsService _effectsService;
        private readonly GameConfig _gameConfig;

        public ObstacleManager(ObstaclesPool obstaclesPool, LevelBounds levelBounds, EffectsService effectsService, GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _effectsService = effectsService;
            _obstaclesPool = obstaclesPool;
            _levelBounds = levelBounds;
        }

        public void LevelStarted(int levelIndex)
        {
            Clear();
            var count = levelIndex + 1;
            SpawnObstacles(count);
        }

        private void SpawnObstacles(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddObstacle(_levelBounds.GetRandomPointInBorders(),
                    Random.insideUnitCircle,
                    Random.Range(0, 0.1f));
            }
        }

        private void AddObstacle(Vector2 pos, Vector2 velocity, float angVel, int division = 0)
        {

            var view = _obstaclesPool.Spawn();
            view.DoReset();
            view.CurrentDivision = division;
            view.transform.position = pos;

            var rb = view.GetComponent<Rigidbody2D>();
            rb.velocity = velocity;
            rb.angularVelocity = angVel;

            view.OnCollisionEntered += OnObstacleCollision;
            view.OnWeaponHit += OnWeaponHit;

            _activeObstacles.Add(view);
        }

        private void OnWeaponHit(Obstacle obstacle, int damage, Vector2 dir)
        {
            if (_activeObstacles.Remove(obstacle))
            {
                var oldVelocity = obstacle.GetComponent<Rigidbody2D>().velocity;
                var oldAngularVelocity = obstacle.GetComponent<Rigidbody2D>().angularVelocity;
                var division = obstacle.CurrentDivision;
                var position = obstacle.transform.position;

                _obstaclesPool.Unspawn(obstacle);
                obstacle.OnCollisionEntered -= OnObstacleCollision;
                obstacle.OnWeaponHit -= OnWeaponHit;

                if (division > _gameConfig.ObstacleDivideCount)
                {
                    _effectsService.ShowExplosionEffect(position);
                    return;
                }


                var shift = Vector2.Perpendicular(dir) * 0.5f;

                AddObstacle((Vector2) position + shift,
                    oldVelocity + shift * oldVelocity.magnitude * 2,
                    oldAngularVelocity * 2f,
                    division + 1);
                AddObstacle((Vector2) position - shift,
                    oldVelocity - shift * oldVelocity.magnitude * 2,
                    oldAngularVelocity * 2f,
                    division + 1);

            }
        }

        private void OnObstacleCollision(Obstacle obstacle, Collision2D arg2)
        {
            var otherGameObject = arg2.collider.gameObject;

            if(otherGameObject.GetComponent<Obstacle>())
                return;

            var hitPoints = otherGameObject.GetComponent<HitPointsComponent>();
            if(hitPoints != null)
            {
                var hitVelocityMag = arg2.relativeVelocity.magnitude;
                if (hitVelocityMag > _gameConfig.ObstacleHitVelocityTolerance)
                {
                    hitPoints.TakeDamage(1, arg2.relativeVelocity);
                    _effectsService.ShowHitEffect(arg2);
                }
                else
                    _effectsService.ShowHitEffect(arg2, 0.2f);
            }
        }

        private void Clear()
        {
            foreach (var obstacleView in _activeObstacles)
            {
                _obstaclesPool.Unspawn(obstacleView);
                obstacleView.OnCollisionEntered -= OnObstacleCollision;
                obstacleView.OnWeaponHit -= OnWeaponHit;
            }
            _activeObstacles.Clear();
        }

        public void DoUpdate(float dt)
        {
            foreach (var obstacleView in _activeObstacles)
                _levelBounds.Warp(obstacleView.transform);
        }

        public void LevelWon(int levelIndex)
        {
            Clear();
        }
    }
}