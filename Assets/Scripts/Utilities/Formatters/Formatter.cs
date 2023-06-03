using ConnectIt.Config.ScriptableObjects;
using System;

namespace ConnectIt.Utilities.Formatters
{
    public class Formatter : IFormatter
    {
        private string _gameplayLevelProgressTitleFormat => _config.GameplayLevelProgressTitleFormat;
        private string _gameplayElapsedTimeFormat => _config.GameplayElapsedTimeFormat;
        private string _detailedGameplayElapsedTimeFormat => _config.DetailedGameplayElapsedTimeFormat;
        private string _sceneLoadingProgressTitleFormat => _config.SceneLoadingProgressTitleFormat;
        private string _versionFormat => _config.VersionFormat;

        private readonly FormatterConfig _config;

        public Formatter(FormatterConfig config)
        {
            _config = config;
        }

        public string FormatGameplayLevelProgress(float progress)
        {
            return string.Format(_gameplayLevelProgressTitleFormat, progress);
        }

        public string FormatGameplayElapsedTime(TimeSpan time)
        {
            return time.ToString(_gameplayElapsedTimeFormat);
        }

        public string FormatDetailedGameplayElapsedTime(TimeSpan time)
        {
            return time.ToString(_detailedGameplayElapsedTimeFormat);
        }

        public string FormatSceneLoadingProgress(float progress)
        {
            return string.Format(_sceneLoadingProgressTitleFormat, progress);
        }

        public string FormatVersion(string applicationVersion, string additionalVersionInfo)
        {
            return string.Format(_versionFormat, applicationVersion, additionalVersionInfo);
        }
    }
}
