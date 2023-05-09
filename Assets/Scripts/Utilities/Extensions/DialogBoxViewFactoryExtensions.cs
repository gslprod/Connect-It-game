using ConnectIt.Config;
using ConnectIt.Gameplay.GameStateHandlers.GameEnd;
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
            TextKey.Factory textKeyFactory, VisualElement parent, ILevelEndHandler levelEndHandler, bool showImmediately = false)
        {
            DialogBoxButtonInfo confirmButtonInfo = new(
                textKeyFactory.Create(TextKeysConstants.Common.Confirm),
                OnConfirmRestartButtonClick,
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

            void OnConfirmRestartButtonClick()
            {
                levelEndHandler.RestartLevel();
            }
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

        public static void CreateDefaultGameEndResultsDialogBox(this DialogBoxView.Factory source,
            TextKey.Factory textKeyFactory, VisualElement parent, TextKey titleTextKey, TextKey messageTextKey, GameplayLogicConfig gameplayLogicConfig,
            ILevelEndHandler levelEndHandler, bool createNextLevelButton, bool showImmediately = false)
        {
            DialogBoxButtonInfo restartButtonInfo = new(
                textKeyFactory.Create(TextKeysConstants.Common.RestartLevel),
                OnRestartLevelButtonClick);

            DialogBoxButtonInfo mainMenuButtonInfo = new(
                textKeyFactory.Create(TextKeysConstants.Common.MainMenu),
                OnMainMenuButtonClick,
                DialogBoxButtonType.Dismiss);

            DialogBoxButtonInfo[] buttonsInfo;
            bool nextLevelExists =
                gameplayLogicConfig.MaxAvailableLevel > gameplayLogicConfig.CurrentLevel;

            if (createNextLevelButton && nextLevelExists)
            {
                DialogBoxButtonInfo nextLevelButtonInfo = new(
                    textKeyFactory.Create(TextKeysConstants.Common.NextLevel),
                    OnNextLevelButtonClick,
                    DialogBoxButtonType.Accept);

                buttonsInfo = new DialogBoxButtonInfo[]
                {
                    nextLevelButtonInfo, restartButtonInfo, mainMenuButtonInfo
                };
            }
            else
            {
                buttonsInfo = new DialogBoxButtonInfo[]
                {
                    restartButtonInfo, mainMenuButtonInfo
                };
            }

            DialogBoxCreationData creationData = new(
                parent,
                titleTextKey,
                messageTextKey,
                buttonsInfo,
                null,
                showImmediately);

            source.Create(creationData);

            void OnNextLevelButtonClick()
            {
                levelEndHandler.GoToNextLevel();
            }

            void OnRestartLevelButtonClick()
            {
                source.CreateDefaultRestartLevelDialogBox(textKeyFactory, parent, levelEndHandler, true);
            }

            void OnMainMenuButtonClick()
            {
                levelEndHandler.ExitToMainMenu();
            }
        }
    }
}
