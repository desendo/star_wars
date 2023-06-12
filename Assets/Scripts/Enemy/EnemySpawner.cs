using Character;
using Common;
using Components;
using Config;
using Enemy.Agents;
using Level;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner
    {
        private readonly EnemyPool _enemyPool;
        private readonly CharacterService _characterService;
        private readonly GameConfig _gameConfig;
        private readonly LevelBounds _levelBounds;

        public EnemySpawner(EnemyPool enemyPool, LevelBounds levelBounds, CharacterService characterService, GameConfig gameConfig)
        {
            _enemyPool = enemyPool;
            _levelBounds = levelBounds;
            _characterService = characterService;
            _gameConfig = gameConfig;
        }

        public Enemy SpawnEnemy()
        {
            var enemy = _enemyPool.Spawn();
            var spawnPosition = _levelBounds.GetRandomPointInBorders();
            enemy.transform.position = spawnPosition;

            var attackAgent = enemy.GetComponent<EnemyAttackAgent>();
            var moveAgent = enemy.GetComponent<EnemyMoveAgent>();

            enemy.GetComponent<HitPointsComponent>().SetHitPoints(_gameConfig.EnemyHealth);
            moveAgent.SetSpeed(_gameConfig.EnemyMoveSpeed);


            var moveDir = _levelBounds.GetRandomPointInBorders();
            moveAgent.SetDestination(moveDir);

            LayerMask weaponHitMask = new LayerMask();
            weaponHitMask |= (1 << (int) PhysicsLayer.Obstacle) | (1 << (int) PhysicsLayer.Player);
            attackAgent.SetDamage(weaponHitMask, _gameConfig.EnemyDamage);
            attackAgent.SetTarget(_characterService.Character);
            attackAgent.SetFireInterval(_gameConfig.EnemyFireInterval);

            return enemy;
        }

        public void Unspawn(Enemy enemy)
        {
            _enemyPool.Unspawn(enemy);
        }

        public void Clear()
        {
            _enemyPool.Clear();
        }
    }
}