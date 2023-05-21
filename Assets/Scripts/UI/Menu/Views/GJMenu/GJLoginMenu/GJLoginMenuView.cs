using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.GJMenu.GJLoginMenu
{
    public class GJLoginMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedLabelView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly DefaultLocalizedButtonView.Factory _defaultLocalizedButtonViewFactory;
        private readonly GameJoltAPIProvider _gameJoltAPIProvider;
        private readonly LoadingDialogBoxView.Factory _loadingDialogBoxViewFactory;
        private readonly DialogBoxView.Factory _dialogBoxViewFactory;

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
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedLabelView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            DefaultLocalizedButtonView.Factory defaultLocalizedButtonViewFactory,
            GameJoltAPIProvider gameJoltAPIProvider,
            LoadingDialogBoxView.Factory loadingDialogBoxViewFactory,
            DialogBoxView.Factory dialogBoxViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _defaultLocalizedButtonViewFactory = defaultLocalizedButtonViewFactory;
            _gameJoltAPIProvider = gameJoltAPIProvider;
            _loadingDialogBoxViewFactory = loadingDialogBoxViewFactory;
            _dialogBoxViewFactory = dialogBoxViewFactory;
        }

        public void Initialize()
        {
            CreateViews();
            SetupTextFields();
        }

        public void Dispose()
        {
            DisposeDisposableViews();
        }

        private void CreateViews()
        {
            _titleLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJMenu.GJLoginMenu.TitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJLoginMenu.Title));

            _gjInfoLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJMenu.GJLoginMenu.GJInfoLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJLoginMenu.GJInfoLabel_Text));

            _usernameLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJMenu.GJLoginMenu.UsernameLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJLoginMenu.UsernameLabel_Text));

            _usernameTextField = _viewRoot.Q<TextField>(NameConstants.GJMenu.GJLoginMenu.UsernameInputField);

            _tokenLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJMenu.GJLoginMenu.TokenLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJLoginMenu.TokenLabel_Text));

            _tokenTextField = _viewRoot.Q<TextField>(NameConstants.GJMenu.GJLoginMenu.TokenInputField);

            _loginButton = _defaultLocalizedButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.GJMenu.GJLoginMenu.LoginButton),
                OnLoginButtonClick,
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJLoginMenu.LoginButton_Text));
        }

        private void SetupTextFields()
        {
            _tokenTextField.isPasswordField = true;
        }

        private void DisposeDisposableViews()
        {
            _titleLabel.Dispose();
            _gjInfoLabel.Dispose();
            _usernameLabel.Dispose();
            _tokenLabel.Dispose();
            _loginButton.Dispose();
        }

        private bool IsInputDataValid()
            =>
            !string.IsNullOrEmpty(_usernameTextField.text) &&
            !string.IsNullOrEmpty(_tokenTextField.text);

        #region LoginButton

        private void OnLoginButtonClick()
        {
            if (!IsInputDataValid())
            {
                _dialogBoxViewFactory.CreateDefaultOneButtonDialogBox(
                    _mainRoot,
                    _textKeyFactory.Create(TextKeysConstants.DialogBox.InvalidLoginInputData_Title),
                    _textKeyFactory.Create(TextKeysConstants.DialogBox.InvalidLoginInputData_Message),
                    _textKeyFactory.Create(TextKeysConstants.Common.Close),
                    true);

                return;
            }

            LoadingDialogBoxView loadingDialogBox = _loadingDialogBoxViewFactory.CreateLoadingDialogBoxView(
                _mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LoadingBox_GJLogin_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LoadingBox_GJLogin_Message));

            _gameJoltAPIProvider.LogIn(_usernameTextField.text, _tokenTextField.text, OnSignInResult);

            void OnSignInResult(bool success)
            {
                loadingDialogBox.Close();

                if (!success)
                    return;

                _tokenTextField.SetValueWithoutNotify(string.Empty);
            }
        }


        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, GJLoginMenuView> { }
    }
}
