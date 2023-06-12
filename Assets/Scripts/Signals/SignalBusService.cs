using System;
using System.Collections.Generic;
using ReactiveExtension;
using UnityEngine;

namespace Signals
{
    public class SignalBusService
    {
        private readonly Dictionary<Type, List<object>> _subscriptions = new Dictionary<Type, List<object>>();
        private readonly Dictionary<Type, List<object>> _unsubscribeBuffer = new Dictionary<Type, List<object>>();
        private readonly HashSet<Type> _currentlyFiring = new HashSet<Type>();
        public void Fire<T>(T signal)
        {
            var type = typeof(T);
            if (!_subscriptions.TryGetValue(type, out var callbacks))
            {
                Debug.LogWarning($"fire {type}: no subscriptions");
                return;
            }

            _currentlyFiring.Add(type);

            foreach (var obj in callbacks)
            {
                if (obj is Action<T> callback)
                    callback.Invoke(signal);
                else if (obj is Action callbackVoid)
                    callbackVoid.Invoke();
            }

            if (_unsubscribeBuffer.ContainsKey(type))
            {
                foreach (var obj in _unsubscribeBuffer[type])
                    callbacks.Remove(obj);
            }

            _currentlyFiring.Remove(typeof(T));
        }

        public IDisposable Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_subscriptions.ContainsKey(type))
            {
                _subscriptions.Add(typeof(T), new List<object>());
            }
            _subscriptions[typeof(T)].Add(callback);
            return new DisposeContainer(()=>DisposeCallback<T>(callback));
        }
        public IDisposable Subscribe<T>(Action callback)
        {
            var type = typeof(T);
            if (!_subscriptions.ContainsKey(type))
            {
                _subscriptions.Add(typeof(T), new List<object>());
            }
            _subscriptions[typeof(T)].Add(callback);
            return new DisposeContainer(()=>DisposeCallback<T>(callback));
        }
        public void UnSubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_currentlyFiring.Contains(type) && _subscriptions.ContainsKey(type))
                _subscriptions[type].Remove(callback);
            //eсли fire в процессе, отписку кладем в буффер, который вызовем в конце fire
            else
            {
                if (!_unsubscribeBuffer.ContainsKey(type))
                    _unsubscribeBuffer.Add(type, new List<object>());
                _unsubscribeBuffer[type].Add(callback);
            }
        }

        private void DisposeCallback<T>(object callback)
        {
            if(callback is Action<T> action)
                UnSubscribe<T>(action);
            else
                throw new Exception($"SignalBusService.DisposeCallback: callback is not {typeof(Action<T>)}");
        }
    }
}