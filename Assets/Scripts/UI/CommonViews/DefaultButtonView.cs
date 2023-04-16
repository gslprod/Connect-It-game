using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultButtonView : IInitializable, IDisposable
    {
        private readonly Button _button;
        private readonly Action _onClick;

        public DefaultButtonView(Button button,
            Action onClick)
        {
            _button = button;
            _onClick = onClick;
        }

        public void Initialize()
        {
            _button.clicked += OnButtonClicked;
        }

        public void Dispose()
        {
            _button.clicked -= OnButtonClicked;
        }

        private void OnButtonClicked()
        {
            _onClick?.Invoke();
        }

        public class Factory : PlaceholderFactory<Button, Action, DefaultButtonView> { }
    }
}
