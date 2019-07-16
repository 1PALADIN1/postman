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
        [SerializeField]
        protected bool _isEnabled = true;

        public UIMarker UIMarker => _marker;
        public bool IsActive => _isActive;
        public bool IsEnabled => _isEnabled;
        public bool IsFixedPosition => _isFixedPosition;

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

        public virtual void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public abstract void Init();
    }
}