using Common;
using Components;
using Config;
using UnityEngine;
using Weapons;

namespace Character
{
    public sealed class CharacterInstaller : IStartNewGameListener
    {
        private readonly CharacterService _characterService;
        private readonly GameConfig _gameConfig;

        public CharacterInstaller(CharacterService characterService, GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _characterService = characterService;
        }

        public void StartNewGame()
        {
            _characterService.Character.SetActive(true);
            _characterService.Character.GetComponent<HitPointsComponent>()
                .SetHitPoints(_gameConfig.CharHealth);

            var weapons = _characterService.Character.GetComponentsInChildren<IWeapon>();

            int weaponHitMask = new LayerMask();
            weaponHitMask |= (1 << (int) PhysicsLayer.Obstacle) | (1 << (int) PhysicsLayer.Enemy);

            foreach (var weapon in weapons)
            {
                weapon.Setup(weaponHitMask, _gameConfig.CharDamage);
            }
        }
    }
}