using Core.Helper.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Core.View
{
    public class FinishStarsImageView : BaseView
    {
        [Header("Спрайты")]
        [SerializeField]
        private Sprite _noStarsSprite;
        [SerializeField]
        private Sprite _oneStarSprite;
        [SerializeField]
        private Sprite _twoStarsSprite;
        [SerializeField]
        private Sprite _threeStarsSprite;

        private Image _imageStars;

        private OptionalObject<Sprite> _noStarsSpriteOptional;
        private OptionalObject<Sprite> _oneStarSpriteOptional;
        private OptionalObject<Sprite> _twoStarsSpriteOptional;
        private OptionalObject<Sprite> _threeStarsSpriteOptional;
        private OptionalObject<Image> _imageStarsOptional;

        public override void Init()
        {
            _noStarsSpriteOptional = new OptionalObject<Sprite>(_noStarsSprite);
            _oneStarSpriteOptional = new OptionalObject<Sprite>(_oneStarSprite);
            _twoStarsSpriteOptional = new OptionalObject<Sprite>(_twoStarsSprite);
            _threeStarsSpriteOptional = new OptionalObject<Sprite>(_threeStarsSprite);

            _imageStars = GetComponent<Image>();
            _imageStarsOptional = new OptionalObject<Image>(_imageStars);
        }

        /// <summary>
        /// Установка количества звёзд для изображения
        /// </summary>
        /// <param name="starNum">Количество звёзд</param>
        public void SetStarNumber(int starNum)
        {
            if (_imageStarsOptional.HasValue)
            {
                switch (starNum)
                {
                    case 0:
                        if (_noStarsSpriteOptional.HasValue)
                            _imageStarsOptional.Value.sprite = _noStarsSpriteOptional.Value;
                        break;
                    case 1:
                        if (_oneStarSpriteOptional.HasValue)
                            _imageStarsOptional.Value.sprite = _oneStarSpriteOptional.Value;
                        break;
                    case 2:
                        if (_twoStarsSpriteOptional.HasValue)
                            _imageStarsOptional.Value.sprite = _twoStarsSpriteOptional.Value;
                        break;
                    case 3:
                        if (_threeStarsSpriteOptional.HasValue)
                            _imageStarsOptional.Value.sprite = _threeStarsSpriteOptional.Value;
                        break;
                }
            }
        }
    }
}
