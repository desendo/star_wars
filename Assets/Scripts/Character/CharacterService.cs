using System.Collections.Generic;
using System.Linq;
using ReactiveExtension;
using UnityEngine;
using Weapons;

namespace Character
{
    public class CharacterService : IStartNewGameListener, IUpdateListener, ILevelStartListener, ILevelWonListener, IGameOverListener
    {
        public IReadonlyReactive<IWeapon> CurrentWeapon => _currentWeapon;
        public GameObject Character { get; private set; }
        public List<IWeapon> Weapons { get; private set; }

        private Vector3 _initialPosition;
        private readonly Reactive<IWeapon> _currentWeapon = new Reactive<IWeapon>();
        private Quaternion _initialRotation;
        private bool _levelStarted;

        public void SetCharacter(GameObject character)
        {
            Character = character;
            Weapons = character.GetComponentsInChildren<IWeapon>().ToList();
            _initialPosition = Character.transform.position;
            _initialRotation = Character.transform.localRotation;
        }

        public void StartNewGame()
        {
            ResetCharacter();
            SelectWeapon(Weapons.FirstOrDefault());
        }

        private void ResetCharacter()
        {
            Character.gameObject.SetActive(true);
            Character.transform.position = _initialPosition;
            Character.transform.localRotation = _initialRotation;
            var rb = Character.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        public void SelectWeapon(IWeapon weapon)
        {
            _currentWeapon.Value = weapon;
        }

        public void DoUpdate(float dt)
        {
            if(!_levelStarted)
                return;

            if(Weapons != null)
                foreach (var weapon in Weapons)
                {
                    weapon.DoUpdate(dt);
                }
        }

        public void LevelStarted(int levelIndex)
        {
            _levelStarted = true;
        }

        public void LevelWon(int levelIndex)
        {
            _levelStarted = false;
        }

        public void GameOver()
        {
            _levelStarted = false;
        }
    }
}