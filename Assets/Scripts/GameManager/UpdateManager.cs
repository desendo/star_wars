using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;

namespace GameManager
{
    public class UpdateManager : MonoBehaviour
    {
        private List<IUpdateListener> _updates;
        private List<IFixedUpdateListener> _fixedUpdates;
        private bool _isInitialized;

        [Inject]
        public void Construct(List<IUpdateListener> updateListeners, List<IFixedUpdateListener> fixedUpdateListeners)
        {
            _updates = updateListeners;
            _fixedUpdates = fixedUpdateListeners;
            _isInitialized = true;
        }

        private void FixedUpdate()
        {
            if (!_isInitialized) return;

            foreach (var fixedUpdate in _fixedUpdates)
                fixedUpdate.DoFixedUpdate(Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (!_isInitialized) return;

            foreach (var update in _updates)
                update.DoUpdate(Time.deltaTime);
        }
    }
}