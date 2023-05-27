using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.MainMenu
{
    public class MainMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly FramesSwitcher<VisualElement> _framesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DialogBoxView.Factory _dialogBoxViewFactory;
        private readonly TextKey.Factory _textKeyFactory;

        private DefaultButtonView _playButtonView;
        private DefaultButtonView _settingsButtonView;
        private DefaultButtonView _shopButtonView;
        private DefaultButtonView _gjApiButtonView;
        private DefaultButtonView _statsButtonView;
        private DefaultButtonView _exitButtonView;

        public MainMenuView(
            VisualElement viewRoot,
            VisualElement mainRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DialogBoxView.Factory dialogBoxViewFactory,
            TextKey.Factory textKeyFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _framesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _dialogBoxViewFactory = dialogBoxViewFactory;
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
            _playButtonView = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.MainMenu.PlayButton), OnPlayButtonClick);

            _settingsButtonView = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.MainMenu.SettingsButton), OnSettingsButtonClick);

            _shopButtonView = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.MainMenu.ShopButton), OnShopButtonClick);

            _gjApiButtonView = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.MainMenu.GjApiButton), OnGjApiButtonClick);

            _statsButtonView = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.MainMenu.StatsButton), OnStatsButtonClick);

            _exitButtonView = _defaultButtonViewFactory.Create
                (_viewRoot.Q<Button>(NameConstants.MainMenu.ExitButton), OnExitButtonClick);
        }

        private void DisposeDisposableViews()
        {
            _playButtonView.Dispose();
            _settingsButtonView.Dispose();
            _shopButtonView.Dispose();
            _gjApiButtonView.Dispose();
            _statsButtonView.Dispose();
            _exitButtonView.Dispose();
        }

        #region PlayButton

        private void OnPlayButtonClick()
        {
            _framesSwitcher.SwitchTo(_menuUIDocumentMonoWrapper.SelectLevelContainer);
        }

        #endregion

        #region SettingsButton

        private void OnSettingsButtonClick()
        {
            _framesSwitcher.SwitchTo(_menuUIDocumentMonoWrapper.SettingsContainer);
        }

        #endregion

        #region ShopButton

        private void OnShopButtonClick()
        {
            _framesSwitcher.SwitchTo(_menuUIDocumentMonoWrapper.ShopContainer);
        }

        #endregion

        #region GjApiButton

        private void OnGjApiButtonClick()
        {
            _framesSwitcher.SwitchTo(_menuUIDocumentMonoWrapper.GJContainer);
        }

        #endregion

        #region StatsButton

        private void OnStatsButtonClick()
        {
            _framesSwitcher.SwitchTo(_menuUIDocumentMonoWrapper.StatsContainer);
        }

        #endregion

        #region ExitButton

        private void OnExitButtonClick()
        {
            _dialogBoxViewFactory.CreateDefaultConfirmCancelDialogBox(
                _mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.ConfirmGameExit_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.ConfirmGameExit_Message),
                _textKeyFactory,
                OnConfirmExitClick,
                true);
        }

        private void OnConfirmExitClick()
        {
            Application.Quit();
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, MainMenuView> { }
    }
}