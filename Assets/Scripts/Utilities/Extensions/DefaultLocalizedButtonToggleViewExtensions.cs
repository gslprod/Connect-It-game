using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.CustomControls;

namespace ConnectIt.Utilities.Extensions
{
    public static class DefaultLocalizedButtonToggleViewExtensions
    {
        public static DefaultLocalizedButtonToggleView Create(this DefaultLocalizedButtonToggleView.Factory source,
            ButtonToggle toggle, TextKey textKey)
            => source.Create(toggle, textKey, null, null);
    }
}
