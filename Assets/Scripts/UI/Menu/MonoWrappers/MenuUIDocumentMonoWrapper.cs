using ConnectIt.Coroutines;
using ConnectIt.UI.Menu.Views.MainMenu;
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
        public VisualElement GjContainer { get; private set; }
        public VisualElement GjLoginContainer { get; private set; }

        private UIDocument _uiDocument;
        private VisualElement _documentRootVE => _uiDocument.rootVisualElement;
        private VisualElement _rootVE;

        private MainMenuView.Factory _mainMenuViewFactory;
        private IUIBlocker _uiBlocker;
        private ICoroutinesGlobalContainer _coroutinesGlobalContainer;

        private FramesSwitcher<VisualElement> _framesSwitcher;
        private MainMenuView _mainMenuView;

        private Coroutine _firstFrameSwitchCoroutine;
        private Coroutine _waitForTransitionEndCoroutine;

        [Inject]
        public void Constructor(
            MainMenuView.Factory mainMenuViewFactory,
            IUIBlocker uiBlocker,
            ICoroutinesGlobalContainer coroutinesGlobalContainer)
        {
            _mainMenuViewFactory = mainMenuViewFactory;
            _uiBlocker = uiBlocker;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
        }

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();

            _rootVE = _documentRootVE.Q<VisualElement>(NameConstants.RootName);
        }

        private void Start()
        {
            CreateSwitcherAndViews();
        }

        private void OnDestroy()
        {
            _framesSwitcher.FrameSwitched -= OnFrameSwitched;

            DisposeDisposableViews();
            StopAllRunningCoroutines();
        }

        private void DisposeDisposableViews()
        {
            _mainMenuView.Dispose();
        }

        private void StopAllRunningCoroutines()
        {
            if (_waitForTransitionEndCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_waitForTransitionEndCoroutine);

            if (_firstFrameSwitchCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_firstFrameSwitchCoroutine);
        }

        #region SwitcherAndViewsCreation

        private void CreateSwitcherAndViews()
        {
            MainMenuContainer = _rootVE.Q<VisualElement>(NameConstants.MainMenuContainer);
            Frame<VisualElement> mainMenuFrame = new(MainMenuContainer);

            SelectLevelContainer = _rootVE.Q<VisualElement>(NameConstants.SelectLevelContainer);
            Frame<VisualElement> selectLevelframe = new(SelectLevelContainer);

            SettingsContainer = _rootVE.Q<VisualElement>(NameConstants.SettingsContainer);
            Frame<VisualElement> settingsFrame = new(SettingsContainer);

            StatsContainer = _rootVE.Q<VisualElement>(NameConstants.StatsContainer);
            Frame<VisualElement> statsFrame = new(StatsContainer);

            ShopContainer = _rootVE.Q<VisualElement>(NameConstants.ShopContainer);
            Frame<VisualElement> shopFrame = new(ShopContainer);

            GjContainer = _rootVE.Q<VisualElement>(NameConstants.GjContainer);
            Frame<VisualElement> gjFrame = new(GjContainer);

            GjLoginContainer = _rootVE.Q<VisualElement>(NameConstants.GjLoginContainer);
            Frame<VisualElement> gjLoginFrame = new(GjLoginContainer);

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

            _firstFrameSwitchCoroutine = _coroutinesGlobalContainer.DelayedAction(() => SwitchToFirstFrame());

            _mainMenuView = _mainMenuViewFactory.Create(MainMenuContainer, _framesSwitcher, this);
        }

        private void SwitchToFirstFrame()
        {
            _firstFrameSwitchCoroutine = null;

            _framesSwitcher.SwitchTo(MainMenuContainer);
        }

        private void OnFrameSwitched(VisualElement frame)
        {
            float transitionLength = frame.resolvedStyle.CalculateMaxTransitionLengthSec();
            print(transitionLength);
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
    }
}