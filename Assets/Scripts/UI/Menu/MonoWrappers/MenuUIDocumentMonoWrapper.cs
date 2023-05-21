using ConnectIt.Coroutines;
using ConnectIt.Coroutines.CustomYieldInstructions;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Menu.Views;
using ConnectIt.UI.Menu.Views.GJMenu;
using ConnectIt.UI.Menu.Views.MainMenu;
using ConnectIt.UI.Menu.Views.SelectLevelMenu;
using ConnectIt.UI.Menu.Views.SettingsMenu;
using ConnectIt.UI.Menu.Views.StatsMenu;
using ConnectIt.UI.Tools;
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
        private GJMenuView.Factory _gjMenuViewFactory;
        private GJMenuView _gjMenuView;

        private CompletedLevelsView.Factory _completedLevelsViewFactory;
        private CompletedLevelsView _completedLevelsView;
        private VersionView.Factory _versionViewFactory;
        private VersionView _versionView;
        private ClickableCoinsView.Factory _clickableCoinsViewFactory;
        private ClickableCoinsView _clickableCoinsView;
        private GameJoltUsernameView.Factory _gameJoltUsernameViewFactory;
        private GameJoltUsernameView _gameJoltUsernameView;
        private GameJoltAvatarView.Factory _gameJoltAvatarViewFactory;
        private GameJoltAvatarView _gameJoltAvatarView;

        private readonly TransitionsStopWaiter _transitionsStopWaiter = new();
        private Coroutine _firstFrameSwitchCoroutine;

        [Inject]
        public void Constructor(
            IUIBlocker uiBlocker,
            ICoroutinesGlobalContainer coroutinesGlobalContainer,
            MainMenuView.Factory mainMenuViewFactory,
            SelectLevelMenuView.Factory selectLevelMenuViewFactory,
            ShopMenuView.Factory shopMenuViewFactory,
            SettingsMenuView.Factory settingsMenuViewFactory,
            StatsMenuView.Factory statsMenuViewFactory,
            GJMenuView.Factory gjMenuViewFactory,
            VersionView.Factory versionViewFactory,
            CompletedLevelsView.Factory completedLevelsViewFactory,
            ClickableCoinsView.Factory clickableCoinsViewFactory,
            GameJoltUsernameView.Factory gameJoltUsernameViewFactory,
            GameJoltAvatarView.Factory gameJoltAvatarViewFactory)
        {
            _uiBlocker = uiBlocker;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
            _mainMenuViewFactory = mainMenuViewFactory;
            _selectLevelMenuViewFactory = selectLevelMenuViewFactory;
            _shopMenuViewFactory = shopMenuViewFactory;
            _settingsMenuViewFactory = settingsMenuViewFactory;
            _statsMenuViewFactory = statsMenuViewFactory;
            _gjMenuViewFactory = gjMenuViewFactory;

            _versionViewFactory = versionViewFactory;
            _completedLevelsViewFactory = completedLevelsViewFactory;
            _clickableCoinsViewFactory = clickableCoinsViewFactory;
            _gameJoltUsernameViewFactory = gameJoltUsernameViewFactory;
            _gameJoltAvatarViewFactory = gameJoltAvatarViewFactory;
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
            _transitionsStopWaiter.AbortIfWaiting();

            DisposeDisposableViews();
            StopAllRunningCoroutines();
        }

        #region SwitcherAndFramesCreationAndSetup

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

            Frame<VisualElement>[] frames = new Frame<VisualElement>[]
            {
                mainMenuFrame,
                selectLevelframe,
                settingsFrame,
                statsFrame,
                shopFrame,
                gjFrame
            };

            _framesSwitcher = new(frames, EnableFrame, DisableFrame);
            _framesSwitcher.FrameSwitched += OnFrameSwitched;
            _firstFrameSwitchCoroutine = _coroutinesGlobalContainer.DelayedAction(SwitchToFirstFrame, new WaitForFrames(2));

            _mainMenuView = _mainMenuViewFactory.Create(MainMenuContainer, _framesSwitcher, this);
            _selectLevelMenuView = _selectLevelMenuViewFactory.Create(SelectLevelContainer, _rootVE, _framesSwitcher, this);
            _shopMenuView = _shopMenuViewFactory.Create(ShopContainer, _rootVE, _framesSwitcher, this);
            _settingsMenuView = _settingsMenuViewFactory.Create(SettingsContainer, _rootVE, _framesSwitcher, this);
            _statsMenuView = _statsMenuViewFactory.Create(StatsContainer, _rootVE, _framesSwitcher, this);
            _gjMenuView = _gjMenuViewFactory.Create(GJContainer, _rootVE, _framesSwitcher, this);
        }

        private void SwitchToFirstFrame()
        {
            _firstFrameSwitchCoroutine = null;

            _framesSwitcher.SwitchTo(MainMenuContainer);
        }

        private void OnFrameSwitched(VisualElement frame)
        {
            _uiBlocker.SetBlock(true, BlockPriority.Transition, this);

            _coroutinesGlobalContainer.DelayedAction(() => _transitionsStopWaiter.AbortCurrentAndWait(OnTransitionEnded, frame));
        }

        private void OnTransitionEnded()
        {
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

            _gameJoltUsernameView = _gameJoltUsernameViewFactory.Create(
                _topContainer.Q<Label>(NameConstants.UsernameLabel));

            _gameJoltAvatarView = _gameJoltAvatarViewFactory.Create(
                _topContainer.Q<VisualElement>(NameConstants.Avatar));
        }

        private void DisposeDisposableViews()
        {
            _mainMenuView.Dispose();
            _selectLevelMenuView.Dispose();
            _shopMenuView.Dispose();
            _settingsMenuView.Dispose();
            _statsMenuView.Dispose();
            _gjMenuView.Dispose();

            _versionView.Dispose();
            _completedLevelsView.Dispose();
            _clickableCoinsView.Dispose();
            _gameJoltUsernameView.Dispose();
            _gameJoltAvatarView.Dispose();
        }

        private void StopAllRunningCoroutines()
        {
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