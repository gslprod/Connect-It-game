using ConnectIt.UI.DialogBox;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Gameplay.Views
{
    public class DefaultButtonView : IInitializable, IDisposable
    {
        private readonly Button _restartButton;
        private readonly DialogBoxView.Factory _dialogBoxFactory;
        private readonly Action _onClick;

        public DefaultButtonView(Button restartButton,
            DialogBoxView.Factory dialogBoxFactory,
            Action onClick)
        {
            _restartButton = restartButton;
            _dialogBoxFactory = dialogBoxFactory;
            _onClick = onClick;
        }

        public void Initialize()
        {
            _restartButton.clicked += OnButtonClicked;
        }

        public void Dispose()
        {
            _restartButton.clicked -= OnButtonClicked;
        }

        private void OnButtonClicked()
        {
            _onClick?.Invoke();
        }

        public class Factory : PlaceholderFactory<Button, Action, DefaultButtonView> { }
    }
}
