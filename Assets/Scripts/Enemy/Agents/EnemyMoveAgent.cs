using Components;
using UnityEngine;

namespace Enemy.Agents
{
    public sealed class EnemyMoveAgent : MonoBehaviour
    {
        [SerializeField] private MoveComponent _moveComponent;
        public bool IsReached { get; private set; }
        private Vector2 _destination;
        private float _speed;

        private void FixedUpdate()
        {
            if (IsReached) return;

            var vector = _destination - (Vector2) transform.position;
            if (vector.magnitude <= 0.25f)
            {
                IsReached = true;
                return;
            }

            var velocity = vector.normalized * (Time.fixedDeltaTime * _speed);
            _moveComponent.SetVelocity(velocity);
        }

        public void SetDestination(Vector2 endPoint)
        {
            _destination = endPoint;
            IsReached = false;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

    }
}