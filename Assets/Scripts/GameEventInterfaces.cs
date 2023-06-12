public interface IStartNewGameListener
{
    public void StartNewGame();
}
public interface IGameOverListener
{
    public void GameOver();
}
public interface IPauseListener
{
    public void GamePaused(bool isPaused);
}

public interface ILevelStartListener
{
    public void LevelStarted(int levelIndex);
}
public interface ILevelWonListener
{
    public void LevelWon(int levelIndex);
}
public interface IUpdateListener
{
    public void DoUpdate(float dt);
}
public interface IFixedUpdateListener
{
    public void DoFixedUpdate(float dt);
}
public interface IInit
{
    public void Init();
}
