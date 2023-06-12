using Projectile;
using UnityEngine;
using UnityEngine.Serialization;

namespace Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game Config/New Game Config")]
    public sealed class GameConfig : ScriptableObject
    {
        [SerializeField] public float CharMoveSpeed;
        [SerializeField] public float CharBreakSpeed;
        [SerializeField] public float CharRotationSpeed;
        [SerializeField] public float CharDrag;
        [SerializeField] public int CharHealth;
        [SerializeField] public float EnemySpawnInterval;
        [SerializeField] public int EnemyHealth;
        [SerializeField] public int EnemyDamage;

        [SerializeField] public float EnemyMoveSpeed;
        [SerializeField] public float EnemyFireInterval;
        [SerializeField] public int CharDamage = 1;
        [SerializeField] public float ObstacleHitVelocityTolerance = 1f;
        [SerializeField] public int ObstacleDivideCount = 4;
    }
}