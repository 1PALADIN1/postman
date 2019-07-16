using Core.Helper.Util;
using Core.Model;
using Core.View;
using UnityEngine;

namespace Core.Controller
{
    public class CollectController : BaseController
    {
        private ICollectable[] _collectObjects;
        private GameStore _gameStore;
        private OptionalObject<LevelCurrentStarsImageView> _currentStarsImageView;
        //количество собранных звёзд
        private int _currentStars = 0;
        //максимальное количество звёзд, которое можно собрать
        private int _maxStars = 0;

        /// <summary>
        /// Максимальное количество звёзд, которое можно собрать
        /// </summary>
        public int MaxStars
        {
            get => _maxStars;
        }

        /// <summary>
        /// Количество собранных звёзд
        /// </summary>
        public int CollectedStars
        {
            get => _currentStars;
        }

        public override void Init()
        {
            _gameStore = GameStore.Store;
            _collectObjects = _gameStore.CollectObjects;
            _currentStarsImageView = new OptionalObject<LevelCurrentStarsImageView>(_gameStore.CurrentStarsImageView);

            foreach (var collectObj in _collectObjects)
                collectObj.SetCollect(OnCollect);

            _maxStars = _collectObjects.Length;
        }

        private void OnCollect(MailBox box, Star star)
        {
            //TODO debug
            Debug.Log($"Собрана звёздочка {star.name} коробочкой {box.name}");

            _currentStars++;
            if (!_currentStarsImageView.HasValue) _currentStarsImageView.SetValue(_gameStore.CurrentStarsImageView);
            _currentStarsImageView.Value.SetStarNumber(_currentStars);
        }
    }
}