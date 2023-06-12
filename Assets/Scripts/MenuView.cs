using System;
using System.Collections.Generic;
using DependencyInjection;
using GameManager;
using Signals;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [SerializeField] private Button _startNewGameButton;
    [SerializeField] private Button _resumeGameButton;
    [SerializeField] private Button _startNextLevelButton;

    private GameStateService _gameStateService;

    private readonly List<IDisposable> _disposed = new List<IDisposable>();
    [Inject]
    public void Construct(SignalBusService signalBusService, GameStateService gameStateService)
    {
        _gameStateService = gameStateService;

        _startNewGameButton.onClick.AddListener(() => signalBusService.Fire(new StartNewGameRequest()));
        _resumeGameButton.onClick.AddListener(() => signalBusService.Fire(new TogglePauseRequest()));
        _startNextLevelButton.onClick.AddListener(() => signalBusService.Fire(new StartNextLevelRequest()));

        _disposed.Add(_gameStateService.GameStarted.Subscribe(UpdateGameStarted));
        _disposed.Add(_gameStateService.IsPaused.Subscribe(UpdateGamePaused));
        _disposed.Add(_gameStateService.IsLevelWon.Subscribe(UpdateLevelWon));
    }
    private void UpdateLevelWon(bool val)
    {
        _startNextLevelButton.gameObject.SetActive(val);
    }
    private void UpdateGamePaused(bool val)
    {
        _resumeGameButton.gameObject.SetActive(val);
        _startNewGameButton.gameObject.SetActive(val || !_gameStateService.GameStarted.Value);

    }

    private void UpdateGameStarted(bool val)
    {
        _startNewGameButton.gameObject.SetActive(!val || _gameStateService.IsPaused.Value);
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposed)
        {
            disposable.Dispose();
        }
    }
}