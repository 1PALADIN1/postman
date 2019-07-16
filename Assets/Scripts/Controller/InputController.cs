using Core.Enum;
using Core.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Controller
{
    public class InputController : BaseController, IOnUpdate
    {
        //TODO debug
        private KeyCode _testKey = KeyCode.P;

        //управление
        private KeyCode _moveLeft = KeyCode.A;
        private KeyCode _moveRight = KeyCode.D;
        private KeyCode _moveUp = KeyCode.W;
        private KeyCode _moveDown = KeyCode.S;
        private KeyCode _selectNext = KeyCode.N;
        //сохранение уровня
        private KeyCode _saveLevel = KeyCode.I;
        //загрузка уровня
        private KeyCode _loadLevel = KeyCode.O;

        private GameStore _gameStore;
        private SelectController _selectController;
        private BoxController _boxController;
        private MSController _msController;
        
        private Vector3 _startPos;
        private Vector3 _endPos;

        public override void Init()
        {
            _gameStore = GameStore.Store;
            _selectController = _gameStore.GetController<SelectController>();
            _boxController = _gameStore.GetController<BoxController>();
            _msController = _gameStore.GetController<MSController>();

            _startPos = new Vector3();
            _endPos = new Vector3();
        }

        public void OnUpdate()
        {
            if (!_gameStore.IsGamePaused)
            {
                //MobileInput();

                if (Application.platform == RuntimePlatform.Android
                    || Application.platform == RuntimePlatform.IPhonePlayer)
                    MobileInput();
                else
                    StandaloneInput();
            }
        }

        #region Обработка мобильного ввода
        /// <summary>
        /// Управление с мобильного устройства
        /// </summary>
        private void MobileInput()
        {
            if (Input.touchCount > 0)
            {
                switch (Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                        _startPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                        break;
                    case TouchPhase.Ended:
                        _endPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);

                        var moveDirection = MovedDirection(_startPos, _endPos);
                        var box = TouchBox(_startPos, _endPos, moveDirection);
                        
                        if (box != null)
                        {
                            if (_selectController.Select(box))
                            {
                                switch (moveDirection)
                                {
                                    case TouchDirection.DirectionLeft:
                                        _boxController.SetAction(GameAction.MoveLeft);
                                        break;
                                    case TouchDirection.DirectionRight:
                                        _boxController.SetAction(GameAction.MoveRight);
                                        break;
                                    case TouchDirection.DirectionUp:
                                        _boxController.SetAction(GameAction.MoveUp);
                                        break;
                                    case TouchDirection.DirectionDown:
                                        _boxController.SetAction(GameAction.MoveDown);
                                        break;
                                }
                            }
                        }
                        break;
                }
            }
        }

        private enum TouchDirection
        {
            None,
            DirectionLeft,
            DirectionRight,
            DirectionUp,
            DirectionDown
        }

        /// <summary>
        /// Определяет в каком направлении был свайп
        /// </summary>
        /// <returns>Возвращает направление</returns>
        private TouchDirection MovedDirection(Vector3 start, Vector3 end)
        {
            TouchDirection result = TouchDirection.None;

            float startX = start.x;
            float startY = start.y;
            float endX = end.x;
            float endY = end.y;

            if (Mathf.Abs(startX - endX) > Mathf.Abs(startY - endY))
            {
                //смещение по X
                if (startX > endX)
                    return TouchDirection.DirectionLeft;
                if (startX < endX)
                    return TouchDirection.DirectionRight;
            }
            else
            {
                //смещение по Y
                if (startY > endY)
                    return TouchDirection.DirectionDown;
                if (startY < endY)
                    return TouchDirection.DirectionUp;
            }

            return result;
        }

        private struct MinMaxPosition
        {
            public float MinX;
            public float MinY;
            public float MaxX;
            public float MaxY;

            public MinMaxPosition(float MinX, float MinY, float MaxX, float MaxY)
            {
                this.MinX = MinX;
                this.MinY = MinY;
                this.MaxX = MaxX;
                this.MaxY = MaxY;
            }
        }

        /// <summary>
        /// Коробка, которую можно двигать
        /// </summary>
        private ISelectable TouchBox(Vector3 startPostion, Vector3 endPosition, TouchDirection direction)
        {
            List<ISelectable> resultList = new List<ISelectable>();

            RaycastHit2D[] hitObjects = Physics2D.LinecastAll(startPostion, endPosition);

            foreach (var hitObject in hitObjects)
            {
                BaseModel model = hitObject.collider.GetComponent<BaseModel>();
                if (model != null)
                {
                    if (model is ISelectable)
                        if ((model as ISelectable).CanSelect())
                            resultList.Add(model as ISelectable);
                }
            }

            if (resultList.Count > 0)
            {
                int resultIndex = 0;
                BaseModel model = resultList[resultIndex] as BaseModel;
                MinMaxPosition minMaxPos = new MinMaxPosition(model.transform.position.x,
                                                            model.transform.position.y,
                                                            model.transform.position.x,
                                                            model.transform.position.y);

                for (int i = 1; i < resultList.Count; i++)
                {
                    BaseModel procModel = resultList[i] as BaseModel;

                    switch (direction)
                    {
                        case TouchDirection.DirectionLeft:
                            if (procModel.transform.position.x < minMaxPos.MinX)
                            {
                                minMaxPos.MinX = procModel.transform.position.x;
                                resultIndex = i;
                            }
                            break;
                        case TouchDirection.DirectionRight:
                            if (procModel.transform.position.x > minMaxPos.MaxX)
                            {
                                minMaxPos.MaxX = procModel.transform.position.x;
                                resultIndex = i;
                            }
                            break;
                        case TouchDirection.DirectionUp:
                            if (procModel.transform.position.y > minMaxPos.MaxY)
                            {
                                minMaxPos.MaxY = procModel.transform.position.y;
                                resultIndex = i;
                            }
                            break;
                        case TouchDirection.DirectionDown:
                            if (procModel.transform.position.x < minMaxPos.MinY)
                            {
                                minMaxPos.MinY = procModel.transform.position.y;
                                resultIndex = i;
                            }
                            break;
                    }
                }

                return resultList[resultIndex];
            }

            return null;
        }
        #endregion

        /// <summary>
        /// Управление с ПК
        /// </summary>
        private void StandaloneInput()
        {
            //управление коробкой
            if (Input.GetKeyDown(_moveLeft))
                _boxController.SetAction(GameAction.MoveLeft);

            if (Input.GetKeyDown(_moveRight))
                _boxController.SetAction(GameAction.MoveRight);

            if (Input.GetKeyDown(_moveUp))
                _boxController.SetAction(GameAction.MoveUp);

            if (Input.GetKeyDown(_moveDown))
                _boxController.SetAction(GameAction.MoveDown);

            if (Input.GetKeyDown(_selectNext))
                _selectController.SelectNext();

            //загрузка уровня
            if (Input.GetKeyDown(_loadLevel))
                _msController.LoadLevel(_gameStore.CurrentLevel, true);
            //сохранение уровня
            if (Input.GetKeyDown(_saveLevel))
                _msController.SaveLevel(_gameStore.CurrentLevel);
        }
    }
}