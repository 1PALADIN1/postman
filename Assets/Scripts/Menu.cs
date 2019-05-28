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

        //панельки
        private List<IPanel> _panelList;
        private BaseView _mainMenuPanel;
        private BaseView _levelPanel;
        private BaseView _abouPanel;

        //кнопки
        private BaseView _nextPageButton;
        private BaseView _prevPageButton;

        private void Start()
        {
            _levelsPanel = GameObject.Find("LevelButtons");
            Rect panelRect = _levelsPanel.GetComponent<RectTransform>().rect;
            _panelWidth = panelRect.width;
            _panelHeight = panelRect.height;
            _levelChecker = new LevelChecker();
            _adaptiveView = new AdaptiveView(_panelBrilliantWidth, _panelWidth);

            InitPager();
            LoadPage(_pager.CurrentPageNumber);
            
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
                            break;
                        case UIMarker.PanelLevelList:
                            _levelPanel = view;
                            _levelPanel.SetActive(false);
                            break;
                        case UIMarker.PanelAbout:
                            _abouPanel = view;
                            _abouPanel.SetActive(false);
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
                            //button.SetAction(StartLevel);
                            break;
                        case UIMarker.ButtonAbout:
                            button.SetAction(OpenAboutPanel);
                            break;
                        case UIMarker.ButtonQuit:
                            button.SetAction(QuitGame);
                            break;
                        case UIMarker.ButtonBackToMainMenu:
                            button.SetAction(BackToMenu);
                            break;
                        case UIMarker.ButtonNextPage:
                            button.SetAction(GoNextPage);
                            _nextPageButton = view;
                            break;
                        case UIMarker.ButtonPreviousPage:
                            button.SetAction(GoPreviousPage);
                            _prevPageButton = view;
                            break;
                    }
                }
            }

            //адаптация кнопок к представлению
            if (_levelPanel is IPanel)
            {
                IPanel panel = _levelPanel as IPanel;
                _adaptiveView.AdaptateToPanelPosition(panel.GetAllChildButtonsAsBase(), panel, true, _buttonsInColumn, _buttonsInRow);
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

            ActivatePageButtons();
        }

        /// <summary>
        /// Загрузка уровня
        /// </summary>
        private void StartLevel()
        {
            SceneManager.LoadScene("LevelScene");
        }

        /// <summary>
        /// Выход из игры
        /// </summary>
        private void QuitGame()
        {
            Application.Quit();
        }

        /// <summary>
        /// Открытие окна "Об игре"
        /// </summary>
        private void OpenAboutPanel()
        {
            _mainMenuPanel.SetActive(false);
            _abouPanel.SetActive(true);
        }

        /// <summary>
        /// Возвращение в гланове меню
        /// </summary>
        private void BackToMenu()
        {
            if (_abouPanel.IsActive) _abouPanel.SetActive(false);
            if (_levelPanel.IsActive) _levelPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }

        /// <summary>
        /// Перелистывает на страницу вперёд
        /// </summary>
        private void GoNextPage()
        {
            LoadPage(_pager.GetNextPageNumber());
            ActivatePageButtons();
        }

        /// <summary>
        /// Перелистывает на предыдущую страницу
        /// </summary>
        private void GoPreviousPage()
        {
            LoadPage(_pager.GetPrevPageNumber());
            ActivatePageButtons();
        }
        #endregion

        private void ActivatePageButtons()
        {
            if (_pager.HasNextPage)
            {
                if (!_nextPageButton.IsActive)
                    _nextPageButton.SetActive(true);
            }
            else
            {
                if (_nextPageButton.IsActive)
                    _nextPageButton.SetActive(false);
            }

            if (_pager.HasPrevPage)
            {
                if (!_prevPageButton.IsActive)
                    _prevPageButton.SetActive(true);
            }
            else
            {
                if (_prevPageButton.IsActive)
                    _prevPageButton.SetActive(false);
            }
        }

        private void InitPager()
        {
            List<int> pagerItems = _levelChecker.LevelsInt;
             _pager = new Pager<int>(pagerItems.ToArray(), _buttonsInColumn * _buttonsInRow);
        }

        public void LoadPage(int pageNum)
        {
            ClearLevelButtons();

            List<BaseView> baseButtons = new List<BaseView>();
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
                        buttonView.SetAction(StartLevel);
                        baseButtons.Add(buttonView);
                        levelCount++;
                    }
                }

                //выход из двойного цикла
                if (isFinished)
                    break;
            }

            //адаптация кнопок к представлению
            if (_levelPanel is IPanel)
            {
                IPanel panel = _levelPanel as IPanel;
                _adaptiveView.AdaptateToPanelPosition(baseButtons.ToArray(), panel, true, _buttonsInColumn, _buttonsInRow);
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
                GoNextPage();

            //TODO debug
            if (Input.GetKeyDown(KeyCode.O))
                GoPreviousPage();
        }
    }
}
