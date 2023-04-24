using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultButtonView : IInitializable, IDisposable
    {
        protected readonly Button button;
        protected readonly Action onClick;

        public DefaultButtonView(Button button,
            Action onClick)
        {
            this.button = button;
            this.onClick = onClick;
        }

        public virtual void Initialize()
        {
            button.clicked += OnButtonClicked;
        }

        public virtual void Dispose()
        {
            button.clicked -= OnButtonClicked;
        }

        private void OnButtonClicked()
        {
            onClick?.Invoke();
        }

        public class Factory : PlaceholderFactory<Button, Action, DefaultButtonView> { }
    }
}
