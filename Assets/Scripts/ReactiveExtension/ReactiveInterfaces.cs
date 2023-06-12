using System;

namespace ReactiveExtension
{

    public interface IEvent : IReadOnlyEvent
    {
        void Invoke();
    }

    public interface IReadOnlyEvent
    {
        public IDisposable Subscribe(Action callback);
    }
    public interface IEvent<T1, T2> : IReadOnlyEvent<T1, T2>
    {
        void Invoke(T1 value1, T2 value2);
    }

    public interface IReadOnlyEvent<out T1, out T2>
    {
        public IDisposable Subscribe(Action<T1, T2> callback);
    }
    public interface IEvent<T> : IReadOnlyEvent<T>
    {
        void Invoke(T value);
    }

    public interface IReadOnlyEvent<out T>
    {
        public IDisposable Subscribe(Action<T> callback);
    }
    public interface IReactive<T> : IReadonlyReactive<T>
    {
        new T Value { get; set; }
    }
    public interface IReadonlyReactive<out T>
    {
        public IDisposable Subscribe(Action<T> callback);
        T Value { get;}
    }
}