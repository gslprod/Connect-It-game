namespace ConnectIt.Security.ConfidentialData
{
    public class ConfidentialValues
    {
        internal string GameJoltAPIGameKey => _confidentialValuesSO.GameJoltAPIGameKey;

        private readonly ConfidentialValuesSO _confidentialValuesSO;

        public ConfidentialValues(
            ConfidentialValuesSO confidentialValuesSO)
        {
            _confidentialValuesSO = confidentialValuesSO;
        }
    }
}
