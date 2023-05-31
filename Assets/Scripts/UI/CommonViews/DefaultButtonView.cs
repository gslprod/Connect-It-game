using ConnectIt.Audio.Sounds;
using ConnectIt.Config;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultButtonView : IInitializable, IDisposable
    {
        public bool RecievesClickCallback { get; private set; } = true;

        protected readonly SoundsPlayer soundsPlayer;
        protected readonly AudioConfig audioConfig;
        protected readonly Button button;
        protected Action onClick;

        public DefaultButtonView(Button button,
            Action onClick,
            SoundsPlayer soundsPlayer,
            AudioConfig audioConfig)
        {
            this.button = button;
            this.onClick = onClick;
            this.soundsPlayer = soundsPlayer;
            this.audioConfig = audioConfig;
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

            soundsPlayer.Play(audioConfig.Click, SoundMixerGroup.UI);
        }

        public class Factory : PlaceholderFactory<Button, Action, DefaultButtonView> { }
    }
}
