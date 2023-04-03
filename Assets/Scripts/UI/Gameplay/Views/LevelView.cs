using ConnectIt.Config;
using ConnectIt.Localization;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Gameplay.Views
{
    public class LevelView : IInitializable, IDisposable
    {
        private readonly Label _timeLabel;
        private readonly GameplayLogicConfig _gameplayLogicConfig;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly TextKey.Factory _textKeyFactory;

        private TextKey _levelTextKey;

        public LevelView(Label timeLabel,
            GameplayLogicConfig gameplayLogicConfig,
            ILocalizationProvider localizationProvider,
            TextKey.Factory textKeyFactory)
        {
            _timeLabel = timeLabel;
            _gameplayLogicConfig = gameplayLogicConfig;
            _localizationProvider = localizationProvider;
            _textKeyFactory = textKeyFactory;
        }

        public void Initialize()
        {
            _levelTextKey = _textKeyFactory.Create(
                TextKeysConstants.Gameplay.Level,
                new object[] { _gameplayLogicConfig.CurrentLevel });

            _localizationProvider.LocalizationChanged += OnLocalizationChanged;
            
            UpdateView();
        }

        public void Dispose()
        {
            _localizationProvider.LocalizationChanged -= OnLocalizationChanged;
        }

        private void OnLocalizationChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _timeLabel.text = _levelTextKey.ToString();
        }

        public class Factory : PlaceholderFactory<Label, LevelView> { }
    }
}
