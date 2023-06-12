using System;
using System.Collections.Generic;
using Character;
using Components;
using DependencyInjection;
using GameManager;
using Level;
using Signals;
using TMPro;
using UnityEngine;

public class HudView : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _life;
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private List<WeaponUIView> _weaponUIViews;
    private CharacterService _characterService;
    private GameStateService _gameStateService;
    private IDisposable _score1Sub;
    private IDisposable _score2Sub;
    private IDisposable _gameStartSub;
    private IDisposable _hpLeftSubSub;
    private SignalBusService _signalBusService;
    private LevelService _levelService;

    [Inject]
    public void Construct(CharacterService characterService, GameStateService gameStateService,
        SignalBusService signalBusService, LevelService levelService)
    {
        _levelService = levelService;
        _signalBusService = signalBusService;
        _characterService = characterService;
        _gameStateService = gameStateService;
        _levelService.LevelIndex.Subscribe(index => _levelNumber.text = $"Level: {index + 1}");
        _score1Sub = _levelService.Score.Subscribe(score =>
        {
            _score.text = $"SCORE: {_levelService.Score.Value} / {_levelService.TargetScore.Value}";
        });
        _score2Sub = _levelService.TargetScore.Subscribe(score =>
        {
            _score.text = $"SCORE: {_levelService.Score.Value} / {_levelService.TargetScore.Value}";
        });

        _gameStartSub = _gameStateService.GameStarted.Subscribe(isGameStarted =>
        {
            if (isGameStarted)
                _hpLeftSubSub = _characterService.Character.GetComponent<HitPointsComponent>().HpLeft
                    .Subscribe(x => _life.text = $"HP:{x}");
            else
                _hpLeftSubSub?.Dispose();
        });

        SetupWeaponUI();
    }

    private void SetupWeaponUI()
    {
        foreach (var weaponUIView in _weaponUIViews)
        {
            weaponUIView.gameObject.SetActive(false);
        }

        for (var i = 0; i < _characterService.Weapons.Count; i++)
        {
            var weapon = _characterService.Weapons[i];
            _weaponUIViews[i].gameObject.SetActive(true);
            _weaponUIViews[i].Setup(() => _signalBusService.Fire(new SwitchWeaponByIdRequest(weapon.Id)),
                weapon.Id, (i + 1).ToString(), weapon.ProgressNormalized);
        }

        _characterService.CurrentWeapon.Subscribe(weapon =>
        {
            foreach (var weaponUIView in _weaponUIViews)
            {
                weaponUIView.SetSelected(weapon?.Id == weaponUIView.Id);
            }
        });
    }

    private void OnDestroy()
    {
        _gameStartSub?.Dispose();
        _score1Sub?.Dispose();
        _score2Sub?.Dispose();
        _hpLeftSubSub?.Dispose();
    }


}