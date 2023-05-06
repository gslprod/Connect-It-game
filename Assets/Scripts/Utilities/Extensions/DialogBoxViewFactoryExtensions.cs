using ConnectIt.Localization;
using ConnectIt.Scenes.Switchers;
using ConnectIt.UI.DialogBox;
using System;
using UnityEngine.UIElements;

namespace ConnectIt.Utilities.Extensions
{
    public static class DialogBoxViewFactoryExtensions
    {
        public static DialogBoxView CreateDefaultRestartLevelDialogBox(this DialogBoxView.Factory source,
            TextKey.Factory textKeyFactory, VisualElement parent, ISceneSwitcher sceneSwitcher, bool showImmediately = false)
        {
            DialogBoxButtonInfo confirmButtonInfo = new(
                textKeyFactory.Create(TextKeysConstants.Common.Confirm),
                () => OnConfirmRestartButtonClick(sceneSwitcher),
                DialogBoxButtonType.Accept);

            DialogBoxButtonInfo cancelButtonInfo = new(
                textKeyFactory.Create(TextKeysConstants.Common.Cancel),
                null,
                DialogBoxButtonType.Dismiss,
                true);

            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[]
            {
                confirmButtonInfo, cancelButtonInfo
            };

            DialogBoxCreationData creationData = new(
                parent,
                textKeyFactory.Create(TextKeysConstants.DialogBox.RestartLevelConfirm_Title),
                textKeyFactory.Create(TextKeysConstants.DialogBox.RestartLevelConfirm_Message),
                buttonsInfo,
                null,
                showImmediately);

            return source.Create(creationData);
        }

        private static void OnConfirmRestartButtonClick(ISceneSwitcher sceneSwitcher)
        {
            sceneSwitcher.TryReloadActiveScene();
        }

        public static DialogBoxView CreateDefaultOneButtonDialogBox(this DialogBoxView.Factory source,
            VisualElement parent, TextKey titleTextKey, TextKey messageTextKey, TextKey closeButtonTextKey, bool showImmediately = false)
        {
            DialogBoxButtonInfo cancelButtonInfo = new(
                closeButtonTextKey,
                null,
                DialogBoxButtonType.Default,
                true);

            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[]
            {
                cancelButtonInfo
            };

            DialogBoxCreationData creationData = new(
                parent,
                titleTextKey,
                messageTextKey,
                buttonsInfo,
                null,
                showImmediately);

            return source.Create(creationData);
        }

        public static DialogBoxView CreateDefaultConfirmCancelDialogBox(this DialogBoxView.Factory source,
            VisualElement parent, TextKey titleTextKey, TextKey messageTextKey, TextKey cancelButtonTextKey,
            TextKey confirmButtonTextKey, Action onConfirmClick, bool showImmediately = false)
        {
            DialogBoxButtonInfo cancelButtonInfo = new(
                cancelButtonTextKey,
                null,
                DialogBoxButtonType.Dismiss,
                true);

            DialogBoxButtonInfo confirmButtonInfo = new(
                confirmButtonTextKey,
                onConfirmClick,
                DialogBoxButtonType.Accept,
                true);

            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[]
            {
                confirmButtonInfo, cancelButtonInfo
            };

            DialogBoxCreationData creationData = new(
                parent,
                titleTextKey,
                messageTextKey,
                buttonsInfo,
                null,
                showImmediately);

            return source.Create(creationData);
        }
    }
}
