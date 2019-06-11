using Core.Enum;
using UnityEngine;

namespace Core.Model
{
    public class FinishPoint : BaseModel
    {
        [SerializeField]
        private BoxColor _color;
        [Header("Спрайты")]
        [SerializeField]
        private Sprite _redSprite;
        [SerializeField]
        private Sprite _blueSprite;
        [SerializeField]
        private Sprite _greenSprite;

        public delegate void BoxOnFinishMethod(MailBox box, FinishPoint finishPoint);

        private BoxOnFinishMethod FinishMethod;
        private SpriteRenderer _spriteRenderer;

        public BoxColor Color
        {
            get => _color;
            set
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                switch (value)
                {
                    case BoxColor.Green:
                        _spriteRenderer.sprite = _greenSprite;
                        break;
                    case BoxColor.Red:
                        _spriteRenderer.sprite = _redSprite;
                        break;
                    case BoxColor.Blue:
                        _spriteRenderer.sprite = _blueSprite;
                        break;
                }
                
                _color = value;
            }
        }

        private void Start()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Color = _color;
        }

        public void SetOnFinishMethod(BoxOnFinishMethod finishMethod)
        {
            FinishMethod = finishMethod;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            MailBox box = collision.gameObject.GetComponent<MailBox>();
            if (box != null)
            {
                FinishMethod?.Invoke(box, this);
            }
        }
    }
}