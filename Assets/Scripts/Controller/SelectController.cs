using Core.Model;

namespace Core.Controller
{
    public class SelectController : BaseController
    {
        private GameStore _gameStore;
        private ISelectable[] _selectObjects;
        private int _currentSelected;

        public ISelectable SelectedObject
        {
            get => _selectObjects[_currentSelected];
        }

        public override void Init()
        {
            _gameStore = GameStore.Store;
            _selectObjects = _gameStore.SelectObjects;
            _currentSelected = 0;
            SelectCurrent();
        }

        /// <summary>
        /// Выбор следующего объекта из списка
        /// </summary>
        /// <returns>Возвращает false, если не может выбрать элемент</returns>
        public bool SelectNext()
        {
            //TODO проверка на то, что текущий ящик не перемещается
            //...

            int tmpSelect = _currentSelected;

            //ищем посылку, которую можем выбрать
            do
            {
                tmpSelect++;
                if (tmpSelect > _selectObjects.Length - 1)
                    tmpSelect = 0;

                if (_selectObjects[tmpSelect].CanSelect())
                {
                    _currentSelected = tmpSelect;
                    tmpSelect = -1;
                    break;
                }
            }
            while (tmpSelect != _currentSelected);

            //не осталось коробочек для выбора
            if (tmpSelect == _currentSelected)
                return false;
            
            SelectCurrent();

            return true;
        }

        /// <summary>
        /// Выбирает текущий объект (с остальных снимает выделение)
        /// </summary>
        private void SelectCurrent()
        {
            for (int i = 0; i < _selectObjects.Length; i++)
            {
                if (_currentSelected == i) _selectObjects[i].Select();
                else
                    _selectObjects[i].Unselect();
            }
        }

        /// <summary>
        /// Выбирает объект, если он есть в списке
        /// </summary>
        /// <param name="selectObject">Объект, который нужно выбрать</param>
        /// <returns>Возваращает true, если объект успешно выбран, иначе - false</returns>
        public bool Select(ISelectable selectObject)
        {
            for (int i = 0; i < _selectObjects.Length; i++)
            {
                if (_selectObjects[i] == selectObject)
                {
                    _currentSelected = i;
                    SelectCurrent();
                    return true;
                }
            }

            return false;
        }
    }
}
