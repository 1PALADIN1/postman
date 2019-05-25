using Core.Enum;
using Core.Model;
using UnityEngine;

namespace Core.Controller
{
    public class FinishController : BaseController
    {
        private GameStore _gameStore;
        private FinishPoint[] _finishPoints;
        private SelectController _selectController;

        public override void Init()
        {
            _gameStore = GameStore.Store;
            _finishPoints = _gameStore.FinishPoints;
            _selectController = _gameStore.GetController<SelectController>();

            foreach (var point in _finishPoints)
                point.SetOnFinishMethod(OnFinish);
        }

        private void OnFinish(MailBox box, FinishPoint point)
        {
            //TODO debug
            Debug.Log($"На финише! Box color = {box.Color}, point color = {point.Color}");

            if (box.Color == point.Color)
            {
                //TODO debug
                Debug.Log("Совпало!!");

                box.SetFinished(OnBoxFinish);
            }
        }

        /// <summary>
        /// Метод вызывается, когда коробочка перестала перемещаться и заняла своё место
        /// </summary>
        private void OnBoxFinish()
        {
            //TODO debug
            Debug.Log("Коробка на месте");
            if (!_selectController.SelectNext())
                _gameStore.FinishLevel();
        }
    }
}
