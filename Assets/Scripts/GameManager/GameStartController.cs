using System.Collections.Generic;
using Signals;

namespace GameManager
{
    public sealed class GameStartController
    {
        private readonly List<IStartNewGameListener> _startNewGameListeners;
        private readonly GameStateService _gameStateService;
        private readonly List<ILevelStartListener> _startLevelListeners;

        public GameStartController(SignalBusService signalBusService,
            List<IStartNewGameListener> startNewGameListeners,
            List<ILevelStartListener> startLevelListeners,
            GameStateService gameStateService)
        {
            _startLevelListeners = startLevelListeners;
            _startNewGameListeners = startNewGameListeners;
            _gameStateService = gameStateService;
            signalBusService.Subscribe<StartNewGameRequest>(HandleStartNewGameRequest);
        }

        private void HandleStartNewGameRequest()
        {
            foreach (var listener in _startNewGameListeners)
            {
                listener.StartNewGame();
            }
            foreach (var listener in _startLevelListeners)
            {
                listener.LevelStarted(0);
            }
        }
    }
}