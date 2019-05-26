using Core.View;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Helper
{
    public class AdaptiveView
    {
        private float _scaleKoef;
        private Vector2 _tmpVector2;

        public AdaptiveView(float brilliantWidth, float currentWidth)
        {
            _scaleKoef = currentWidth / brilliantWidth;
            _tmpVector2 = new Vector2();
        }

        /// <summary>
        /// Пропорциональное масштабирование представления
        /// </summary>
        /// <param name="view">Объект типа BaseView</param>
        public void AdaptateSize(BaseView view)
        {
            if (view is IButton)
            {
                Button button = view.GetComponent<Button>();
                Text buttonText = button.GetComponentInChildren<Text>(true);
                RectTransform buttonRect = button.GetComponent<RectTransform>();

                buttonRect.sizeDelta = new Vector2(_scaleKoef * buttonRect.rect.width,
                                                        _scaleKoef * buttonRect.rect.height);
                buttonText.fontSize = Mathf.RoundToInt(buttonText.fontSize * _scaleKoef);
            }
        }

        /// <summary>
        /// Равномерное расположение объектов внутри панели
        /// </summary>
        /// <param name="views">Массив объектов на панеле</param>
        /// <param name="panel">Панель</param>
        /// <param name="adaptSize">Нужно ли адаптировать размер компонента</param>
        /// <param name="numInColumn">Количество элеменов в столбце</param>
        /// <param name="numInRow">Количество элементов в ряду</param>
        public void AdaptateToPanelPosition(BaseView[] views, IPanel panel, bool adaptSize = false, int numInColumn = 1, int numInRow = 1)
        {
            float panelWidth = panel.Width;
            float panelHeight = panel.Height;

            float insideButsX = panelWidth * (1 - (panel.PaddingLeft + panel.PaddingRight)) / numInColumn;
            float insideButsY = panelHeight * (1 - (panel.PaddingTop + panel.PaddingBottom)) / numInRow;

            int maxPageSize = numInRow * numInColumn;
            int j = 0;
            for (int i = 0; i < views.Length; i++)
            {
                if (i > maxPageSize - 1)
                    break;

                BaseView currentElement = views[i];
                RectTransform elementRectTransform = currentElement.GetComponent<RectTransform>();
                j = i / numInColumn;
                int i_pos = i % numInColumn;

                _tmpVector2.Set(panelWidth * panel.PaddingLeft + insideButsX * i_pos + insideButsX / 2,
                                -(panelHeight * panel.PaddingTop + insideButsY * j + insideButsY / 2));

                elementRectTransform.anchoredPosition = _tmpVector2;

                if (adaptSize)
                    AdaptateSize(currentElement);
            }
        }
    }
}
