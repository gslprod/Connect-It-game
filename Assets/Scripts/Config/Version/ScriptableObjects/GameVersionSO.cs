using UnityEngine;

namespace ConnectIt.Config.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameVersionConfig.asset", menuName = "Config/GameVersionConfig")]
    public class GameVersionSO : ScriptableObject
    {
        public string AdditionalVersionInfo => _additionalVersionInfo;

        [SerializeField] private string _additionalVersionInfo;
    }
}
