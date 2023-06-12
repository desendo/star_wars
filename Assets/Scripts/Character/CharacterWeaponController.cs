using System.Linq;
using GameManager;
using Input;
using Signals;

namespace Character
{
    public sealed class CharacterWeaponController : IStartNewGameListener, IGameOverListener, IUpdateListener
    {
        private readonly CharacterService _characterService;
        private readonly InputManager _inputManager;
        private bool _started;

        public CharacterWeaponController(InputManager inputManager, CharacterService characterService,
            SignalBusService signalBusService, GameStateService gameStateService)
        {
            _inputManager = inputManager;
            _characterService = characterService;
            signalBusService.Subscribe<SwitchWeaponByIdRequest>(HandleSwitchRequest);
        }

        private void HandleSwitchRequest(SwitchWeaponByIdRequest obj)
        {
            if(!_started)
                return;

            var weapon = _characterService.Weapons.FirstOrDefault(x => x.Id == obj.WeaponId);
            if(weapon != null)
                _characterService.SelectWeapon(weapon);

        }

        public void DoUpdate(float dt)
        {
            if (!_started)
                return;

            if (_inputManager.Fire)
                OnFire();

            if (_inputManager.SelectedWeaponSlot > 0)
                SelectWeapon(_inputManager.SelectedWeaponSlot);
        }

        private void SelectWeapon(int slot)
        {
            var selected = _characterService.Weapons[slot - 1];
            _characterService.SelectWeapon(selected);
        }

        private void OnFire()
        {
            var weapon = _characterService.CurrentWeapon.Value;
            weapon?.TryFire();
        }

        public void GameOver()
        {
            _started = false;
        }

        public void StartNewGame()
        {
            _started = true;
        }
    }
}