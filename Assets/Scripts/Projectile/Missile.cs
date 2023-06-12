using System;
using UnityEngine;

namespace Projectile
{
    public sealed class Missile : MonoBehaviour, IProjectile, IRadiusDamage
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private float _radius;
        private float _thrust;
        public int Damage { get; set; }
        public LayerMask DamageLayerMask { get; set; }
        public Transform Transform => transform;

        public event Action<IProjectile, Collision2D> OnCollisionEntered;

        public void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEntered?.Invoke(this, collision);
        }

        private void FixedUpdate()
        {
            _rigidbody2D.AddForce(_thrust * transform.up);
        }

        public Vector2 GetDirection()
        {
            return _rigidbody2D.velocity.normalized;
        }

        public void SetVelocity(Vector2 velocity)
        {
            _rigidbody2D.velocity = velocity;
        }
        public void SetThrust(float thrust)
        {
            _thrust = thrust;
        }
        public void SetRadius(float radius)
        {
            _radius = radius;
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

        public float Radius => _radius;
    }
}