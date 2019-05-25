using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.View
{
    public class MenuButtonView : BaseView, IButton
    {
        //если на объект кнопки нет ссылки (чтобы избежать проверки на null)
        private bool _isObjectNull = true;
        private Button _button;

        public override void Init()
        {
            _button = gameObject.GetComponent<Button>();
            _isObjectNull = _button == null ? true : false; 
        }

        public void SetAction(UnityAction action)
        {
            if (_isObjectNull)
                return;

            _button.onClick.AddListener(action);
        }
    }
}