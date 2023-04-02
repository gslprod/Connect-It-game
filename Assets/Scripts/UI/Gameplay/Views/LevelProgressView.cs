using ConnectIt.Config;
using ConnectIt.Gameplay.Observers;
using System;
using UnityEngine;
using Zenject;
using Custom = ConnectIt.UI.CustomControls;

namespace ConnectIt.UI.Gameplay.Views
{
    public class LevelProgressView : IInitializable, IDisposable
    {
        private readonly Custom.ProgressBar _levelProgressBar;
        private readonly IGameStateObserver _gameStateObserver;
        private readonly GameplayViewConfig _gameplayViewConfig;

        public LevelProgressView(Custom.ProgressBar levelProgressBar,
            IGameStateObserver gameStateObserver,
            GameplayViewConfig gameplayViewConfig)
        {
            _levelProgressBar = levelProgressBar;
            _gameStateObserver = gameStateObserver;
            _gameplayViewConfig = gameplayViewConfig;
        }

        public void Initialize()
        {
            _gameStateObserver.GameCompleteProgressPercentsChanged += UpdateView;

            UpdateView();
        }

        public void Dispose()
        {
            _gameStateObserver.GameCompleteProgressPercentsChanged -= UpdateView;
        }

        private void UpdateView()
        {
            float progress = _gameStateObserver.GameCompleteProgressPercents;

            _levelProgressBar.value = progress;
            _levelProgressBar.Title = string.Format(_gameplayViewConfig.LevelCompleteProgressTitleFormat, Mathf.FloorToInt(progress));
        }

        public class Factory : PlaceholderFactory<Custom.ProgressBar, LevelProgressView> { }
    }
}
