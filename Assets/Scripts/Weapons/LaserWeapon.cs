using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class LaserWeapon : WeaponBase
    {
        [SerializeField] private LineRenderer _beam;
        private IEnumerator _fireRoutine;
        public override string Id => "Laser";

        protected override void OnFire()
        {
            if (_fireRoutine != null)
                StopCoroutine(_fireRoutine);
            var hit = Physics2D.Raycast(_firePoint.position, _firePoint.up, 100, DamageLayerMask);
            if (hit.collider != null)
            {
                _fireRoutine = FireRoutine(hit.distance);
                StartCoroutine(_fireRoutine);
                hit.collider.GetComponent<IDamage>()?.TakeDamage(Damage, _firePoint.up);
            }
            else
            {
                _fireRoutine = FireRoutine(100f);
                StartCoroutine(_fireRoutine);
            }
        }

        private IEnumerator FireRoutine(float hitDistance)
        {
            var position = _firePoint.position;
            _beam.gameObject.SetActive(true);
            _beam.SetPositions(new []{position, _firePoint.up * hitDistance + position});
            yield return new WaitForSeconds(0.1f);

            _beam.gameObject.SetActive(false);
        }
    }
}