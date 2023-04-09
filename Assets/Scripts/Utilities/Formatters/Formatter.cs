using ConnectIt.Config.ScriptableObjects;
using System;

namespace ConnectIt.Utilities.Formatters
{
    public class Formatter : IFormatter
    {
        private string _gameplayLevelProgressTitleFormat;
        private string _gameplayElapsedTimeFormat;
        private string _sceneLoadingProgressTitleFormat;

        private readonly FormatterConfig _config;

        public Formatter(FormatterConfig config)
        {
            _config = config;

            SetValuesFromSO();
        }

        public string FormatGameplayLevelProgress(float progress)
        {
            return string.Format(_gameplayLevelProgressTitleFormat, progress);
        }

        public string FormatGameplayElapsedTime(TimeSpan time)
        {
            return time.ToString(_gameplayElapsedTimeFormat);
        }

        public string FormatSceneLoadingProgress(float progress)
        {
            return string.Format(_sceneLoadingProgressTitleFormat, progress);
        }

        private void SetValuesFromSO()
        {
            _gameplayLevelProgressTitleFormat = _config.GameplayLevelProgressTitleFormat;
            _gameplayElapsedTimeFormat = _config.GameplayElapsedTimeFormat;
            _sceneLoadingProgressTitleFormat = _config.SceneLoadingProgressTitleFormat;
        }
    }
}
