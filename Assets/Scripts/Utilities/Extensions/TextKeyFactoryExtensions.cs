using ConnectIt.Localization;

namespace ConnectIt.Utilities.Extensions
{
    public static class TextKeyFactoryExtensions
    {
        public static TextKey Create(this TextKey.Factory source, string textKey)
        {
            return source.Create(textKey, null);
        }
    }
}
