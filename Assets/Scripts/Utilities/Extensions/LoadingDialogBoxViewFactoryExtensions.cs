using ConnectIt.Localization;
using ConnectIt.UI.DialogBox;
using UnityEngine.UIElements;

namespace ConnectIt.Utilities.Extensions
{
    public static class LoadingDialogBoxViewFactoryExtensions
    {
        public static LoadingDialogBoxView CreateLoadingDialogBoxView(this LoadingDialogBoxView.Factory source,
            VisualElement parent, TextKey titleKey, TextKey messageKey, DialogBoxButtonInfo bottomButton = null,
            bool showImmediately = true)
        {
            LoadingDialogBoxViewCreationData creationData = new(
                parent,
                titleKey,
                messageKey,
                bottomButton,
                showImmediately);

            return source.Create(creationData);
        }
    }
}
