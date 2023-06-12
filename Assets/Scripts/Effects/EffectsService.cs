using UnityEngine;

namespace Effects
{
    public class EffectsService : ILevelStartListener
    {
        private readonly HitEffectPool _hitEffectPool;
        private readonly ExplosionEffectPool _explosionEffectPool;
        private readonly AoeExplosionEffectPool _aoeExplosionEffectPool;

        public EffectsService(HitEffectPool hitEffectPool, ExplosionEffectPool explosionEffectPool, AoeExplosionEffectPool aoeExplosionEffectPool)
        {
            _aoeExplosionEffectPool = aoeExplosionEffectPool;
            _hitEffectPool = hitEffectPool;
            _explosionEffectPool = explosionEffectPool;
        }
        public void ShowAoeEffect(Vector2 point, float radius)
        {
            var effectInstance = _aoeExplosionEffectPool.Spawn();
            effectInstance.transform.localScale = Vector3.one * radius;
            effectInstance.transform.position = point;
        }
        public void ShowHitEffect(Vector2 point, Vector2 normal, float scale = 1f)
        {
            var effectInstance = _hitEffectPool.Spawn();
            effectInstance.transform.position = point;
            effectInstance.SetScale(scale);
            effectInstance.transform.LookAt(point + normal);
        }
        public void ShowHitEffect(Collision2D collision, float scale = 1f)
        {
            var midPoint = Vector2.zero;
            var midNormal = Vector2.zero;
            foreach (var contactPoint2D in collision.contacts)
            {
                midPoint += contactPoint2D.point;
                midNormal += contactPoint2D.normal;
            }

            midNormal /= collision.contacts.Length;
            midPoint /= collision.contacts.Length;
            ShowHitEffect(midPoint, midNormal, scale);
        }
        public void ShowExplosionEffect(Vector2 point)
        {
            var effectInstance = _explosionEffectPool.Spawn();
            effectInstance.transform.position = point;
            effectInstance.transform.LookAt(point);
        }

        public void LevelStarted(int levelIndex)
        {
            _hitEffectPool.Clear();
            _explosionEffectPool.Clear();
        }

    }
}