using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.CustomControls;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Extensions;
using System;
using System.Linq;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.SettingsMenu
{
    public class SettingsMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly FramesSwitcher<VisualElement> _framesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedTextElementView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly DialogBoxView.Factory _dialogBoxViewFactory;
        private readonly DefaultLocalizedButtonView.Factory _defaultLocalizedButtonViewFactory;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly OSTVolumeSliderView.Factory _ostVolumeSliderViewFactory;

        private DefaultButtonView _backButton;
        private DefaultLocalizedTextElementView _titleLabel;
        private DefaultLocalizedButtonView _languageButton;
        private OSTVolumeSliderView _ostVolumeSliderView;

        public SettingsMenuView(VisualElement viewRoot,
            VisualElement mainRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedTextElementView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            DialogBoxView.Factory dialogBoxViewFactory,
            DefaultLocalizedButtonView.Factory defaultLocalizedButtonViewFactory,
            ILocalizationProvider localizationProvider,
            OSTVolumeSliderView.Factory ostVolumeSliderViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _framesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _dialogBoxViewFactory = dialogBoxViewFactory;
            _defaultLocalizedButtonViewFactory = defaultLocalizedButtonViewFactory;
            _localizationProvider = localizationProvider;
            _ostVolumeSliderViewFactory = ostVolumeSliderViewFactory;
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
                _viewRoot.Q<Button>(NameConstants.SettingsMenu.BackButton), OnBackButtonClick);

            _titleLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.SettingsMenu.TitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.SettingsMenu.Title));

            _languageButton = _defaultLocalizedButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.SettingsMenu.LanguageButton),
                OnLanguageButtonClick,
                _textKeyFactory.Create(TextKeysConstants.Menu.SettingsMenu.LanguageButton_Text));

            _ostVolumeSliderView = _ostVolumeSliderViewFactory.Create(
                _viewRoot.Q<ProgressBarSlider>(NameConstants.SettingsMenu.MusicSlider));
        }

        private void DisposeDisposableViews()
        {
            _backButton.Dispose();
            _titleLabel.Dispose();
            _languageButton.Dispose();
            _ostVolumeSliderView.Dispose();
        }

        #region BackButton

        private void OnBackButtonClick()
        {
            _framesSwitcher.SwitchBackOrToDefault(_menuUIDocumentMonoWrapper.MainMenuContainer);
        }

        #endregion

        #region LanguageButton

        private void OnLanguageButtonClick()
        {
            int langCount = _localizationProvider.AllSupporedLanguages.Count();
            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[langCount];
            for (int i = 0; i < langCount; i++)
            {
                SupportedLanguages language = (SupportedLanguages)i + 1;

                buttonsInfo[i] = new(
                    _textKeyFactory.Create(string.Format(TextKeysConstants.Languages.Pattern, _localizationProvider.AllSupporedLanguages.ElementAt(i))),
                    () => OnLanguageButtonClick(language),
                    DialogBoxButtonType.Default);
            }

            DialogBoxButtonInfo cancelButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Common.Close),
                null,
                DialogBoxButtonType.Dismiss,
                true);

            DialogBoxCreationData creationData = new(
                _mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LanguageChange_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LanguageChange_Message),
                buttonsInfo,
                cancelButtonInfo,
                true);

            _dialogBoxViewFactory.Create(creationData);

            void OnLanguageButtonClick(SupportedLanguages language)
            {
                _localizationProvider.Language = language;
            }
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, SettingsMenuView> { }
    }
}
