using System;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Obstacles
{
    public class Obstacle : MonoBehaviour, IDamage
    {
        private Vector3 _initialScale;
        private int _division;
        public event Action<Obstacle, Collision2D> OnCollisionEntered;
        public event Action<Obstacle, int, Vector2> OnWeaponHit;
        public int CurrentDivision
        {
            get => _division;
            set
            {
                _division = value;
                transform.localScale = _initialScale * Mathf.Pow(0.8f, _division);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEntered?.Invoke(this, collision);
        }

        public void TakeDamage(int damage, Vector2 dir)
        {
            OnWeaponHit?.Invoke(this, damage, dir);
        }

        private void Awake()
        {
            _initialScale = transform.localScale;
            foreach (Transform o in transform)
            {
                o.gameObject.SetActive(Random.value > 0.5f);
                o.localPosition = UnityEngine.Random.insideUnitCircle * 0.07f;
            }
        }

        public void DoReset()
        {
            CurrentDivision = 0;
            transform.localScale = _initialScale;
            var rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}