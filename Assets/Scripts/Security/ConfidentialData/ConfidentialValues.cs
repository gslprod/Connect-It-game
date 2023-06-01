namespace ConnectIt.Security.ConfidentialData
{
    public class ConfidentialValues
    {
        internal string GameJoltAPIGameKey => _confidentialValuesSO.GameJoltAPIGameKey;
        internal string EncryptionKey => _confidentialValuesSO.EncryptionKey;

        private readonly ConfidentialValuesSO _confidentialValuesSO;

        public ConfidentialValues(
            ConfidentialValuesSO confidentialValuesSO)
        {
            _confidentialValuesSO = confidentialValuesSO;
        }
    }
}
