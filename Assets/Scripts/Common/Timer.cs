using System;
using ReactiveExtension;
using UnityEngine;

namespace Common
{
    public interface ITimer
    {
        public IReadonlyReactive<float> ProgressNormalized { get; }
        public void SetInterval(float interval);
    }

    public class Timer : ITimer
    {
        private float _interval;
        private float _time;
        public event Action OnTime;
        private bool _timerReached;
        public bool TimerReached => _timerReached;
        public IReadonlyReactive<float> ProgressNormalized => _progressNormalized;
        private readonly Reactive<float> _progressNormalized = new Reactive<float>(0);

        public Timer(float interval)
        {
            _interval = interval;
        }
        public Timer()
        {
            _interval = 1f;
        }

        public void SetInterval(float interval)
        {
            _interval = interval;
            Reset();
        }

        public void Update(float delta)
        {
            _time += delta;
            if (_time > _interval && !_timerReached)
            {
                _timerReached = true;
                OnTime?.Invoke();
            }
            _progressNormalized.Value = Mathf.Clamp01(_time / _interval);
        }

        public void Reset()
        {
            _timerReached = false;
            _time = 0f;
        }

    }
}