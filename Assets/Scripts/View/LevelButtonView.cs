using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.View
{
    public class LevelButtonView : BaseView, IButton
    {
        private Button _levelButton;
        private Text _levelButtonText;
        private RectTransform _levelButtonRect;
        private int _levelNum = 1;

        /// <summary>
        /// Установка отображаемого на кнопке текста
        /// </summary>
        public string Text
        {
            set => _levelButtonText.text = value;
        }

        /// <summary>
        /// Установка загружаемого уровня
        /// </summary>
        public int Level
        {
            set => _levelNum = value;
        }

        public override void Init()
        {
            _levelButton = gameObject.GetComponent<Button>();
            _levelButtonText = _levelButton.GetComponentInChildren<Text>(true);
            _levelButtonRect = _levelButton.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Установка позиции кнопки
        /// </summary>
        /// <param name="posX">Координата по оси X</param>
        /// <param name="posY">Координата по оси Y</param>
        public void SetPosition(float posX, float posY)
        {
            _levelButtonRect.anchoredPosition = new Vector2(posX, posY);
        }

        public void SetAction(UnityAction action)
        {
            action = () =>
            {
                if (IsEnabled)
                {
                    PlayerPrefs.SetInt("CurrentLevel", _levelNum);
                    SceneManager.LoadScene("LevelScene");
                }
            };

            _levelButton.onClick.AddListener(action);
        }
    }
}