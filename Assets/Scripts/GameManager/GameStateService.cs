using ReactiveExtension;
using UnityEngine;


namespace GameManager
{
    public sealed class GameStateService : IStartNewGameListener, IGameOverListener, ILevelStartListener, ILevelWonListener, IPauseListener
    {
        public readonly Reactive<bool> IsPaused = new Reactive<bool>();
        public readonly Reactive<bool> GameStarted = new Reactive<bool>();
        public readonly Reactive<bool> IsLevelWon = new Reactive<bool>();
        public readonly Reactive<bool> IsLevelStarted = new Reactive<bool>();


        public void StartNewGame()
        {
            GameStarted.Value = true;
            IsLevelWon.Value = false;
            IsLevelStarted.Value = false;

            GamePaused(false);
        }

        public void GameOver()
        {
            GameStarted.Value = false;
            IsLevelWon.Value = false;
            IsLevelStarted.Value = false;

        }

        public void LevelStarted(int levelIndex)
        {
            IsLevelWon.Value = false;
            IsLevelStarted.Value = true;

        }

        public void LevelWon(int levelIndex)
        {
            IsLevelWon.Value = true;
            IsLevelStarted.Value = false;

        }

        public void GamePaused(bool isPaused)
        {
            Time.timeScale = isPaused ? 0f : 1f;
            IsPaused.Value = isPaused;
        }
    }
}