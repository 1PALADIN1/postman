using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.View
{
    public class LevelButtonView : BaseView, IButton
    {
        //UI
        [Header("Изображения")]
        [SerializeField]
        private Sprite _noStarsSprite;
        [SerializeField]
        private Sprite _oneStarSprite;
        [SerializeField]
        private Sprite _twoStarsSprite;
        [SerializeField]
        private Sprite _threeStarsSprite;
        [SerializeField]
        private Sprite _disableSprite;

        private Button _levelButton;
        private Text _levelButtonText;
        private Image _levelButtonImage;
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
            _levelButtonImage = _levelButton.GetComponent<Image>();
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
        
        /// <summary>
        /// Установка спрайта для кнопки
        /// </summary>
        /// <param name="starsNum">Количество звёзд</param>
        public void SetStarSprite(int starsNum)
        {
            if (IsEnabled)
            {
                switch (starsNum)
                {
                    case 1:
                        _levelButtonImage.sprite = _oneStarSprite;
                        break;
                    case 2:
                        _levelButtonImage.sprite = _twoStarsSprite;
                        break;
                    case 3:
                        _levelButtonImage.sprite = _threeStarsSprite;
                        break;
                    default:
                        _levelButtonImage.sprite = _noStarsSprite;
                        break;
                }
            }
            else
                _levelButtonImage.sprite = _disableSprite;
        }
    }
}