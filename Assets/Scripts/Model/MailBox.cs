using Core.Enum;
using UnityEngine;

namespace Core.Model
{
    public class MailBox : BaseModel, ISelectable
    {
        [SerializeField]
        private BoxColor _color = BoxColor.None;
        [SerializeField]
        private float _moveSpeed = 10;
        [SerializeField]
        private BoxState _currentState = BoxState.Idle;
        [Header("Спрайты коробочек")]
        [SerializeField]
        private Sprite _redBoxSprite;
        [SerializeField]
        private Sprite _blueBoxSprite;
        [SerializeField]
        private Sprite _greenBoxSprite;

        public delegate void OnFinishMethod();

        private GameAction _currentAction = GameAction.None;
        private bool _isSelected = false;
        private Rigidbody2D _rigidbody2d;
        private OnFinishMethod FinishMethod;
        private SpriteRenderer _spriteRenderer;

        public BoxColor Color
        {
            get => _color;
            set
            {
                switch (value)
                {
                    case BoxColor.Green:
                        _spriteRenderer.sprite = _greenBoxSprite;
                        break;
                    case BoxColor.Red:
                        _spriteRenderer.sprite = _redBoxSprite;
                        break;
                    case BoxColor.Blue:
                        _spriteRenderer.sprite = _blueBoxSprite;
                        break;
                }

                _color = value;
            }
        }

        private void Awake()
        {
            _rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Color = _color;
        }

        private void FixedUpdate()
        {
            switch (_currentState)
            {
                //состояние покоя
                case BoxState.Idle:
                    switch (_currentAction)
                    {
                        case GameAction.MoveLeft:
                            _rigidbody2d.velocity = Vector2.left * _moveSpeed;
                            _currentState = BoxState.Move;
                            break;
                        case GameAction.MoveRight:
                            _rigidbody2d.velocity = Vector2.right * _moveSpeed;
                            _currentState = BoxState.Move;
                            break;
                        case GameAction.MoveUp:
                            _rigidbody2d.velocity = Vector2.up * _moveSpeed;
                            _currentState = BoxState.Move;
                            break;
                        case GameAction.MoveDown:
                            _rigidbody2d.velocity = Vector2.down * _moveSpeed;
                            _currentState = BoxState.Move;
                            break;
                        case GameAction.Finished:
                            _currentState = BoxState.Finish;
                            FinishMethod?.Invoke();
                            break;
                    }
                    _currentAction = GameAction.None;
                    break;
                //перемещение
                case BoxState.Move:
                    if (_rigidbody2d.velocity.x == 0 && _rigidbody2d.velocity.y == 0)
                        FixPosition();
                    break;
            }
        }

        private void FixPosition()
        {
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
                                                Mathf.RoundToInt(transform.position.y),
                                                transform.position.z);
            _currentState = BoxState.Idle;
        }

        /// <summary>
        /// Выбрать объект
        /// </summary>
        public void Select()
        {
            _isSelected = true;
            _rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        /// <summary>
        /// Снять выделение объекта
        /// </summary>
        public void Unselect()
        {
            _isSelected = false;
            _rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public bool IsSelected()
        {
            return _isSelected;
        }

        /// <summary>
        /// Применить действие к объекту
        /// </summary>
        /// <param name="action">Действие</param>
        public void SetAction(GameAction action)
        {
            if (_currentState != BoxState.Idle)
                return;
            _currentAction = action;
        }

        public bool CanSelect()
        {
            return _currentState != BoxState.Finish;
        }

        public void SetFinished(OnFinishMethod finishMethod)
        {
            _currentAction = GameAction.Finished;
            FinishMethod = finishMethod;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            MailBox box = collision.gameObject.GetComponent<MailBox>();
            if (box != null && _currentState == BoxState.Move)
                _rigidbody2d.velocity = Vector2.zero;
        }
    }
}
