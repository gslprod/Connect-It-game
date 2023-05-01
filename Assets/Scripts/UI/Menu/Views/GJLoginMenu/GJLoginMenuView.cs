using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Tools;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.GJLoginMenu
{
    public class GJLoginMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly FramesSwitcher<VisualElement> _framesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedLabelView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;

        private DefaultButtonView _backButton;
        private DefaultLocalizedLabelView _titleLabel;

        public GJLoginMenuView(VisualElement viewRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedLabelView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory)
        {
            _viewRoot = viewRoot;
            _framesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
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
                _viewRoot.Q<Button>(NameConstants.GJLoginMenu.BackButton), OnBackButtonClick);

            _titleLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJLoginMenu.TitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJLoginMenu.Title, null));
        }

        private void DisposeDisposableViews()
        {
            _backButton.Dispose();
            _titleLabel.Dispose();
        }

        #region BackButton

        private void OnBackButtonClick()
        {
            _framesSwitcher.SwitchBackOrToDefault(_menuUIDocumentMonoWrapper.MainMenuContainer);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, GJLoginMenuView> { }
    }
}
