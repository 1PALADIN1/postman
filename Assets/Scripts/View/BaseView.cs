using Core.Enum;
using UnityEngine;

namespace Core.View
{
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField]
        protected UIMarker _marker = UIMarker.None;

        protected bool _isActive;

        public UIMarker UIMarker
        {
            get => _marker;
        }

        public bool IsActive
        {
            get => _isActive;
        }

        private void Start()
        {
            _isActive = true;
            gameObject.SetActive(true);
        }

        public virtual void SetActive(bool active)
        {
            _isActive = active;
            gameObject.SetActive(active);
        }

        public abstract void Init();
    }
}