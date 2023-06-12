using System;
using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;

namespace Pool
{
    public class PoolBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private Transform _worldTransform;
        [SerializeField] private Transform _container;
        [SerializeField] private T _prefab;
        [SerializeField] private int _initialPoolSize;
        [SerializeField] private bool _injectOnSpawn;
        [SerializeField] private bool _injectOnInstantiate;

        private readonly Queue<T> _pool = new Queue<T>();
        private readonly HashSet<T> _active = new HashSet<T>();
        private DependencyContainer _dependencyContainer;

        [Inject]
        public void Construct(DependencyContainer dependencyContainer)
        {
            _dependencyContainer = dependencyContainer;
        }

        private void Awake()
        {
            for (var i = 0; i < _initialPoolSize; i++)
            {
                AddToPool();
            }
        }

        private void AddToPool()
        {
            var instance = GameObject.Instantiate(_prefab, _container);
            if (_injectOnInstantiate)
            {
                _dependencyContainer.Inject(instance);
                var monoBehaviours = instance.GetComponentsInChildren<MonoBehaviour>();
                if (monoBehaviours != null)
                    foreach (var monoBehaviour in monoBehaviours)
                    {
                        _dependencyContainer.Inject(monoBehaviour);
                    }
                var monoBehavioursSelf = instance.GetComponents<MonoBehaviour>();
                if (monoBehaviours != null)
                    foreach (var monoBehaviour in monoBehavioursSelf)
                    {
                        _dependencyContainer.Inject(monoBehaviour);
                    }
            }

            _pool.Enqueue(instance);
        }

        public virtual T Spawn()
        {
            if(_pool.Count == 0)
                AddToPool();

            if (_pool.TryDequeue(out var instance))
            {
                if (_injectOnSpawn)
                {
                    _dependencyContainer.Inject(instance);
                    var monoBehaviours = instance.GetComponentsInChildren<MonoBehaviour>();
                    if (monoBehaviours != null)
                        foreach (var monoBehaviour in monoBehaviours)
                        {
                            _dependencyContainer.Inject(monoBehaviour);
                        }
                    var monoBehavioursSelf = instance.GetComponents<MonoBehaviour>();
                    if (monoBehaviours != null)
                        foreach (var monoBehaviour in monoBehavioursSelf)
                        {
                            _dependencyContainer.Inject(monoBehaviour);
                        }
                }

                instance.transform.SetParent(_worldTransform);
                if(_active.Add(instance))
                    return instance;

                throw new Exception($"already {typeof(T)} in active");
            }
            throw new Exception($"no {typeof(T)} in pool");
        }

        public void Unspawn(T instance)
        {
            instance.transform.SetParent(_container);

            if (_active.Contains(instance))
            {
                _pool.Enqueue(instance);
                _active.Remove(instance);
            }
        }

        public void Clear()
        {
            foreach (var instance in _active)
            {
                instance.transform.SetParent(_container);
                _pool.Enqueue(instance);
            }
            _active.Clear();
        }
    }
}