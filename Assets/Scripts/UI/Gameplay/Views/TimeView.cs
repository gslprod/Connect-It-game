using ConnectIt.Gameplay.Time;
using ConnectIt.Utilities.Formatters;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Gameplay.Views
{
    public class TimeView : IInitializable, ITickable
    {
        private readonly Label _timeLabel;
        private readonly ITimeProvider _timeProvider;
        private readonly IFormatter _formatter;

        public TimeView(Label timeLabel,
            ITimeProvider gameStateObserver,
            IFormatter gameplayViewConfig)
        {
            _timeLabel = timeLabel;
            _timeProvider = gameStateObserver;
            _formatter = gameplayViewConfig;
        }

        public void Initialize()
        {
            UpdateView();
        }

        public void Tick()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _timeLabel.text = _formatter.FormatGameplayElapsedTime(
                _timeProvider.ElapsedTime);
        }

        public class Factory : PlaceholderFactory<Label, TimeView> { }
    }
}
