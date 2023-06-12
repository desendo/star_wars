using ReactiveExtension;

namespace Level
{
    public class LevelService: IStartNewGameListener, ILevelStartListener
    {
        public IReadonlyReactive<int> Score => _score;
        public IReadonlyReactive<int> TargetScore => _targetScore;
        public IReadonlyReactive<int> LevelIndex => _levelIndex;

        private readonly Reactive<int> _levelIndex = new Reactive<int>();
        private readonly Reactive<int> _score = new Reactive<int>();
        private readonly Reactive<int> _targetScore = new Reactive<int>();

        public void StartNewGame()
        {
            _levelIndex.Value = 0;

        }

        public void LevelStarted(int levelIndex)
        {
            _levelIndex.Value = levelIndex;
            _score.Value = 0;
            _targetScore.Value =  (levelIndex * 2) + 10;
        }


        public void AddScore(int scoreAdd)
        {
            _score.Value += scoreAdd;
        }
    }
}