using UnityEngine;

namespace Components
{
    public sealed class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;

        public void AddForceAxially(float deltaForce)
        {
            _rigidbody2D.AddForce(transform.up * deltaForce);
        }

        public void AddSideForce(float rotationDelta)
        {
            transform.Rotate(new Vector3(0,0,1), rotationDelta);
        }

        public void SetVelocity(Vector2 velocity)
        {
            _rigidbody2D.velocity = velocity;
        }

    }
}