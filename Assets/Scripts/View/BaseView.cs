using Core.Enum;
using UnityEngine;

namespace Core.View
{
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField]
        protected UIMarker _marker = UIMarker.None;
        [SerializeField]
        protected bool _isFixedPosition = false;

        protected bool _isActive;

        public UIMarker UIMarker
        {
            get => _marker;
        }

        public bool IsActive
        {
            get => _isActive;
        }

        public bool IsFixedPosition
        {
            get => _isFixedPosition;
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