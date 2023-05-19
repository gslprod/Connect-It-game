using UnityEngine;

namespace ConnectIt.Security.ConfidentialData
{
    [CreateAssetMenu(fileName = "ConfidentialValues.asset", menuName = "Config/ConfidentialValues")]
    public class ConfidentialValuesSO : ScriptableObject
    {
        internal string GameJoltAPIGameKey => _gameJoltAPIGameKey;

        [SerializeField] private string _gameJoltAPIGameKey;
    }
}
