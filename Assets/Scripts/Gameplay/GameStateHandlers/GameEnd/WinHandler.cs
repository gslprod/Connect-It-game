using ConnectIt.Config;
using ConnectIt.Coroutines;
using ConnectIt.Gameplay.Data;
using ConnectIt.Gameplay.Observers;
using ConnectIt.Gameplay.Time;
using ConnectIt.Gameplay.Tools.Calculators;
using ConnectIt.Localization;
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Gameplay.MonoWrappers;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using ConnectIt.Utilities.Formatters;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public class WinHandler : IWinHandler, IInitializable, IDisposable
    {
        public event Action Won;

        public float ProgressPercentsToWin
        {
            get => _progressPercentsToWin;
            set
            {
                Assert.ThatArgIs(value >= 0f, value <= 100f);

                _progressPercentsToWin = value;
                TryWin();
            }
        }

        private readonly IGameStateObserver _gameStateObserver;
        private readonly GameplayUIDocumentMonoWrapper _gameplayUIDocumentMonoWrapper;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly DialogBoxView.Factory _dialogBoxFactory;
        private readonly GameplayLogicConfig _gameplayLogicConfig;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;
        private readonly ILevelsPassDataProvider _levelsPassDataProvider;
        private readonly IGameplayTimeProvider _gameplayTimeProvider;
        private readonly IFormatter _formatter;
        private readonly ILevelEndHandler _levelEndHandler;
        private readonly IScoresCalculator _scoresCalculator;
        private readonly ICoinsCalculator _coinsCalculator;
        private readonly ICustomer _playerCustomer;

        private long _totalEarnedCoins;
        private long _gainedCoins;
        private long _score;
        private float _progressPercentsToWin = 100f;

        public WinHandler(
            IGameStateObserver gameStateObserver,
            GameplayUIDocumentMonoWrapper gameplayUIDocumentMonoWrapper,
            TextKey.Factory textKeyFactory,
            DialogBoxView.Factory dialogBoxFactory,
            GameplayLogicConfig gameplayLogicConfig,
            ICoroutinesGlobalContainer coroutinesGlobalContainer,
            ILevelsPassDataProvider levelsPassDataProvider,
            IGameplayTimeProvider gameplayTimeProvider,
            IFormatter formatter,
            ILevelEndHandler levelEndHandler,
            IScoresCalculator scoresCalculator,
            ICoinsCalculator coinsCalculator,
            ICustomer playerCustomer)
        {
            _gameStateObserver = gameStateObserver;
            _gameplayUIDocumentMonoWrapper = gameplayUIDocumentMonoWrapper;
            _textKeyFactory = textKeyFactory;
            _dialogBoxFactory = dialogBoxFactory;
            _gameplayLogicConfig = gameplayLogicConfig;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
            _levelsPassDataProvider = levelsPassDataProvider;
            _gameplayTimeProvider = gameplayTimeProvider;
            _formatter = formatter;
            _levelEndHandler = levelEndHandler;
            _scoresCalculator = scoresCalculator;
            _coinsCalculator = coinsCalculator;
            _playerCustomer = playerCustomer;
        }

        public void Initialize()
        {
            _gameStateObserver.GameCompleteProgressPercentsChanged += TryWin;
        }

        public void Dispose()
        {
            _gameStateObserver.GameCompleteProgressPercentsChanged -= TryWin;
        }

        public void Win()
        {
            _totalEarnedCoins = _coinsCalculator.Calculate();
            _score = _scoresCalculator.Calculate();

            long earnedCoinsBefore = _levelsPassDataProvider.GetDataByLevelId(_gameplayLogicConfig.CurrentLevel).TotalEarnedCoins;
            _gainedCoins = _totalEarnedCoins - earnedCoinsBefore;

            if (_gainedCoins <= 0)
                _gainedCoins = 0;
            else
                _playerCustomer.Wallet.Add(_gainedCoins);

            LevelData levelData = new(_gameplayLogicConfig.CurrentLevel)
            {
                PassState = PassStates.Passed,
                Score = _score,
                TotalEarnedCoins = _totalEarnedCoins,
                PassTimeSec = _gameplayTimeProvider.ElapsedTimeSec,
                PassLevelProgress = _gameStateObserver.GameCompleteProgressPercents,
                BoostsUsed = _gameStateObserver.AnyBoostWasUsed
            };
            _levelsPassDataProvider.SaveData(levelData);

            ShowResults();

            Won?.Invoke();
        }

        private void TryWin()
        {
            if (_gameStateObserver.GameCompleteProgressPercents < ProgressPercentsToWin)
                return;

            Win();
        }

        private void ShowResults()
        {
            TextKey titleTextKey = _textKeyFactory.Create(
                _gameStateObserver.AnyBoostWasUsed ?
                TextKeysConstants.Gameplay.WinMenu_Title_BoostsUsed :
                TextKeysConstants.Gameplay.WinMenu_Title,
                new object[]
                {
                    _gameplayLogicConfig.CurrentLevel
                });

            _dialogBoxFactory.CreateDefaultGameEndResultsDialogBox(
                _textKeyFactory,
                _gameplayUIDocumentMonoWrapper.Root,
                titleTextKey,
                _textKeyFactory.Create(TextKeysConstants.Gameplay.WinMenu_Content,
                    new object[]
                    {
                        _formatter.FormatDetailedGameplayElapsedTime(_gameplayTimeProvider.ElapsedTime),
                        Mathf.FloorToInt(_gameStateObserver.GameCompleteProgressPercents),
                        _score,
                        _gainedCoins
                    }),
                _gameplayLogicConfig,
                _levelEndHandler,
                true,
                true);
        }
    }
}
