using ConnectIt.Config;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Gameplay.Views
{
    public class LevelView : DefaultLocalizedLabelView
    {
        private readonly GameplayLogicConfig _gameplayLogicConfig;
        private readonly TextKey.Factory _textKeyFactory;

        public LevelView(Label timeLabel,
            GameplayLogicConfig gameplayLogicConfig,
            TextKey.Factory textKeyFactory,
            ILocalizationProvider localizationProvider) : base(timeLabel, null, localizationProvider)
        {
            _gameplayLogicConfig = gameplayLogicConfig;
            _textKeyFactory = textKeyFactory;
        }

        public override void Initialize()
        {
            textKey = _textKeyFactory.Create(
                TextKeysConstants.Gameplay.LevelLabel_Text,
                new object[] { _gameplayLogicConfig.CurrentLevel });

            base.Initialize();
        }

        public new class Factory : PlaceholderFactory<Label, LevelView> { }
    }
}
