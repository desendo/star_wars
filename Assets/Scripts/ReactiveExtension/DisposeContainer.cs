using System;

namespace ReactiveExtension
{
    internal class DisposeContainer : IDisposable
    {
        private readonly Action _disposeAction;

        public DisposeContainer(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            _disposeAction.Invoke();
        }
    }
}