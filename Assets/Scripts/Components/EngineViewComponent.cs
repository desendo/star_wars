using UnityEngine;

namespace Components
{
    public sealed class EngineViewComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _marchEngine;
        [SerializeField] private GameObject _breakEngine;
        [SerializeField] private GameObject _sideLeftEngine;
        [SerializeField] private GameObject _sideRightEngine;

        public void SetEngineState(float thrust, float rotation)
        {
            _marchEngine.SetActive(thrust > 0);
            _breakEngine.SetActive(thrust < 0);
            _sideLeftEngine.SetActive(rotation > 0);
            _sideRightEngine.SetActive(rotation < 0);
        }
    }

}