using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveExtension
{
    public class Reactive<T> : IReactive<T>
    {
        private T _value;

        private readonly List<Action<T>> _actions = new List<Action<T>>();
        private readonly List<Action> _voidActions = new List<Action>();
        private readonly HashSet<Action<T>> _skipInvoke = new HashSet<Action<T>>();
        private readonly HashSet<Action> _skipVoidInvoke = new HashSet<Action>();
        private bool _invokeStarted;

        public Reactive(T value = default)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    InvokeSubscriptions(value);
                }
            }
        }

        public IDisposable Subscribe(Action<T> callback)
        {
            _actions.Add(callback);
            callback?.Invoke(_value);
            return new DisposeContainer(()=>DisposeCallback(callback));
        }
        public IDisposable Subscribe(Action callback)
        {
            _voidActions.Add(callback);
            callback?.Invoke();
            return new DisposeContainer(()=>DisposeCallback(callback));
        }
        private void DisposeCallback(Action<T> callback)
        {
            if(!_invokeStarted)
                _actions.Remove(callback);
            else
                _skipInvoke.Add(callback);

        }
        private void DisposeCallback(Action callback)
        {
            if(!_invokeStarted)
                _voidActions.Remove(callback);
            else
                _skipVoidInvoke.Add(callback);

        }

        private void InvokeSubscriptions(T value)
        {
            _invokeStarted = true;
            foreach (var action in _actions)
            {
                if(!_skipInvoke.Contains(action))
                    action?.Invoke(value);
            }
            foreach (var action in _voidActions)
            {
                if(!_skipVoidInvoke.Contains(action))
                    action?.Invoke();
            }
            _invokeStarted = false;
            _skipVoidInvoke.Clear();
            _skipInvoke.Clear();
        }
    }
}