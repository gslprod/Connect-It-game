using ConnectIt.Config;
using ConnectIt.Coroutines;
using ConnectIt.Gameplay.Data;
using ConnectIt.Gameplay.Observers;
using ConnectIt.Gameplay.Pause;
using ConnectIt.Gameplay.Time;
using ConnectIt.Localization;
using ConnectIt.Scenes.Switchers;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Gameplay.MonoWrappers;
using ConnectIt.Utilities.Extensions;
using ConnectIt.Utilities.Extensions.IPauseService;
using ConnectIt.Utilities.Formatters;
using System;
using Zenject;

namespace ConnectIt.Gameplay.GameStateHandlers
{
    public class WinHandler : IInitializable, IDisposable
    {
        private readonly IGameStateObserver _gameStateObserver;
        private readonly GameplayUIDocumentMonoWrapper _gameplayUIDocumentMonoWrapper;
        private readonly IPauseService _pauseService;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly DialogBoxView.Factory _dialogBoxFactory;
        private readonly GameplayLogicConfig _gameplayLogicConfig;
        private readonly ISceneSwitcher _sceneSwitcher;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;
        private readonly ILevelsPassDataProvider _levelsPassDataProvider;
        private readonly IGameplayTimeProvider _gameplayTimeProvider;
        private readonly IFormatter _formatter;

        public WinHandler(
            IGameStateObserver gameStateObserver,
            GameplayUIDocumentMonoWrapper gameplayUIDocumentMonoWrapper,
            IPauseService pauseService,
            TextKey.Factory textKeyFactory,
            DialogBoxView.Factory dialogBoxFactory,
            GameplayLogicConfig gameplayLogicConfig,
            ISceneSwitcher sceneSwitcher,
            ICoroutinesGlobalContainer coroutinesGlobalContainer,
            ILevelsPassDataProvider levelsPassDataProvider,
            IGameplayTimeProvider gameplayTimeProvider,
            IFormatter formatter)
        {
            _gameStateObserver = gameStateObserver;
            _gameplayUIDocumentMonoWrapper = gameplayUIDocumentMonoWrapper;
            _pauseService = pauseService;
            _textKeyFactory = textKeyFactory;
            _dialogBoxFactory = dialogBoxFactory;
            _gameplayLogicConfig = gameplayLogicConfig;
            _sceneSwitcher = sceneSwitcher;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
            _levelsPassDataProvider = levelsPassDataProvider;
            _gameplayTimeProvider = gameplayTimeProvider;
            _formatter = formatter;
        }

        public void Initialize()
        {
            _gameStateObserver.GameCompleteProgressPercentsChanged += TryWin;
        }

        public void Dispose()
        {
            _gameStateObserver.GameCompleteProgressPercentsChanged -= TryWin;
        }

        private void TryWin()
        {
            if (_gameStateObserver.GameCompleteProgressPercents != 100f)
                return;

            _coroutinesGlobalContainer.DelayedAction(Win);
        }

        private void Win()
        {
            //todo
            LevelData levelData = new(_gameplayLogicConfig.CurrentLevel)
            {
                PassState = PassStates.Passed,
                Score = 0,
                PassTimeSec = _gameplayTimeProvider.ElapsedTimeSec
            };
            _levelsPassDataProvider.SaveData(levelData);

            _pauseService.SetPause(true, PauseEnablePriority.WinGameState, this);
            ShowResults();
        }

        private void ShowResults()
        {
            DialogBoxButtonInfo restartButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Gameplay.WinMenu_Restart),
                OnRestartLevelButtonClick);

            DialogBoxButtonInfo mainMenuButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Gameplay.WinMenu_MainMenu),
                OnMainMenuButtonClick,
                DialogBoxButtonType.Dismiss);

            DialogBoxButtonInfo[] buttonsInfo;
            bool nextLevelExists =
                _gameplayLogicConfig.MaxAvailableLevel > _gameplayLogicConfig.CurrentLevel;

            if (nextLevelExists)
            {
                DialogBoxButtonInfo nextLevelButtonInfo = new(
                    _textKeyFactory.Create(TextKeysConstants.Gameplay.WinMenu_NextLevel),
                    OnNextLevelButtonClick,
                    DialogBoxButtonType.Accept);

                buttonsInfo = new DialogBoxButtonInfo[]
                {
                    nextLevelButtonInfo, restartButtonInfo, mainMenuButtonInfo
                }; 
            }
            else
            {
                buttonsInfo = new DialogBoxButtonInfo[]
                {
                    restartButtonInfo, mainMenuButtonInfo
                };
            }

            //todo
            DialogBoxCreationData creationData = new(
                _gameplayUIDocumentMonoWrapper.Root,
                _textKeyFactory.Create(TextKeysConstants.Gameplay.WinMenu_Title, 
                    new object[] { _gameplayLogicConfig.CurrentLevel }),
                _textKeyFactory.Create(TextKeysConstants.Gameplay.WinMenu_Content,
                    new object[]
                    { 
                        _formatter.FormatGameplayElapsedTime(_gameplayTimeProvider.ElapsedTime),
                        99,
                        99
                    }),
                buttonsInfo);

            _dialogBoxFactory.Create(creationData);
        }

        private void OnNextLevelButtonClick()
        {
            _gameplayLogicConfig.SetCurrentLevel(_gameplayLogicConfig.CurrentLevel + 1);
            _sceneSwitcher.TryReloadActiveScene();
        }

        private void OnRestartLevelButtonClick()
        {
            _dialogBoxFactory.CreateDefaultRestartLevelDialogBox(_textKeyFactory, _gameplayUIDocumentMonoWrapper.Root, _sceneSwitcher, true);
        }

        private void OnMainMenuButtonClick()
        {
            _sceneSwitcher.TryGoToScene(Scenes.SceneType.MenuScene);
        }
    }
}
