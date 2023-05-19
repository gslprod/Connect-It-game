using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.LoadingScreen;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.GJLoginMenu
{
    public class GJLoginMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly FramesSwitcher<VisualElement> _framesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedLabelView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly DefaultLocalizedButtonView.Factory _defaultLocalizedButtonViewFactory;
        private readonly GameJoltAPIProvider _gameJoltAPIProvider;
        private readonly LoadingDialogBoxView.Factory _loadingDialogBoxViewFactory;

        private DefaultButtonView _backButton;
        private DefaultLocalizedLabelView _titleLabel;
        private DefaultLocalizedLabelView _gjInfoLabel;
        private DefaultLocalizedLabelView _usernameLabel;
        private TextField _usernameTextField;
        private DefaultLocalizedLabelView _tokenLabel;
        private TextField _tokenTextField;
        private DefaultLocalizedButtonView _loginButton;

        public GJLoginMenuView(
            VisualElement viewRoot,
            VisualElement mainRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedLabelView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            DefaultLocalizedButtonView.Factory defaultLocalizedButtonViewFactory,
            GameJoltAPIProvider gameJoltAPIProvider,
            LoadingDialogBoxView.Factory loadingDialogBoxViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _framesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _defaultLocalizedButtonViewFactory = defaultLocalizedButtonViewFactory;
            _gameJoltAPIProvider = gameJoltAPIProvider;
            _loadingDialogBoxViewFactory = loadingDialogBoxViewFactory;
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
                _textKeyFactory.Create(TextKeysConstants.Menu.GJLoginMenu.Title));

            _gjInfoLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJLoginMenu.GJInfoLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJLoginMenu.GJInfoLabel_Text));

            _usernameLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJLoginMenu.UsernameLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJLoginMenu.UsernameLabel_Text));

            _usernameTextField = _viewRoot.Q<TextField>(NameConstants.GJLoginMenu.UsernameInputField);

            _tokenLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJLoginMenu.TokenLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJLoginMenu.TokenLabel_Text));

            _tokenTextField = _viewRoot.Q<TextField>(NameConstants.GJLoginMenu.TokenInputField);

            _loginButton = _defaultLocalizedButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.GJLoginMenu.LoginButton),
                OnLoginButtonClick,
                _textKeyFactory.Create(TextKeysConstants.Menu.GJLoginMenu.LoginButton_Text));
        }

        private void DisposeDisposableViews()
        {
            _backButton.Dispose();
            _titleLabel.Dispose();
            _gjInfoLabel.Dispose();
            _usernameLabel.Dispose();
            _tokenLabel.Dispose();
            _loginButton.Dispose();
        }

        #region BackButton

        private void OnBackButtonClick()
        {
            _framesSwitcher.SwitchBackOrToDefault(_menuUIDocumentMonoWrapper.MainMenuContainer);
        }

        #endregion

        #region LoginButton

        private void OnLoginButtonClick()
        {
            LoadingDialogBoxViewCreationData creationData = new(
                _mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LoadingBox_GJLogin_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LoadingBox_GJLogin_Message));

            _loadingDialogBoxViewFactory.Create(creationData);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, GJLoginMenuView> { }
    }
}
