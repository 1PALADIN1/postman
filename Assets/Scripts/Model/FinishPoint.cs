using Core.Enum;
using UnityEngine;

namespace Core.Model
{
    public class FinishPoint : BaseModel
    {
        [SerializeField]
        private BoxColor _color;

        public delegate void BoxOnFinishMethod(MailBox box, FinishPoint finishPoint);

        private BoxOnFinishMethod FinishMethod;

        public BoxColor Color
        {
            get => _color;
            set
            {
                var renderColor = UnityEngine.Color.white;
                switch (value)
                {
                    case BoxColor.Green:
                        renderColor = UnityEngine.Color.green;
                        break;
                    case BoxColor.Red:
                        renderColor = UnityEngine.Color.red;
                        break;
                    case BoxColor.Blue:
                        renderColor = UnityEngine.Color.blue;
                        break;
                }

                gameObject.GetComponent<SpriteRenderer>().color = renderColor;
                _color = value;
            }
        }

        private void Start()
        {
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