using Projectile;
using Character;
using Config;
using DependencyInjection;
using Effects;
using Enemy;
using GameManager;
using Input;
using Level;
using Obstacles;
using Signals;
using UnityEngine;
using UnityEngine.Serialization;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private UpdateManager _updateManager;
    [SerializeField] private LevelBounds _levelBounds;
    [SerializeField] private GameObject _character;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private MissilePool _missilePool;
    [SerializeField] private AoeExplosionEffectPool _aoeExplosionEffectPool;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private ObstaclesPool _obstaclesPool;
    [SerializeField] private ExplosionEffectPool _explosionEffectPool;
    [SerializeField] private HitEffectPool _hitEffectPool;

    private DependencyContainer _container;
    private void Start()
    {
        Bind();
        //Initialize();
    }

    private void Bind()
    {
        _container = new DependencyContainer();
        _container.Bind(_container);
        _container.Bind<SignalBusService>();

        _container.Add(_gameConfig);

        _container.Add(_enemyPool);
        _container.Add(_bulletPool);
        _container.Add(_missilePool);
        _container.Add(_hitEffectPool);
        _container.Add(_explosionEffectPool);
        _container.Add(_obstaclesPool);
        _container.Add(_aoeExplosionEffectPool);

        _container.Add(_levelBounds);

        _container.Add<GameStateService>();
        _container.Add<LevelService>();
        _container.Bind<InputManager>();

        _container.Bind<EffectsService>();

        _container.Bind<ProjectileSpawner>();
        _container.Bind<ProjectileManager>();

        _container.Add<CharacterService>().SetCharacter(_character);

        _container.Bind<EnemySpawner>();
        _container.Bind<EnemyManager>();
        _container.Bind<EnemyDeathController>();

        _container.Bind<ObstacleManager>();
        _container.Bind<CharacterDeathObserver>();
        _container.Bind<CharacterWeaponController>();
        _container.Bind<CharacterMoveController>();
        _container.Bind<CharacterInstaller>();

        _container.Bind<GameStartController>();
        _container.Bind<GamePauseController>();
        _container.Bind<LevelFinishController>();
        _container.Bind<NextLevelController>();


        _container.Inject(_updateManager);
        //ищем по сцене кандидатов на инъекцию .Inject()
        SearchAndInject();
    }

    private void SearchAndInject()
    {
        var objects = FindObjectsOfType<Component>();
        foreach (var obj in objects)
        {
            var monoBehaviours = obj.GetComponents<MonoBehaviour>();
            if (monoBehaviours == null) continue;

            foreach (var monoBehaviour in monoBehaviours)
            {
                _container.Inject(monoBehaviour);
            }
        }
    }

    private void Initialize()
    {
        var inits = _container.GetList<IInit>();
        foreach (var init in inits)
        {
            init.Init();
        }
    }
}