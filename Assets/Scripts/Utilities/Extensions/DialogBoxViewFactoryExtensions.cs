using ConnectIt.Localization;
using ConnectIt.Scenes.Switchers;
using ConnectIt.UI.DialogBox;
using UnityEngine.UIElements;

namespace ConnectIt.Utilities.Extensions
{
    public static class DialogBoxViewFactoryExtensions
    {
        public static DialogBoxView CreateDefaultRestartLevelDialogBox(this DialogBoxView.Factory source,
            TextKey.Factory textKeyFactory, VisualElement parent, ISceneSwitcher sceneSwitcher, bool showImmediately = false)
        {
            DialogBoxButtonInfo confirmButtonInfo = new(
                textKeyFactory.Create(TextKeysConstants.Common.Confirm, null),
                () => OnConfirmRestartButtonClick(sceneSwitcher),
                DialogBoxButtonType.Accept);

            DialogBoxButtonInfo cancelButtonInfo = new(
                textKeyFactory.Create(TextKeysConstants.Common.Cancel, null),
                null,
                DialogBoxButtonType.Dismiss,
                true);

            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[]
            {
                confirmButtonInfo, cancelButtonInfo
            };

            DialogBoxCreationData creationData = new(
                parent,
                textKeyFactory.Create(TextKeysConstants.DialogBox.RestartLevelConfirm_Title, null),
                textKeyFactory.Create(TextKeysConstants.DialogBox.RestartLevelConfirm_Message, null),
                buttonsInfo,
                null,
                showImmediately);

            return source.Create(creationData);
        }

        private static void OnConfirmRestartButtonClick(ISceneSwitcher sceneSwitcher)
        {
            sceneSwitcher.TryReloadActiveScene();
        }
    }
}
