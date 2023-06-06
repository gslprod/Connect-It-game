using ConnectIt.Gameplay.Pause;
using ConnectIt.Gameplay.TutorialsSystem.Wrappers;
using ConnectIt.Localization;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Gameplay.MonoWrappers;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using ConnectIt.Utilities.Extensions.IPauseService;
using System.Collections.Generic;

namespace ConnectIt.Gameplay.TutorialsSystem
{
    public class TutorialsStarter : ITutorialsStarter
    {
        private readonly GameplayUIDocumentMonoWrapper _gameplayUIDocument;
        private readonly DialogBoxView.Factory _dialogBoxViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly IPauseService _pauseService;

        public TutorialsStarter(
            GameplayUIDocumentMonoWrapper gameplayUIDocument,
            DialogBoxView.Factory dialogBoxViewFactory,
            TextKey.Factory textKeyFactory,
            IPauseService pauseService)
        {
            _gameplayUIDocument = gameplayUIDocument;
            _dialogBoxViewFactory = dialogBoxViewFactory;
            _textKeyFactory = textKeyFactory;
            _pauseService = pauseService;
        }

        public void StartDialogBoxTutorial(DialogBoxTutorialData data)
        {
            Assert.ArgIsNotNull(data);
            Assert.ThatArgIs(data.Frames.Length > 0);

            _pauseService.SetPause(true, PauseEnablePriority.DialogBoxTutorial, this);
            ShowNextDialogBoxTutorialFrame(data, 0);
        }

        #region DialogBoxTutorial

        private void ShowNextDialogBoxTutorialFrame(DialogBoxTutorialData data, int frameIndex)
        {
            DialogBoxView createdTutorialDialogBox = null;

            List<DialogBoxButtonInfo> buttonsInfo = new();
            if (frameIndex < data.Frames.Length - 1)
            {
                buttonsInfo.Add(new(
                    _textKeyFactory.Create(TextKeysConstants.Gameplay.Tutorial.Common_NextButton_Text),
                    OnNextButtonClick,
                    DialogBoxButtonType.Default,
                    true));

                if (data.Skippable)
                {
                    buttonsInfo.Add(new(
                        _textKeyFactory.Create(TextKeysConstants.Gameplay.Tutorial.Common_SkipButton_Text),
                        OnSkipButtonClick,
                        DialogBoxButtonType.Dismiss,
                        false));
                }
            }
            else
            {
                buttonsInfo.Add(new(
                    _textKeyFactory.Create(TextKeysConstants.Gameplay.Tutorial.Common_СompleteButton_Text),
                    OnCompleteButtonClick,
                    DialogBoxButtonType.Accept,
                    true));
            }

            DialogBoxCreationData creationData = new(
                _gameplayUIDocument.Root,
                data.Frames[frameIndex].Title,
                data.Frames[frameIndex].Message,
                buttonsInfo.ToArray(),
                null,
                true);

            createdTutorialDialogBox = _dialogBoxViewFactory.Create(creationData);

            void OnSkipButtonClick()
            {
                _dialogBoxViewFactory.CreateDefaultConfirmCancelDialogBox(
                    _gameplayUIDocument.Root,
                    _textKeyFactory.Create(TextKeysConstants.Gameplay.Tutorial.ConfirmSkipDialogBox_Title),
                    _textKeyFactory.Create(TextKeysConstants.Gameplay.Tutorial.ConfirmSkipDialogBox_Message),
                    _textKeyFactory,
                    OnConfirmClick,
                    true);

                void OnConfirmClick()
                {
                    createdTutorialDialogBox.Close();

                    _pauseService.ResetPause(this);
                }
            }

            void OnNextButtonClick()
            {
                ShowNextDialogBoxTutorialFrame(data, frameIndex + 1);
            }

            void OnCompleteButtonClick()
            {
                _pauseService.ResetPause(this);
            }
        }

        #endregion
    }
}
