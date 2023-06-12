using Components;
using Config;
using Input;
using Level;
using UnityEngine;

namespace Character
{
    public sealed class CharacterMoveController : ILevelStartListener, ILevelWonListener, IGameOverListener, IFixedUpdateListener
    {
        private readonly InputManager _inputManager;
        private readonly CharacterService _characterService;
        private readonly GameConfig _gameConfig;
        private readonly LevelBounds _levelBounds;

        private bool _moveEnabled;
        public CharacterMoveController(InputManager inputManager, CharacterService characterService, LevelBounds levelBounds, GameConfig gameConfig)
        {
            _levelBounds = levelBounds;
            _gameConfig = gameConfig;
            _inputManager = inputManager;
            _characterService = characterService;
        }

        public void LevelStarted(int levelIndex)
        {
            _moveEnabled = true;
        }

        public void LevelWon(int levelIndex)
        {
            _moveEnabled = false;
        }
        public void GameOver()
        {
            _moveEnabled = false;
        }

        public void DoFixedUpdate(float dt)
        {
            if(!_moveEnabled)
                return;

            var deltaForceMag = 0f;

            if(_inputManager.ThrustDirection > 0)
                deltaForceMag = _inputManager.ThrustDirection * dt * _gameConfig.CharMoveSpeed;
            if(_inputManager.ThrustDirection < 0)
                deltaForceMag = _inputManager.ThrustDirection * dt * _gameConfig.CharBreakSpeed;

            var rotationDelta = _inputManager.RotateDirection * dt * _gameConfig.CharRotationSpeed;

            var moveComponent = _characterService.Character.GetComponent<MoveComponent>();
            moveComponent.AddForceAxially(deltaForceMag);
            moveComponent.AddSideForce(rotationDelta);
            moveComponent.transform.position = _levelBounds.Warp(moveComponent.transform.position);

            var engineViewComponent = _characterService.Character.GetComponentInChildren<EngineViewComponent>();
            engineViewComponent.SetEngineState(_inputManager.ThrustDirection, _inputManager.RotateDirection);

        }


    }
}