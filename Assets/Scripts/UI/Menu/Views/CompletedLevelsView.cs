using ConnectIt.Config;
using ConnectIt.Gameplay.Data;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.Utilities.Extensions;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views
{
    public class CompletedLevelsView : DefaultLocalizedTextElementView
    {
        private readonly TextKey.Factory _textKeyFactory;
        private readonly ILevelsPassDataProvider _levelsPassDataProvider;
        private readonly GameplayLogicConfig _gameplayLogicConfig;

        public CompletedLevelsView(Label label,
            ILocalizationProvider localizationProvider,
            TextKey.Factory textKeyFactory,
            ILevelsPassDataProvider levelsPassDataProvider,
            GameplayLogicConfig gameplayLogicConfig) : base(label, null, localizationProvider)
        {
            _textKeyFactory = textKeyFactory;
            _levelsPassDataProvider = levelsPassDataProvider;
            _gameplayLogicConfig = gameplayLogicConfig;
        }

        public override void Initialize()
        {
            textKey = _textKeyFactory.Create(
                TextKeysConstants.Menu.CompletedLevelsLabel_Text,
                new object[]
                {
                    GetLastCompletedLevel(),
                    _gameplayLogicConfig.MaxAvailableLevel
                });

            _levelsPassDataProvider.LevelDataChanged += UpdateInfo;

            base.Initialize();
        }

        public override void Dispose()
        {
            _levelsPassDataProvider.LevelDataChanged -= UpdateInfo;

            base.Dispose();
        }

        private void UpdateInfo()
        {
            textKey.SetArgs(new object[]
            {
                GetLastCompletedLevel(),
                _gameplayLogicConfig.MaxAvailableLevel
            });

            UpdateLabel();
        }

        private int GetLastCompletedLevel()
            => _levelsPassDataProvider.TryGetLastCompletedLevelData(out LevelData levelData) ? levelData.Id : 0;

        public new class Factory : PlaceholderFactory<Label, CompletedLevelsView> { }
    }
}
