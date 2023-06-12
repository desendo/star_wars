using System.Collections.Generic;
using Signals;

namespace GameManager
{
    public sealed class GamePauseController
    {
        private readonly List<IPauseListener> _pauseListeners;
        private readonly GameStateService _gameStateService;

        public GamePauseController(SignalBusService signalBusService,
            List<IPauseListener> pauseListeners,
            GameStateService gameStateService)
        {
            _pauseListeners = pauseListeners;
            _gameStateService = gameStateService;
            signalBusService.Subscribe<TogglePauseRequest>(HandlePauseRequest);

        }

        private void HandlePauseRequest()
        {
            if(!_gameStateService.GameStarted.Value)
                return;

            foreach (var listener in _pauseListeners)
            {
                listener.GamePaused(!_gameStateService.IsPaused.Value);
            }
        }
    }
}