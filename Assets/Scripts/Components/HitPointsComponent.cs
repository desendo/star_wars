using System;
using ReactiveExtension;
using UnityEngine;
using Weapons;

namespace Components
{
    public sealed class HitPointsComponent : MonoBehaviour, IDamage
    {
        public event Action<GameObject> HpEmpty;
        public readonly Reactive<int> HpLeft = new Reactive<int>();

        public bool IsHitPointsExists()
        {
            return HpLeft.Value > 0;
        }

        public void TakeDamage(int damage, Vector2 dir)
        {
            if (HpLeft.Value > 0)
            {
                HpLeft.Value -= damage;
                if (HpLeft.Value <= 0)
                    HpEmpty?.Invoke(gameObject);
            }
        }

        public void SetHitPoints(int current)
        {
            HpLeft.Value = current;
        }
    }
}