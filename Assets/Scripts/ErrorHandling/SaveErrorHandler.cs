using ConnectIt.Coroutines;
using ConnectIt.Localization;
using ConnectIt.Save.SaveProviders;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Global.MonoWrappers;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine;
using Zenject;

namespace ConnectIt.ErrorHandling
{
    public class SaveErrorHandler : IInitializable, IDisposable
    {
        private readonly GameSaveProvider _saveProvider;
        private readonly GlobalUIDocumentMonoWrapper _globalUIDocument;
        private readonly DialogBoxView.Factory _dialogBoxViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;

        private bool _loadErrorDetected = false;
        private bool _saveErrorDetected = false;

        public SaveErrorHandler(
            GameSaveProvider saveProvider,
            GlobalUIDocumentMonoWrapper globalUIDocument,
            DialogBoxView.Factory dialogBoxViewFactory,
            TextKey.Factory textKeyFactory,
            ICoroutinesGlobalContainer coroutinesGlobalContainer)
        {
            _saveProvider = saveProvider;
            _globalUIDocument = globalUIDocument;
            _dialogBoxViewFactory = dialogBoxViewFactory;
            _textKeyFactory = textKeyFactory;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
        }

        public void Initialize()
        {
            _saveProvider.LoadFail += OnSaveProviderLoadFail;
            _saveProvider.SaveFail += OnSaveProviderSaveFail;
        }

        public void Dispose()
        {
            _saveProvider.LoadFail -= OnSaveProviderLoadFail;
            _saveProvider.SaveFail -= OnSaveProviderSaveFail;
        }

        #region LoadErrorHadling

        private void OnSaveProviderLoadFail(Type saveDataType, Exception ex)
        {
            if (_loadErrorDetected)
                return;

            _loadErrorDetected = true;

            _coroutinesGlobalContainer.DelayedAction(() => CreateLoadErrorWarnDialogBox(saveDataType, ex));
        }

        private void CreateLoadErrorWarnDialogBox(Type saveDataType, Exception ex)
        {
            DialogBoxButtonInfo doNothingButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.DialogBox.SaveLoadError_DoNothingButton_Text),
                OnDoNothingButtonClick,
                DialogBoxButtonType.Default,
                false);

            DialogBoxButtonInfo eraseDataButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.DialogBox.SaveLoadError_EraseDataButton_Text),
                OnEraseDataButtonClick,
                DialogBoxButtonType.Dismiss,
                false);

            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[]
            {
                eraseDataButtonInfo, doNothingButtonInfo
            };

            DialogBoxCreationData creationData = new(
                _globalUIDocument.Root,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.SaveLoadError_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.SaveLoadError_Message,
                new object[]
                {
                    $"{saveDataType.Name} ({ex.GetType().Name} - {ex.Message})"
                }),
                buttonsInfo,
                null,
                true); ;

            _dialogBoxViewFactory.Create(creationData);
        }

        private void OnDoNothingButtonClick()
        {
            Application.Quit();
        }

        private void OnEraseDataButtonClick()
        {
            _dialogBoxViewFactory.CreateDefaultConfirmCancelDialogBox(
                _globalUIDocument.Root,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.ConfirmSaveErasing_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.ConfirmSaveErasing_Message),
                _textKeyFactory,
                OnEraseDataConfirmButtonClick,
                true);
        }

        private void OnEraseDataConfirmButtonClick()
        {
            _saveProvider.ResetSaveData(true);

            Application.Quit();
        }

        #endregion

        #region SaveErrorHadling

        private void OnSaveProviderSaveFail(Type saveDataType, Exception ex)
        {
            if (_saveErrorDetected || _loadErrorDetected)
                return;

            _saveErrorDetected = true;

            _coroutinesGlobalContainer.DelayedAction(() => CreateSaveErrorWarnDialogBox(saveDataType, ex));
        }

        private void CreateSaveErrorWarnDialogBox(Type saveDataType, Exception ex)
        {
            _dialogBoxViewFactory.CreateDefaultOneButtonDialogBox(
                _globalUIDocument.Root,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.DataSaveError_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.DataSaveError_Message,
                new object[]
                {
                    $"{saveDataType.Name} ({ex.GetType().Name} - {ex.Message})"
                }),
                _textKeyFactory,
                true);
        }

        #endregion
    }
}
