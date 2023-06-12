using Signals;
using UnityEngine;

namespace Input
{
    public sealed class InputManager : ILevelStartListener, IGameOverListener, IUpdateListener, ILevelWonListener
    {
        private bool _levelStarted;
        private readonly SignalBusService _signalBus;

        public InputManager(SignalBusService signalBus)
        {
            _signalBus = signalBus;
        }

        public int RotateDirection { get; private set; }
        public int ThrustDirection { get; private set; }
        public bool Fire { get; private set; }
        public int SelectedWeaponSlot { get; private set; }

        public void DoUpdate(float dt)
        {
            if(!_levelStarted)
                return;

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                _signalBus.Fire(new TogglePauseRequest());

            var slot = 0;
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
                slot = 1;
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
                slot = 2;
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
                slot = 3;

            SelectedWeaponSlot = slot;
            Fire = UnityEngine.Input.GetKeyDown(KeyCode.LeftShift);

            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
                RotateDirection = 1;
            else if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
                RotateDirection = -1;
            else
                RotateDirection = 0;

            if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
                ThrustDirection = 1;
            else if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
                ThrustDirection = -1;
            else
                ThrustDirection = 0;

        }

        public void GameOver()
        {
            _levelStarted = false;
        }

        public void LevelStarted(int levelIndex)
        {
            _levelStarted = true;
        }

        public void LevelWon(int levelIndex)
        {
            _levelStarted = false;
        }
    }
}