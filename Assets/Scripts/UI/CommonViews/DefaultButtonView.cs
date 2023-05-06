using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultButtonView : IInitializable, IDisposable
    {
        public bool RecievesClickCallback { get; private set; } = true;

        protected readonly Button button;
        protected Action onClick;

        public DefaultButtonView(Button button,
            Action onClick)
        {
            this.button = button;
            this.onClick = onClick;
        }

        public virtual void Initialize()
        {
            SubscribeToClickCallback();
        }

        public virtual void Dispose()
        {
            UnsubscribeFromClickCallback();
        }

        public void RecieveClickCallback(bool recieve)
        {
            if (recieve == RecievesClickCallback)
                return;

            RecievesClickCallback = recieve;

            if (recieve)
                SubscribeToClickCallback();
            else
                UnsubscribeFromClickCallback();
        }

        public void SetOnClick(Action onClick)
        {
            this.onClick = onClick;
        }

        private void SubscribeToClickCallback()
        {
            button.clicked += OnButtonClicked;
        }

        private void UnsubscribeFromClickCallback()
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
