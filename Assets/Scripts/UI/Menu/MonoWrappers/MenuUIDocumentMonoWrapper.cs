using ConnectIt.Coroutines;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Menu.Views;
using ConnectIt.UI.Menu.Views.GJLoginMenu;
using ConnectIt.UI.Menu.Views.GJMenu;
using ConnectIt.UI.Menu.Views.MainMenu;
using ConnectIt.UI.Menu.Views.SelectLevelMenu;
using ConnectIt.UI.Menu.Views.SettingsMenu;
using ConnectIt.UI.Menu.Views.StatsMenu;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Extensions;
using ConnectIt.Utilities.Extensions.IUIBlocker;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.MonoWrappers
{
    public class MenuUIDocumentMonoWrapper : MonoBehaviour
    {
        public VisualElement MainMenuContainer { get; private set; }
        public VisualElement SelectLevelContainer { get; private set; }
        public VisualElement SettingsContainer { get; private set; }
        public VisualElement StatsContainer { get; private set; }
        public VisualElement ShopContainer { get; private set; }
        public VisualElement GJContainer { get; private set; }
        public VisualElement GJLoginContainer { get; private set; }

        private UIDocument _uiDocument;
        private VisualElement _documentRootVE => _uiDocument.rootVisualElement;
        private VisualElement _rootVE;
        private VisualElement _topContainer;
        private VisualElement _contentContainer;
        private VisualElement _bottomContainer;

        private IUIBlocker _uiBlocker;
        private ICoroutinesGlobalContainer _coroutinesGlobalContainer;

        private FramesSwitcher<VisualElement> _framesSwitcher;
        private MainMenuView.Factory _mainMenuViewFactory;
        private MainMenuView _mainMenuView;
        private SelectLevelMenuView.Factory _selectLevelMenuViewFactory;
        private SelectLevelMenuView _selectLevelMenuView;
        private ShopMenuView.Factory _shopMenuViewFactory;
        private ShopMenuView _shopMenuView;
        private SettingsMenuView.Factory _settingsMenuViewFactory;
        private SettingsMenuView _settingsMenuView;
        private StatsMenuView.Factory _statsMenuViewFactory;
        private StatsMenuView _statsMenuView;
        private GJLoginMenuView.Factory _gjLoginMenuViewFactory;
        private GJLoginMenuView _gjLoginMenuView;
        private GJMenuView.Factory _gjMenuViewFactory;
        private GJMenuView _gjMenuView;

        private CompletedLevelsView.Factory _completedLevelsViewFactory;
        private CompletedLevelsView _completedLevelsView;
        private VersionView.Factory _versionViewFactory;
        private VersionView _versionView;
        private ClickableCoinsView.Factory _clickableCoinsViewFactory;
        private ClickableCoinsView _clickableCoinsView;

        private Coroutine _firstFrameSwitchCoroutine;
        private Coroutine _waitForTransitionEndCoroutine;

        [Inject]
        public void Constructor(
            IUIBlocker uiBlocker,
            ICoroutinesGlobalContainer coroutinesGlobalContainer,
            MainMenuView.Factory mainMenuViewFactory,
            SelectLevelMenuView.Factory selectLevelMenuViewFactory,
            ShopMenuView.Factory shopMenuViewFactory,
            SettingsMenuView.Factory settingsMenuViewFactory,
            StatsMenuView.Factory statsMenuViewFactory,
            GJLoginMenuView.Factory gjLoginMenuViewFactory,
            GJMenuView.Factory gjMenuViewFactory,
            VersionView.Factory versionViewFactory,
            CompletedLevelsView.Factory completedLevelsViewFactory,
            ClickableCoinsView.Factory clickableCoinsViewFactory)
        {
            _uiBlocker = uiBlocker;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
            _mainMenuViewFactory = mainMenuViewFactory;
            _selectLevelMenuViewFactory = selectLevelMenuViewFactory;
            _shopMenuViewFactory = shopMenuViewFactory;
            _settingsMenuViewFactory = settingsMenuViewFactory;
            _statsMenuViewFactory = statsMenuViewFactory;
            _gjLoginMenuViewFactory = gjLoginMenuViewFactory;
            _gjMenuViewFactory = gjMenuViewFactory;

            _versionViewFactory = versionViewFactory;
            _completedLevelsViewFactory = completedLevelsViewFactory;
            _clickableCoinsViewFactory = clickableCoinsViewFactory;
        }

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();

            _rootVE = _documentRootVE.Q<VisualElement>(NameConstants.RootName);

            _topContainer = _documentRootVE.Q<VisualElement>(NameConstants.TopContainer);
            _contentContainer = _documentRootVE.Q<VisualElement>(NameConstants.ContentContainer);
            _bottomContainer = _documentRootVE.Q<VisualElement>(NameConstants.BottomContainer);
        }

        private void Start()
        {
            CreateSwitcherAndFrames();
            CreateViews();
        }

        private void OnDestroy()
        {
            _framesSwitcher.FrameSwitched -= OnFrameSwitched;

            DisposeDisposableViews();
            StopAllRunningCoroutines();
        }

        #region SwitcherAndFramesCreation

        private void CreateSwitcherAndFrames()
        {
            MainMenuContainer = _contentContainer.Q<VisualElement>(NameConstants.MainMenuContainer);
            Frame<VisualElement> mainMenuFrame = new(MainMenuContainer);

            SelectLevelContainer = _contentContainer.Q<VisualElement>(NameConstants.SelectLevelContainer);
            Frame<VisualElement> selectLevelframe = new(SelectLevelContainer);

            SettingsContainer = _contentContainer.Q<VisualElement>(NameConstants.SettingsContainer);
            Frame<VisualElement> settingsFrame = new(SettingsContainer);

            StatsContainer = _contentContainer.Q<VisualElement>(NameConstants.StatsContainer);
            Frame<VisualElement> statsFrame = new(StatsContainer);

            ShopContainer = _contentContainer.Q<VisualElement>(NameConstants.ShopContainer);
            Frame<VisualElement> shopFrame = new(ShopContainer);

            GJContainer = _contentContainer.Q<VisualElement>(NameConstants.GJContainer);
            Frame<VisualElement> gjFrame = new(GJContainer);

            GJLoginContainer = _contentContainer.Q<VisualElement>(NameConstants.GJLoginContainer);
            Frame<VisualElement> gjLoginFrame = new(GJLoginContainer);

            Frame<VisualElement>[] frames = new Frame<VisualElement>[]
            {
                mainMenuFrame,
                selectLevelframe,
                settingsFrame,
                statsFrame,
                shopFrame,
                gjFrame,
                gjLoginFrame
            };

            _framesSwitcher = new(frames, EnableFrame, DisableFrame);
            _framesSwitcher.FrameSwitched += OnFrameSwitched;

            _firstFrameSwitchCoroutine = _coroutinesGlobalContainer.DelayedAction(SwitchToFirstFrame);

            _mainMenuView = _mainMenuViewFactory.Create(MainMenuContainer, _framesSwitcher, this);
            _selectLevelMenuView = _selectLevelMenuViewFactory.Create(SelectLevelContainer, _rootVE, _framesSwitcher, this);
            _shopMenuView = _shopMenuViewFactory.Create(ShopContainer, _rootVE, _framesSwitcher, this);
            _settingsMenuView = _settingsMenuViewFactory.Create(SettingsContainer, _rootVE, _framesSwitcher, this);
            _statsMenuView = _statsMenuViewFactory.Create(StatsContainer, _rootVE, _framesSwitcher, this);
            _gjLoginMenuView = _gjLoginMenuViewFactory.Create(GJLoginContainer, _framesSwitcher, this);
            _gjMenuView = _gjMenuViewFactory.Create(GJContainer, _framesSwitcher, this);
        }

        private void SwitchToFirstFrame()
        {
            _firstFrameSwitchCoroutine = null;

            _framesSwitcher.SwitchTo(MainMenuContainer);
        }

        private void OnFrameSwitched(VisualElement frame)
        {
            float transitionLength = frame.resolvedStyle.CalculateMaxTransitionLengthSec();
            if (transitionLength == 0)
                return;

            _uiBlocker.SetBlock(true, BlockPriority.Transition, this);

            _waitForTransitionEndCoroutine = _coroutinesGlobalContainer.DelayedAction(OnTransitionEnded, transitionLength);
        }

        private void OnTransitionEnded()
        {
            _waitForTransitionEndCoroutine = null;

            _uiBlocker.ResetBlock(this);
        }

        private void EnableFrame(VisualElement frame)
        {
            frame.RemoveFromClassList(ClassNamesConstants.MenuView.ContentContainerFrameClosed);
        }

        private void DisableFrame(VisualElement frame)
        {
            frame.AddToClassList(ClassNamesConstants.MenuView.ContentContainerFrameClosed);
        }

        #endregion

        private void CreateViews()
        {
            _versionView = _versionViewFactory.Create(
                _bottomContainer.Q<Label>(NameConstants.VersionLabel));

            _completedLevelsView = _completedLevelsViewFactory.Create(
                _topContainer.Q<Label>(NameConstants.CompletedLevelsLabel));

            _clickableCoinsView = _clickableCoinsViewFactory.Create(
                _topContainer.Q<Button>(NameConstants.CoinsLabel),
                OnCoinsClick);
        }

        private void DisposeDisposableViews()
        {
            _mainMenuView.Dispose();
            _selectLevelMenuView.Dispose();
            _shopMenuView.Dispose();
            _settingsMenuView.Dispose();
            _statsMenuView.Dispose();
            _gjLoginMenuView.Dispose();
            _gjMenuView.Dispose();

            _versionView.Dispose();
            _completedLevelsView.Dispose();
            _clickableCoinsView.Dispose();
        }

        private void StopAllRunningCoroutines()
        {
            if (_waitForTransitionEndCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_waitForTransitionEndCoroutine);

            if (_firstFrameSwitchCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_firstFrameSwitchCoroutine);
        }

        #region CoinsClick

        private void OnCoinsClick()
        {
            _framesSwitcher.SwitchTo(ShopContainer);
        }

        #endregion
    }
}