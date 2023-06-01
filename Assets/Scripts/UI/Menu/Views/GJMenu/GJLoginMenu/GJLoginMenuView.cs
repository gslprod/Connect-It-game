using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.Localization;
using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.CustomControls;
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
        private readonly DefaultLocalizedTextElementView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly DefaultLocalizedButtonView.Factory _defaultLocalizedButtonViewFactory;
        private readonly GameJoltAPIProvider _gameJoltAPIProvider;
        private readonly LoadingDialogBoxView.Factory _loadingDialogBoxViewFactory;
        private readonly DialogBoxView.Factory _dialogBoxViewFactory;
        private readonly DefaultLocalizedButtonToggleView.Factory _defaultLocalizedButtonToggleViewFactory;
        private readonly IExternalServerSaveProvider _externalServerSaveProvider;

        private DefaultLocalizedTextElementView _titleLabel;
        private DefaultLocalizedTextElementView _gjInfoLabel;
        private DefaultLocalizedTextElementView _usernameLabel;
        private TextField _usernameTextField;
        private DefaultLocalizedTextElementView _tokenLabel;
        private TextField _tokenTextField;
        private DefaultLocalizedButtonView _loginButton;
        private DefaultButtonView _autoLoginInfoButton;
        private DefaultLocalizedButtonToggleView _autoLoginToggleView;

        public GJLoginMenuView(
            VisualElement viewRoot,
            VisualElement mainRoot,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedTextElementView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            DefaultLocalizedButtonView.Factory defaultLocalizedButtonViewFactory,
            GameJoltAPIProvider gameJoltAPIProvider,
            LoadingDialogBoxView.Factory loadingDialogBoxViewFactory,
            DialogBoxView.Factory dialogBoxViewFactory,
            DefaultLocalizedButtonToggleView.Factory defaultLocalizedButtonToggleViewFactory,
            IExternalServerSaveProvider externalServerSaveProvider)
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
            _defaultLocalizedButtonToggleViewFactory = defaultLocalizedButtonToggleViewFactory;
            _externalServerSaveProvider = externalServerSaveProvider;
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

            _autoLoginInfoButton = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.GJMenu.GJLoginMenu.AutoLoginInfoButton),
                OnAutoLoginInfoButtonClick);

            _autoLoginToggleView = _defaultLocalizedButtonToggleViewFactory.Create(
                _viewRoot.Q<ButtonToggle>(NameConstants.GJMenu.GJLoginMenu.AutoLoginToggle),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJLoginMenu.AutoLoginToggle_Text));
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
            _autoLoginInfoButton.Dispose();
            _autoLoginToggleView.Dispose();
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
                    _textKeyFactory,
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

                if (_autoLoginToggleView.Value)
                {
                    ExternalServerSaveData saveData = _externalServerSaveProvider.LoadExternalServerData();
                    saveData.Username = _usernameTextField.text;
                    saveData.Token = _tokenTextField.text;
                    _externalServerSaveProvider.SaveExternalServerData(saveData);
                }

                _tokenTextField.SetValueWithoutNotify(string.Empty);
            }
        }

        #endregion

        #region AutoLoginInfoButton

        private void OnAutoLoginInfoButtonClick()
        {
            _dialogBoxViewFactory.CreateDefaultOneButtonDialogBox(
                _mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.AutoLoginInfo_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.AutoLoginInfo_Message),
                _textKeyFactory,
                true);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, GJLoginMenuView> { }
    }
}
