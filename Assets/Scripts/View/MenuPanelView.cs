using System.Collections.Generic;
using UnityEngine;

namespace Core.View
{
    public class MenuPanelView : BaseView, IPanel
    {
        [SerializeField]
        private float _paddingTop = 0.1f;
        [SerializeField]
        private float _paddingBottom = 0.1f;
        [SerializeField]
        private float _paddingLeft = 0.1f;
        [SerializeField]
        private float _paddingRight = 0.1f;

        public float PaddingTop => _paddingTop;
        public float PaddingBottom => _paddingBottom;
        public float PaddingLeft => _paddingLeft;
        public float PaddingRight => _paddingRight;
        public float Width => _panelWidth;
        public float Height => _panelHeight;

        //если на объект кнопки нет ссылки (чтобы избежать проверки на null)
        private bool _isObjectNull = true;
        private BaseView[] _allChildViews;
        private List<IButton> _allChildButtons;
        private List<BaseView> _allChildButtonsAsBase;
        private float _panelWidth;
        private float _panelHeight;

        public IButton[] GetAllChildButtons()
        {
            return _allChildButtons.ToArray();
        }

        public BaseView[] GetAllChildButtonsAsBase()
        {
            return _allChildButtonsAsBase.ToArray();
        }

        public BaseView[] GetAllChilds()
        {
            return _allChildViews;
        }

        public override void Init()
        {
            _panelWidth = gameObject.GetComponent<RectTransform>().rect.width;
            _panelHeight = gameObject.GetComponent<RectTransform>().rect.height;

            _allChildViews = gameObject.GetComponentsInChildren<BaseView>();
            _allChildButtons = new List<IButton>();
            _allChildButtonsAsBase = new List<BaseView>();
            foreach (var view in _allChildViews)
            {
                if (view is IButton)
                {
                    _allChildButtons.Add(view as IButton);
                    _allChildButtonsAsBase.Add(view);
                }
            }
        }

        
    }
}