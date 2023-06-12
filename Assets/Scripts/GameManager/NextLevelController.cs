using System.Collections.Generic;
using Level;
using Signals;

namespace GameManager
{
    public sealed class NextLevelController
    {

        private readonly LevelService _levelService;
        private readonly List<ILevelStartListener> _levelStartListeners;
        private readonly SignalBusService _signalBusService;
        private readonly GameStateService _gameStateService;

        public NextLevelController(LevelService levelService, List<ILevelStartListener> levelStartListeners, GameStateService gameStateService,
            SignalBusService signalBusService)
        {
            _gameStateService = gameStateService;
            _signalBusService = signalBusService;
            _levelStartListeners = levelStartListeners;
            _levelService = levelService;

            _signalBusService.Subscribe<StartNextLevelRequest>(NextLevel);
        }

        private void NextLevel()
        {
            if(_gameStateService.GameStarted.Value && _gameStateService.IsLevelWon.Value)
                foreach (var levelWonListener in _levelStartListeners)
                {
                    levelWonListener.LevelStarted(_levelService.LevelIndex.Value + 1);
                }
        }

    }
}