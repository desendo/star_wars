using System;
using UnityEngine;

namespace Projectile
{
    public sealed class Bullet : MonoBehaviour, IProjectile
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public int Damage { get; set; }
        public LayerMask DamageLayerMask { get; set; }
        public event Action<IProjectile, Collision2D> OnCollisionEntered;

        public Transform Transform => transform;
        public void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEntered?.Invoke(this, collision);
        }

        public Vector2 GetDirection()
        {
            return _rigidbody2D.velocity.normalized;
        }

        public void SetVelocity(Vector2 velocity)
        {
            _rigidbody2D.velocity = velocity;
        }

        public void SetPhysicsLayer(int physicsLayer)
        {
            gameObject.layer = physicsLayer;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}