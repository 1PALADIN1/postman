using Core.Enum;
using Core.Helper;
using Core.View;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private Button _levelButton;
        [Header("Позицирование кнопок")]
        [SerializeField]
        private int _buttonsInColumn = 5;
        [SerializeField]
        private int _buttonsInRow = 2;
        [SerializeField]
        private float _widthPercent = 0.1f;
        //настройка рядов
        [SerializeField]
        private float _heightPercentTop = 0.1f;
        [SerializeField]
        private float _heightPercentBottom = 0.25f;
        
        private GameObject _levelsPanel;
        private float _panelBrilliantWidth = 841;
        private float _panelWidth = 0.0f;
        private float _panelHeight = 0.0f;
        private Pager<int> _pager;
        private LevelChecker _levelChecker;
        private AdaptiveView _adaptiveView;

        private List<IPanel> _panelList;
        private BaseView _mainMenuPanel;
        private BaseView _levelPanel;

        private void Start()
        {
            _levelsPanel = GameObject.Find("LevelsPanel");
            Rect panelRect = _levelsPanel.GetComponent<RectTransform>().rect;
            _panelWidth = panelRect.width;
            _panelHeight = panelRect.height;
            _levelChecker = new LevelChecker();
            _adaptiveView = new AdaptiveView(_panelBrilliantWidth, _panelWidth);

            InitPager();
            LoadPage(_pager.CurrentPageNumber);

            //TODO подстроить весь код под архитектуру ниже
            _panelList = new List<IPanel>();

            foreach (var view in GameObject.FindObjectsOfType<BaseView>())
            {
                view.Init();

                //панели
                if (view is IPanel)
                {
                    IPanel panel = view as IPanel;
                    _panelList.Add(view as IPanel);

                    switch (view.UIMarker)
                    {
                        case UIMarker.PanelMainMenu:
                            _mainMenuPanel = view;
                            _mainMenuPanel.SetActive(true);
                            _adaptiveView.AdaptateToPanelPosition(panel.GetAllChildButtonsAsBase(), panel, true);
                            break;
                        case UIMarker.PanelLevelList:
                            _levelPanel = view;
                            _levelPanel.SetActive(false);

                            //TODO test feature
                            _adaptiveView.AdaptateToPanelPosition(panel.GetAllChildButtonsAsBase(), panel, true, _buttonsInColumn, _buttonsInRow);
                            break;
                    }
                }

                //кнопки
                if (view is IButton)
                {
                    IButton button = view as IButton;

                    switch (view.UIMarker)
                    {
                        case UIMarker.ButtonNewGame:
                            button.SetAction(OpenLevelList);
                            break;
                        case UIMarker.ButtonLevel:
                            button.SetAction(StartLevel);
                            break;
                    }
                }

            }
        }

        #region Button Actions
        /// <summary>
        /// Открывает меню с уровнями
        /// </summary>
        private void OpenLevelList()
        {
            _mainMenuPanel.SetActive(false);
            _levelPanel.SetActive(true);
        }

        /// <summary>
        /// Загрузка уровня
        /// </summary>
        private void StartLevel()
        {
            SceneManager.LoadScene("LevelScene");
        }
        #endregion

        private void InitPager()
        {
            List<int> pagerItems = _levelChecker.LevelsInt;
             _pager = new Pager<int>(pagerItems.ToArray(), _buttonsInColumn * _buttonsInRow);
        }

        public void LoadPage(int pageNum)
        {
            ClearLevelButtons();

            float insideButsX = _panelWidth * (1 - _widthPercent * 2) / _buttonsInColumn;
            float insideButsY = _panelHeight * (1 - (_heightPercentTop + _heightPercentBottom)) / _buttonsInRow;

            int[] pageElements = _pager.GetPage(pageNum);
            int levelCount = 0;
            bool isFinished = false;

            for (int j = 0; j < _buttonsInRow; j++)
            {
                for (int i = 0; i < _buttonsInColumn; i++)
                {
                    //если на странице больше нет элементов
                    if (levelCount > pageElements.Length - 1)
                    {
                        isFinished = true;
                        break;
                    }

                    Button butInst = GameObject.Instantiate<Button>(_levelButton, _levelsPanel.transform);
                    LevelButtonView buttonView = butInst.GetComponent<LevelButtonView>();

                    if (buttonView != null)
                    {
                        buttonView.Init();
                        
                        buttonView.Text = pageElements[levelCount].ToString();
                        buttonView.Level = pageElements[levelCount];

                        levelCount++;
                    }
                }

                //выход из двойного цикла
                if (isFinished)
                    break;
            }
        }

        /// <summary>
        /// Очистка кнопок с уровнями
        /// </summary>
        private void ClearLevelButtons()
        {
            Button[] childButtons = _levelsPanel.GetComponentsInChildren<Button>();
            if (childButtons != null)
            {
                foreach (var child in childButtons)
                    Destroy(child.gameObject);
            }
        }

        private void Update()
        {
            //TODO debug
            if (Input.GetKeyDown(KeyCode.P))
                LoadPage(_pager.GetNextPageNumber());

            //TODO debug
            if (Input.GetKeyDown(KeyCode.O))
                LoadPage(_pager.GetPrevPageNumber());
        }
    }
}
