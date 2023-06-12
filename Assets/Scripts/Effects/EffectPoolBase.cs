using Pool;

namespace Effects
{
    public class EffectPoolBase : PoolBase<Effect>
    {
        public override Effect Spawn()
        {
            var effect = base.Spawn();
            effect.Setup(this);
            return effect;
        }
    }
}