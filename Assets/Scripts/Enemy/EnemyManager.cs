using System;
using System.Collections.Generic;
using Common;
using Config;
using Effects;
using Level;
using UnityEngine;

namespace Enemy
{
    public sealed class EnemyManager : ILevelStartListener, IGameOverListener, IUpdateListener, ILevelWonListener
    {
        public event Action<Enemy> OnEnemySpawn;
        public event Action<Enemy> OnEnemyUnspawn;

        private readonly HashSet<Enemy> _activeEnemies = new HashSet<Enemy>();
        private readonly EnemySpawner _enemySpawner;
        private readonly EffectsService _effectsService;
        private readonly Timer _spawnTimer;

        private bool _levelStarted;
        private readonly LevelBounds _levelBounds;
        private readonly LevelService _levelService;
        private GameConfig _gameConfig;

        public EnemyManager(EnemySpawner enemySpawner, EffectsService effectsService, LevelBounds levelBounds,
            GameConfig gameConfig, LevelService levelService)
        {
            _gameConfig = gameConfig;
            _levelService = levelService;
            _levelBounds = levelBounds;
            _effectsService = effectsService;
            _enemySpawner = enemySpawner;
            _spawnTimer = new Timer();
        }

        public void GameOver()
        {
            _levelStarted = false;
            _spawnTimer.OnTime -= SpawnEnemy;
        }

        public void LevelStarted(int levelIndex)
        {
            Clear();
            _spawnTimer.SetInterval(_gameConfig.EnemySpawnInterval * Mathf.Pow(0.8f, levelIndex));
            _levelStarted = true;
            _spawnTimer.OnTime += SpawnEnemy;
        }

        public void DoUpdate(float dt)
        {
            if (!_levelStarted)
                return;

            _spawnTimer.Update(dt);
            foreach (var activeEnemy in _activeEnemies)
            {
                _levelBounds.Warp(activeEnemy.transform);
            }
        }

        private void SpawnEnemy()
        {
            _spawnTimer.Reset();
            var enemyInstance = _enemySpawner.SpawnEnemy();
            if(_activeEnemies.Add(enemyInstance))
                OnEnemySpawn?.Invoke(enemyInstance);
        }

        public void SetEnemyIsDead(GameObject gameObject)
        {
            var enemy = gameObject.GetComponent<Enemy>();
            if (enemy == null)
                throw new Exception("no enemy component");

            if (_activeEnemies.Remove(enemy))
            {
                OnEnemyUnspawn?.Invoke(enemy);
                _effectsService.ShowExplosionEffect(enemy.transform.position);
                _enemySpawner.Unspawn(enemy);
            }

            _levelService.AddScore(1);
        }

        private void Clear()
        {
            foreach (var enemy in _activeEnemies)
            {
                OnEnemyUnspawn?.Invoke(enemy);
            }

            _spawnTimer.Reset();
            _enemySpawner.Clear();
            _activeEnemies.Clear();
        }

        public void LevelWon(int levelIndex)
        {
            Clear();
            _levelStarted = false;
        }
    }
}