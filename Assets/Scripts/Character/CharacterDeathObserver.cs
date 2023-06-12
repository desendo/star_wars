using System.Collections.Generic;
using Components;
using Effects;
using GameManager;
using UnityEngine;

namespace Character
{
    public sealed class CharacterDeathObserver : IStartNewGameListener, IGameOverListener
    {
        private readonly CharacterService _characterService;
        private readonly EffectsService _effectsService;
        private readonly GameStateService _gameStateService;
        private readonly List<IGameOverListener> _gameOverListeners;

        public CharacterDeathObserver(CharacterService characterService, EffectsService effectsService,
            List<IGameOverListener> gameOverListeners, GameStateService gameStateService)
        {
            _gameOverListeners = gameOverListeners;
            _characterService = characterService;
            _effectsService = effectsService;
            _gameStateService = gameStateService;
        }

        public void StartNewGame()
        {
            _characterService.Character.GetComponent<HitPointsComponent>().HpEmpty += OnCharacterDeath;
        }

        public void GameOver()
        {
            _characterService.Character.GetComponent<HitPointsComponent>().HpEmpty -= OnCharacterDeath;
        }

        private void OnCharacterDeath(GameObject x)
        {
            _effectsService.ShowExplosionEffect(x.transform.position);
            x.SetActive(false);
            foreach (var gameOverListener in _gameOverListeners)
                gameOverListener.GameOver();
        }
    }
}