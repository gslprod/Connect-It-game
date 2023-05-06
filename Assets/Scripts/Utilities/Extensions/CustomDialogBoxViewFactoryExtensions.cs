using ConnectIt.Localization;
using ConnectIt.Scenes.Switchers;
using ConnectIt.UI.DialogBox;
using UnityEngine.UIElements;

namespace ConnectIt.Utilities.Extensions
{
    public static class CustomDialogBoxViewFactoryExtensions
    {
        public static CustomDialogBoxView CreateDefaultCancelButtonDialogBox(this CustomDialogBoxView.Factory source,
            VisualElement parent, VisualTreeAsset asset, TextKey titleTextKey, TextKey cancelButtonTextKey, bool showImmediately = false)
        {
            DialogBoxButtonInfo cancelButtonInfo = new(
                cancelButtonTextKey,
                null,
                DialogBoxButtonType.Default,
                true);

            CustomDialogBoxCreationData creationData = new(
                parent,
                titleTextKey,
                asset,
                cancelButtonInfo,
                showImmediately);

            return source.Create(creationData);
        }
    }
}
