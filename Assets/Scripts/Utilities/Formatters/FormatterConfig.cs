using UnityEngine;

namespace ConnectIt.Config.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FormatterConfig.asset", menuName = "Config/FormatterConfig")]
    public class FormatterConfig : ScriptableObject
    {
        public string GameplayLevelProgressTitleFormat => _gameplayLevelProgressTitleFormat;
        public string GameplayElapsedTimeFormat => _gameplayElapsedTimeFormat;
        public string DetailedGameplayElapsedTimeFormat => _detailedGameplayElapsedTimeFormat;
        public string SceneLoadingProgressTitleFormat => _sceneLoadingProgressTitleFormat;
        public string VersionFormat => _versionFormat;

        [Header("Gameplay")]

        [Tooltip("Gameplay Level Progress Title Format")]
        [SerializeField] private string _gameplayLevelProgressTitleFormat;

        [Tooltip("Gameplay Elapsed Time Format. f - ms, s - sec, m - min, h - hour")]
        [SerializeField] private string _gameplayElapsedTimeFormat;

        [Tooltip("Detailed Gameplay Elapsed Time Format. f - ms, s - sec, m - min, h - hour")]
        [SerializeField] private string _detailedGameplayElapsedTimeFormat;

        [Header("Global")]

        [Tooltip("Scene Loading Progress Title Format")]
        [SerializeField] private string _sceneLoadingProgressTitleFormat;

        [Header("Version")]

        [Tooltip("Version Format. 0 - application version, 1 - additional version info")]
        [SerializeField] private string _versionFormat;
    }
}
