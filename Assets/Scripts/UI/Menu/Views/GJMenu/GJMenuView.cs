using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Tools;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.GJMenu
{
    public class GJMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly FramesSwitcher<VisualElement> _framesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;

        private DefaultButtonView _backButton;

        public GJMenuView(VisualElement viewRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory)
        {
            _viewRoot = viewRoot;
            _framesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
        }

        public void Initialize()
        {
            CreateViews();
        }

        public void Dispose()
        {
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
        }

        #region BackButton

        private void OnBackButtonClick()
        {
            _framesSwitcher.SwitchBackOrToDefault(_menuUIDocumentMonoWrapper.MainMenuContainer);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, GJMenuView> { }
    }
}
