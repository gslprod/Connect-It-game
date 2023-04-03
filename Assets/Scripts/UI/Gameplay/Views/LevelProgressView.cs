using ConnectIt.Gameplay.Observers;
using ConnectIt.Utilities.Formatters;
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
        private readonly IFormatter _formatter;

        public LevelProgressView(Custom.ProgressBar levelProgressBar,
            IGameStateObserver gameStateObserver,
            IFormatter gameplayViewConfig)
        {
            _levelProgressBar = levelProgressBar;
            _gameStateObserver = gameStateObserver;
            _formatter = gameplayViewConfig;
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
            _levelProgressBar.Title = _formatter.FormatGameplayLevelProgress(Mathf.FloorToInt(progress));
        }

        public class Factory : PlaceholderFactory<Custom.ProgressBar, LevelProgressView> { }
    }
}
