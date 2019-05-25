using Core.Enum;
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

        public override void Init()
        {
            _gameStore = GameStore.Store;
            _selectController = _gameStore.GetController<SelectController>();
            _boxController = _gameStore.GetController<BoxController>();
            _msController = _gameStore.GetController<MSController>();
        }

        public void OnUpdate()
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
                _msController.LoadLevel(_gameStore.CurrentLevel);
            //сохранение уровня
            if (Input.GetKeyDown(_saveLevel))
                _msController.SaveLevel(_gameStore.CurrentLevel);
        }
    }
}
