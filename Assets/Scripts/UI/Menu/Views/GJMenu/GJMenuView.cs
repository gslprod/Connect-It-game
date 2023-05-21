using ConnectIt.Coroutines;
using ConnectIt.Coroutines.CustomYieldInstructions;
using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Menu.Views.GJMenu.GJLoginMenu;
using ConnectIt.UI.Menu.Views.GJMenu.GJProfileMenu;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Extensions;
using ConnectIt.Utilities.Extensions.IUIBlocker;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.GJMenu
{
    public class GJMenuView : IInitializable, IDisposable
    {
        public VisualElement GJProfileContainer { get; private set; }
        public VisualElement GJLoginContainer { get; private set; }

        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly FramesSwitcher<VisualElement> _parentFramesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;
        private readonly IUIBlocker _uiBlocker;
        private readonly GameJoltAPIProvider _gameJoltAPIProvider;
        private readonly DialogBoxView.Factory _dialogBoxViewFactory;
        private readonly TextKey.Factory _textKeyFactory;

        private GJLoginMenuView.Factory _gjLoginMenuViewFactory;
        private GJLoginMenuView _gjLoginMenuView;
        private GJProfileMenuView.Factory _gjProfileMenuViewFactory;
        private GJProfileMenuView _gjProfileMenuView;

        private FramesSwitcher<VisualElement> _localFramesSwitcher;
        private DefaultButtonView _backButton;

        private TransitionsStopWaiter _transitionsStopWaiter = new();
        private Coroutine _firstFrameSwitchCoroutine;

        private bool _showFailMessageBox = false;

        public GJMenuView(
            VisualElement viewRoot,
            VisualElement mainRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory,
            ICoroutinesGlobalContainer coroutinesGlobalContainer,
            GJLoginMenuView.Factory gjLoginMenuViewFactory,
            GJProfileMenuView.Factory gjProfileMenuViewFactory,
            IUIBlocker uiBlocker,
            GameJoltAPIProvider gameJoltAPIProvider,
            DialogBoxView.Factory dialogBoxViewFactory,
            TextKey.Factory textKeyFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _parentFramesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
            _gjLoginMenuViewFactory = gjLoginMenuViewFactory;
            _gjProfileMenuViewFactory = gjProfileMenuViewFactory;
            _uiBlocker = uiBlocker;
            _gameJoltAPIProvider = gameJoltAPIProvider;
            _dialogBoxViewFactory = dialogBoxViewFactory;
            _textKeyFactory = textKeyFactory;
        }

        public void Initialize()
        {
            _gameJoltAPIProvider.UserLogInAttempt += OnGJLogInAttempt;
            _gameJoltAPIProvider.UserLogOut += OnGJLogOut;

            _parentFramesSwitcher.FrameSwitched += OnParentFrameSwitched;

            CreateSwitcherAndFrames();
            CreateViews();
        }

        public void Dispose()
        {
            _gameJoltAPIProvider.UserLogInAttempt -= OnGJLogInAttempt;
            _gameJoltAPIProvider.UserLogOut -= OnGJLogOut;

            _parentFramesSwitcher.FrameSwitched -= OnParentFrameSwitched;

            _localFramesSwitcher.FrameSwitched -= OnFrameSwitched;
            _transitionsStopWaiter.AbortIfWaiting();

            StopAllRunningCoroutines();
            DisposeDisposableViews();
        }

        private void CreateViews()
        {
            _backButton = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.GJMenu.BackButton), OnBackButtonClick);
        }

        private void DisposeDisposableViews()
        {
            _backButton.Dispose();
            _gjLoginMenuView.Dispose();
            _gjProfileMenuView.Dispose();
        }

        private void StopAllRunningCoroutines()
        {
            if (_firstFrameSwitchCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_firstFrameSwitchCoroutine);
        }

        #region SwitcherAndFramesCreationAndSetup

        private void CreateSwitcherAndFrames()
        {
            GJProfileContainer = _viewRoot.Q<VisualElement>(NameConstants.GJMenu.GJProfileContainer);
            Frame<VisualElement> gjFrame = new(GJProfileContainer);

            GJLoginContainer = _viewRoot.Q<VisualElement>(NameConstants.GJMenu.GJLoginContainer);
            Frame<VisualElement> gjLoginFrame = new(GJLoginContainer);

            Frame<VisualElement>[] frames = new Frame<VisualElement>[]
            {
                gjFrame,
                gjLoginFrame
            };

            _localFramesSwitcher = new(frames, EnableFrame, DisableFrame);
            _localFramesSwitcher.FrameSwitched += OnFrameSwitched;
            _firstFrameSwitchCoroutine = _coroutinesGlobalContainer.DelayedAction(SwitchToFirstFrame, new WaitForFrames(2));

            _gjLoginMenuView = _gjLoginMenuViewFactory.Create(GJLoginContainer, _mainRoot);
            _gjProfileMenuView = _gjProfileMenuViewFactory.Create(GJProfileContainer, _mainRoot);
        }

        private void SwitchToFirstFrame()
        {
            _firstFrameSwitchCoroutine = null;

            _localFramesSwitcher.SwitchTo(GJLoginContainer);
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

        #region GameJolt

        private void OnGJLogInAttempt(bool success)
        {
            if (success)
            {
                _localFramesSwitcher.SwitchTo(GJProfileContainer, false);
                return;
            }

            if (_parentFramesSwitcher.Current != _viewRoot)
            {
                _showFailMessageBox = true;
                return;
            }

            ShowFailLogInMessageBox();
        }

        private void OnGJLogOut()
        {
            _localFramesSwitcher.SwitchTo(GJLoginContainer, false);
        }

        private void OnParentFrameSwitched(VisualElement current)
        {
            if (!_showFailMessageBox || current != _viewRoot)
                return;

            ShowFailLogInMessageBox();
            _showFailMessageBox = false;
        }

        private void ShowFailLogInMessageBox()
        {
            _dialogBoxViewFactory.CreateDefaultOneButtonDialogBox(
                _mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LoginFailed_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LoginFailed_Message),
                _textKeyFactory.Create(TextKeysConstants.Common.Close),
                true);
        }

        #endregion

        #region BackButton

        private void OnBackButtonClick()
        {
            _parentFramesSwitcher.SwitchBackOrToDefault(_menuUIDocumentMonoWrapper.MainMenuContainer);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, GJMenuView> { }
    }
}
