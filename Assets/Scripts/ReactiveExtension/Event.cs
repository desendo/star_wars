using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveExtension
{
    public class Event : IEvent
    {
        private readonly List<Action> _actions = new List<Action>();

        public void Invoke()
        {
            InvokeSubscriptions();
        }

        public IDisposable Subscribe(Action callback)
        {
            _actions.Add(callback);
            return new DisposeContainer(() => DisposeCallback(callback));
        }

        private void DisposeCallback(Action callback)
        {
            _actions.Remove(callback);
        }

        private void InvokeSubscriptions()
        {
            var cache = _actions.ToList();

            foreach (var action in cache)
            {
                action?.Invoke();
            }
        }
    }

    public class Event<T1, T2> : IEvent<T1,T2>
    {
        private readonly List<Action<T1,T2>> _actions = new List<Action<T1,T2>>();

        public void Invoke(T1 value1, T2 value2)
        {
            InvokeSubscriptions(value1, value2);
        }

        public IDisposable Subscribe(Action<T1,T2> callback)
        {
            _actions.Add(callback);
            return new DisposeContainer(()=>DisposeCallback(callback));
        }

        private void DisposeCallback(Action<T1,T2> callback)
        {
            _actions.Remove(callback);
        }

        private void InvokeSubscriptions(T1 value1, T2 value2)
        {
            var cache = _actions.ToList();
            foreach (var action in cache)
            {
                action?.Invoke(value1, value2);
            }
        }
    }
    public class Event<T1> : IEvent<T1>
    {
        private readonly List<Action<T1>> _actions = new List<Action<T1>>();

        public void Invoke(T1 value1)
        {
            InvokeSubscriptions(value1);
        }

        public IDisposable Subscribe(Action<T1> callback)
        {
            _actions.Add(callback);
            return new DisposeContainer(()=>DisposeCallback(callback));
        }

        private void DisposeCallback(Action<T1> callback)
        {
            _actions.Remove(callback);
        }

        private void InvokeSubscriptions(T1 value1)
        {
            var cache = _actions.ToList();
            foreach (var action in cache)
            {
                action?.Invoke(value1);
            }
        }
    }
}