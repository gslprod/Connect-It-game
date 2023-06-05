using ConnectIt.Config;
using ConnectIt.Gameplay.Data;
using ConnectIt.Gameplay.Observers;
using ConnectIt.Localization;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Gameplay.MonoWrappers;
using ConnectIt.Utilities.Extensions;
using System;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public class SkipHandler : ISkipHandler
    {
        public event Action Skipped;

        private readonly ILevelEndHandler _levelEndHandler;
        private readonly GameplayLogicConfig _gameplayLogicConfig;
        private readonly ILevelsPassDataProvider _levelsPassDataProvider;
        private readonly GameplayUIDocumentMonoWrapper _gameplayUIDocumentMonoWrapper;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly DialogBoxView.Factory _dialogBoxFactory;
        private readonly IGameStateObserver _gameStateObserver;

        public SkipHandler(
            ILevelEndHandler levelEndHandler,
            GameplayLogicConfig gameplayLogicConfig,
            GameplayUIDocumentMonoWrapper gameplayUIDocumentMonoWrapper,
            TextKey.Factory textKeyFactory,
            DialogBoxView.Factory dialogBoxFactory,
            ILevelsPassDataProvider levelsPassDataProvider,
            IGameStateObserver gameStateObserver)
        {
            _levelEndHandler = levelEndHandler;
            _gameplayLogicConfig = gameplayLogicConfig;
            _gameplayUIDocumentMonoWrapper = gameplayUIDocumentMonoWrapper;
            _textKeyFactory = textKeyFactory;
            _dialogBoxFactory = dialogBoxFactory;
            _levelsPassDataProvider = levelsPassDataProvider;
            _gameStateObserver = gameStateObserver;
        }

        public void Dispose()
        {

        }

        public void Skip()
        {
            LevelData levelData = new(_gameplayLogicConfig.CurrentLevel)
            {
                PassState = PassStates.Skipped,
                BoostsUsed = _gameStateObserver.AnyBoostWasUsed
            };
            _levelsPassDataProvider.SaveData(levelData);

            ShowResults();

            Skipped?.Invoke();
        }

        private void ShowResults()
        {
            _dialogBoxFactory.CreateDefaultGameEndResultsDialogBox(
                _textKeyFactory,
                _gameplayUIDocumentMonoWrapper.Root,
                _textKeyFactory.Create(TextKeysConstants.Gameplay.SkipMenu_Title,
                    new object[] { _gameplayLogicConfig.CurrentLevel }),
                _textKeyFactory.Create(TextKeysConstants.Gameplay.SkipMenu_Content),
                _gameplayLogicConfig,
                _levelEndHandler,
                true,
                true);
        }
    }
}
