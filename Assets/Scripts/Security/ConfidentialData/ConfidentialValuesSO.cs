using UnityEngine;

namespace ConnectIt.Security.ConfidentialData
{
    [CreateAssetMenu(fileName = "ConfidentialValues.asset", menuName = "Config/ConfidentialValues")]
    public class ConfidentialValuesSO : ScriptableObject
    {
        internal string GameJoltAPIGameKey => _gameJoltAPIGameKey;
        internal string EncryptionKey => _encryptionKey;

        [SerializeField] private string _gameJoltAPIGameKey;
        [SerializeField] private string _encryptionKey;
    }
}
