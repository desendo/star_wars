using System;
using UnityEngine;

namespace Projectile
{
    public interface IRadiusDamage
    {
        float Radius { get; }
    }

    public interface IProjectile
    {
        LayerMask DamageLayerMask { get; set; }
        int Damage { get; set; }
        Transform Transform { get;}
        event Action<IProjectile, Collision2D> OnCollisionEntered;
        void OnCollisionEnter2D(Collision2D collision);
        Vector2 GetDirection();
        void SetVelocity(Vector2 velocity);
        void SetPhysicsLayer(int physicsLayer);
        void SetPosition(Vector3 position);
        void SetColor(Color color);
    }
}