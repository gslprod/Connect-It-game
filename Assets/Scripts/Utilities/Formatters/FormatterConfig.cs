using UnityEngine;

namespace ConnectIt.Config.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FormatterConfig.asset", menuName = "Config/FormatterConfig")]
    public class FormatterConfig : ScriptableObject
    {
        public string GameplayLevelProgressTitleFormat => _gameplayLevelProgressTitleFormat;
        public string GameplayElapsedTimeFormat => _gameplayElapsedTimeFormat;
        public string SceneLoadingProgressTitleFormat => _sceneLoadingProgressTitleFormat;

        [Header("Gameplay")]

        [Tooltip("Gameplay Level Progress Title Format")]
        [SerializeField] private string _gameplayLevelProgressTitleFormat;

        [Tooltip("Gameplay Elapsed Time Format. s - sec, m - min, h - hour")]
        [SerializeField] private string _gameplayElapsedTimeFormat;

        [Header("Global")]

        [Tooltip("Scene Loading Progress Title Format")]
        [SerializeField] private string _sceneLoadingProgressTitleFormat;
    }
}
