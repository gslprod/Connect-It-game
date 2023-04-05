using ConnectIt.Localization;
using ConnectIt.Utilities;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.DialogBox
{
    public class DialogBoxButton : IInitializable, IDisposable
    {
        private readonly ILocalizationProvider _localizationProvider;
        private readonly Button _button;
        private readonly DialogBoxView _dialogBoxView;
        private DialogBoxButtonInfo _buttonInfo;

        private TextKey _titleKey;
        private Action _onClick;

        public DialogBoxButton(ILocalizationProvider localizationProvider,
            DialogBoxButtonInfo buttonInfo,
            Button button,
            DialogBoxView dialogBoxView)
        {
            _localizationProvider = localizationProvider;
            _buttonInfo = buttonInfo;
            _button = button;
            _dialogBoxView = dialogBoxView;
        }

        public void Initialize()
        {
            _titleKey = _buttonInfo.TitleKey;
            _onClick = _buttonInfo.OnClick;

            if (_buttonInfo.ClosesDialogBox)
                _onClick += () => _dialogBoxView.Close();

            SetupButtonView();
            UpdateLocalization();

            _buttonInfo = null;

            _button.clicked += OnButtonClick;

            _localizationProvider.LocalizationChanged += UpdateLocalization;
            _titleKey.ArgsChanged += OnTitleKeyArgsChanged;
        }

        public void Dispose()
        {
            _button.clicked -= OnButtonClick;

            _localizationProvider.LocalizationChanged -= UpdateLocalization;
            _titleKey.ArgsChanged -= OnTitleKeyArgsChanged;
        }

        private void SetupButtonView()
        {
            if (_buttonInfo.Type == DialogBoxButtonType.Default)
                return;

            string classToAdd = _buttonInfo.Type switch
            {
                DialogBoxButtonType.Accept => ClassNamesConstants.DialogBoxButtonAccept,
                DialogBoxButtonType.Dismiss => ClassNamesConstants.DialogBoxButtonDismiss,

                _ => throw Assert.GetFailException(),
            };

            _button.AddToClassList(classToAdd);
        }

        private void OnButtonClick()
        {
            _onClick?.Invoke();
        }

        private void OnTitleKeyArgsChanged(TextKey obj)
        {
            UpdateLocalization();
        }

        private void UpdateLocalization()
        {
            _button.text = _titleKey.ToString();
        }

        public class Factory : PlaceholderFactory<DialogBoxButtonInfo, Button, DialogBoxView, DialogBoxButton> { }
    }
}
