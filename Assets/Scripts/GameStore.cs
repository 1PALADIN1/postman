using Core.Controller;
using Core.Enum;
using Core.Helper;
using Core.Model;
using Core.View;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameStore : MonoBehaviour
    {
        [Header("Префабы моделей")]
        [SerializeField]
        private GameObject _boxPrefab;
        [SerializeField]
        private GameObject _wallPrefab;
        [SerializeField]
        private GameObject _starPrefab;
        [SerializeField]
        private GameObject _finishPointPrefab;
        [Header("Настройки уровня")]
        [SerializeField]
        private LevelMode _levelMode = LevelMode.Game;
        [SerializeField]
        private int _currentLevel = 1;

        //модели
        //Объекты, которые можно выделять
        private ISelectable[] _selectObjects;
        private FinishPoint[] _finishPoints;
        private ICollectable[] _collectObjects;
        private BaseModel[] _allModels;

        //контроллеры
        private IOnUpdate[] _onUpdateController;
        private BaseController[] _allControllers;

        private InputController _inputController;
        private SelectController _selectController;
        private BoxController _boxController;
        private FinishController _finishController;
        private CollectController _collectController;
        private MSController _msController;

        //представления
        private LevelFinishView _levelFinishView;
        private BaseView _nextLevelButton;

        private static GameStore _instance;

        private LevelChecker _levelChecker;

        //TODO debug
        private float _fps = 0f;
        //TODO debug
        private float _lastTime = 0f;

        #region Свойства
        public static GameStore Store
        {
            get => _instance;
        }

        //модели
        public ISelectable[] SelectObjects
        {
            get => _selectObjects;
        }

        public FinishPoint[] FinishPoints
        {
            get => _finishPoints;
        }

        public ICollectable[] CollectObjects
        {
            get => _collectObjects;
        }

        public BaseModel[] AllModels
        {
            get => _allModels;
        }

        public int CurrentLevel
        {
            get => _currentLevel;
        }

        public LevelChecker LevelChecker
        {
            get
            {
                if (_levelChecker == null) _levelChecker = new LevelChecker();
                return _levelChecker;
            }
        }
        #endregion

        private void Start()
        {
            _instance = this;

            //получаем номер уровня
            if (PlayerPrefs.HasKey("CurrentLevel") && _levelMode == LevelMode.Game)
                _currentLevel = PlayerPrefs.GetInt("CurrentLevel");

            //загрузчик уровней
            _msController = new MSController();
            _msController.SetPrefabs(_boxPrefab, _wallPrefab, _starPrefab, _finishPointPrefab);
            if (_levelMode == LevelMode.Game) _msController.LoadLevel(_currentLevel, false);
            
            FillModelArrays();

            //контроллеры
            _selectController = new SelectController();
            _inputController = new InputController();
            _boxController = new BoxController();
            _finishController = new FinishController();
            _collectController = new CollectController();


            _allControllers = new BaseController[]
            {
                _selectController,
                _inputController,
                _boxController,
                _finishController,
                _collectController,
                _msController
            };

            InitAllControllers();

            _onUpdateController = new IOnUpdate[]
            {
                _inputController
            };

            //представления
            foreach (var view in GameObject.FindObjectsOfType<BaseView>())
            {
                if (view is LevelFinishView) _levelFinishView = (LevelFinishView)view;

                view.Init();

                if (view is IButton)
                {
                    IButton button = view as IButton;
                    switch (view.UIMarker)
                    {
                        case UIMarker.ButtonNextLevel:
                            button.SetAction(LoadNextLevel);
                            _nextLevelButton = view;
                            break;
                    }
                }
            }

            _nextLevelButton?.SetActive(false);
            _levelFinishView?.SetActive(false);

            //прочее
            AdaptateCamera();
        }

        /// <summary>
        /// Адаптация размера камеры к уровню
        /// </summary>
        private void AdaptateCamera()
        {
            //ищем координаты самых крайних объектов
            Vector3 modelPosition = _allModels[0].transform.position;
            Border modelBorder = new Border(modelPosition.x, modelPosition.y, modelPosition.x, modelPosition.y);

            for (int i = 1; i < _allModels.Length; i++)
            {
                modelPosition = _allModels[i].transform.position;

                if (modelPosition.x > modelBorder.MaxX)
                    modelBorder.MaxX = modelPosition.x;
                if (modelPosition.x < modelBorder.MinX)
                    modelBorder.MinX = modelPosition.x;
                if (modelPosition.y > modelBorder.MaxY)
                    modelBorder.MaxY = modelPosition.y;
                if (modelPosition.y < modelBorder.MinY)
                    modelBorder.MinY = modelPosition.y;
            }

            Camera mainCamera = Camera.main;
            bool isAdapted = false;

            while (!isAdapted)
            {
                Vector3 minVector = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
                Vector3 maxVector = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
                //координаты углов прямоугольника камеры
                Border cameraBorder = new Border(minVector.x, minVector.y, maxVector.x, maxVector.y);

                //адаптируем камеру
                if (modelBorder.MaxX >= cameraBorder.MaxX ||
                    modelBorder.MaxY >= cameraBorder.MaxY ||
                    modelBorder.MinX <= cameraBorder.MinX ||
                    modelBorder.MinY <= cameraBorder.MinY)
                {
                    mainCamera.orthographicSize++;
                }
                else
                {
                    isAdapted = true;
                }
            }
        }

        /// <summary>
        /// Пересобираем объекты на сцене в коллекции
        /// </summary>
        public void FillModelArrays()
        {
            List<ISelectable> selectObjects = new List<ISelectable>();
            List<FinishPoint> finishPoints = new List<FinishPoint>();
            List<ICollectable> collectObjects = new List<ICollectable>();

            //получаем все модели на сцене
            List<BaseModel> allModels = new List<BaseModel>();

            foreach (var model in GameObject.FindObjectsOfType<BaseModel>())
            {
                if (model is ISelectable) selectObjects.Add((ISelectable)model);
                if (model is FinishPoint) finishPoints.Add((FinishPoint)model);
                if (model is ICollectable) collectObjects.Add((ICollectable)model);

                allModels.Add(model);
            }

            _allModels = allModels.ToArray();
            _selectObjects = selectObjects.ToArray();
            _finishPoints = finishPoints.ToArray();
            _collectObjects = collectObjects.ToArray();
        }

        /// <summary>
        /// Инациализация всех контроллеров
        /// </summary>
        public void InitAllControllers()
        {
            foreach (var controller in _allControllers)
                controller.Init();
        }

        private void Update()
        {
            foreach (var controller in _onUpdateController)
                controller.OnUpdate();
        }

        //TODO debug info
        //Отрисовка системных переменных
        private void OnGUI()
        {

            if (Time.time - _lastTime > 0.5f)
            {
                _fps = 1 / Time.deltaTime;
                _lastTime = Time.time;
            }

            GUI.Label(new Rect(10, 10, 100, 100), $"{_fps:0.0}");
        }

        /// <summary>
        /// Метод срабатывающий, когда уровень завершён
        /// </summary>
        public void FinishLevel()
        {
            //TODO debug
            Debug.Log($"Уровень завершён, собрано звёзд {_collectController.CollectedStars}/{_collectController.MaxStars}");

            _levelFinishView?.SetActive(true);
            _levelFinishView?.SetStarsFinished(_collectController.MaxStars, _collectController.CollectedStars);

            //кнопка Дальше
            if (LevelChecker.HasNextLevel(_currentLevel))
            {
                if (!_nextLevelButton.IsActive)
                    _nextLevelButton.SetActive(true);
            }
            else
            {
                if (_nextLevelButton.IsActive)
                    _nextLevelButton.SetActive(false);
            }
        }

        /// <summary>
        /// Перезагрузка уровня
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Переход на в меню с уровнями
        /// </summary>
        public void LeaveLevel()
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        /// <summary>
        /// Загрузка следующего уровня
        /// </summary>
        public void LoadNextLevel()
        {
            if (LevelChecker.HasNextLevel(_currentLevel))
            {
                _currentLevel++;
                PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
                RestartLevel();
            }
        }

        /// <summary>
        /// Получение нужного контроллера
        /// </summary>
        /// <typeparam name="T">Контроллер</typeparam>
        /// <returns>Возвращает объект контроллера нужного типа</returns>
        public T GetController<T>() where T : BaseController
        {
            foreach (var controller in _allControllers)
            {
                if (controller is T)
                    return (T)controller;
            }

            return null;
        }
    }
}
