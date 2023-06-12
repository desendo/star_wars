using System.Collections.Generic;
using Level;

namespace GameManager
{
    public sealed class LevelFinishController
    {

        private readonly LevelService _levelService;
        private readonly List<ILevelWonListener> _levelWonListeners;
        private GameStateService _gameStateService;

        public LevelFinishController(LevelService levelService, List<ILevelWonListener> levelWonListeners, GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
            _levelService = levelService;
            _levelWonListeners = levelWonListeners;
            _levelService.TargetScore.Subscribe(i => CheckScore());
            _levelService.Score.Subscribe(i => CheckScore());
        }

        private void CheckScore()
        {
            if (_levelService.TargetScore.Value > 0 && _levelService.Score.Value >= _levelService.TargetScore.Value && _gameStateService.GameStarted.Value)
            {
                foreach (var levelWonListener in _levelWonListeners)
                {
                    levelWonListener.LevelWon(_levelService.LevelIndex.Value);
                }
            }
        }

    }
}