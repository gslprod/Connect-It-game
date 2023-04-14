using ConnectIt.Coroutines;
using ConnectIt.Localization;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.DialogBox
{
    public class DialogBoxView : IInitializable, IDisposable
    {
        public const string DialogBoxAssetId = "DialogBoxAsset";
        public const string DialogBoxButtonAssetId = "DialogBoxButtonAsset";
        public const string DialogBoxContainerName = "dialog-box-container";
        public const string DialogBoxButtonParentName = "buttons-group";
        public const string TitleLabelName = "title-label";
        public const string MessageLabelName = "message-label";

        public event Action<DialogBoxView> Showing;
        public event Action<DialogBoxView> Closing;

        private readonly ILocalizationProvider _localizationProvider;
        private readonly VisualTreeAsset _uiAsset;
        private readonly VisualTreeAsset _uiButtonAsset;
        private readonly DialogBoxButton.Factory _dialogBoxButtonFactory;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;
        private DialogBoxCreationData _creationData;

        private VisualElement _parent;
        private TemplateContainer _root;
        private VisualElement _elementsContainer;
        private Label _titleLabel;
        private Label _messageLabel;
        private TextKey _titleKey;
        private TextKey _messageKey;
        private DialogBoxButtonInfo[] _buttonsInfo;
        private DialogBoxButtonInfo _additionalBottomButtonInfo;
        private DialogBoxButton[] _createdButtons;

        private Coroutine _delayedShowingAnimationCoroutine;
        private Coroutine _delayedClosingAnimationCoroutine;
        private Coroutine _delayedDisposeCoroutine;

        public DialogBoxView(ILocalizationProvider localizationProvider,
            [Inject(Id = DialogBoxAssetId)] VisualTreeAsset uiAsset,
            [Inject(Id = DialogBoxButtonAssetId)] VisualTreeAsset uiButtonAsset,
            DialogBoxCreationData creationData,
            DialogBoxButton.Factory dialogBoxButtonFactory,
            ICoroutinesGlobalContainer coroutinesGlobalContainer)
        {
            _localizationProvider = localizationProvider;
            _uiAsset = uiAsset;
            _uiButtonAsset = uiButtonAsset;
            _creationData = creationData;
            _dialogBoxButtonFactory = dialogBoxButtonFactory;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
        }

        public void Initialize()
        {
            _parent = _creationData.Parent;
            _titleKey = _creationData.TitleKey;
            _messageKey = _creationData.MessageKey;
            _buttonsInfo = _creationData.Buttons;
            _additionalBottomButtonInfo = _creationData.AdditionalBottomButton;

            if (_creationData.ShowImmediately)
                Show();

            _creationData = null;
        }

        public void Show()
        {
            Assert.That(_root == null);

            _root = _uiAsset.CloneTree();
            _parent.Add(_root);
            _root.AddToClassList(ClassNamesConstants.Global.DialogBoxRoot);
            _root.AddToClassList(ClassNamesConstants.Global.DialogBoxRootClosed);

            _titleLabel = _root.Q<Label>(TitleLabelName);
            _messageLabel = _root.Q<Label>(MessageLabelName);
            _elementsContainer = _root.Q<VisualElement>(DialogBoxContainerName);

            _elementsContainer.AddToClassList(ClassNamesConstants.Global.DialogBoxContainerClosed);

            CreateButtons();
            CreateAdditionalButton();
            UpdateLocalization();

            _delayedShowingAnimationCoroutine = _coroutinesGlobalContainer.DelayedAction(StartShowingAnimation);

            _titleKey.ArgsChanged += OnTitleKeyArgsChanged;
            _messageKey.ArgsChanged += OnMessageKeyArgsChanged;
            _localizationProvider.LocalizationChanged += UpdateLocalization;

            Showing?.Invoke(this);
        }

        public void Close()
        {
            Assert.IsNull(_delayedDisposeCoroutine);

            foreach (var button in _createdButtons)
                button.ReceiveButtonCallback(false);

            _delayedClosingAnimationCoroutine = _coroutinesGlobalContainer.DelayedAction(StartClosingAnimation);

            Closing?.Invoke(this);
        }

        public void Dispose()
        {
            _delayedDisposeCoroutine = null;

            _root.RemoveFromHierarchy();

            StopRunningCoroutines();

            foreach (var button in _createdButtons)
                button.Dispose();

            _titleKey.ArgsChanged -= OnTitleKeyArgsChanged;
            _messageKey.ArgsChanged -= OnMessageKeyArgsChanged;
            _localizationProvider.LocalizationChanged -= UpdateLocalization;
        }

        private void StartShowingAnimation()
        {
            _delayedShowingAnimationCoroutine = null;

            _elementsContainer.RemoveFromClassList(ClassNamesConstants.Global.DialogBoxContainerClosed);
            _root.RemoveFromClassList(ClassNamesConstants.Global.DialogBoxRootClosed);
        }

        private void StartClosingAnimation()
        {
            _delayedClosingAnimationCoroutine = null;

            _elementsContainer.AddToClassList(ClassNamesConstants.Global.DialogBoxContainerClosed);
            _root.AddToClassList(ClassNamesConstants.Global.DialogBoxRootClosed);

            float closeDelaySec = Mathf.Max(
                _elementsContainer.resolvedStyle.CalculateMaxTransitionLength(),
                _root.resolvedStyle.CalculateMaxTransitionLength());

            _delayedDisposeCoroutine = _coroutinesGlobalContainer.DelayedAction(Dispose, new WaitForSeconds(closeDelaySec));
        }

        private void StopRunningCoroutines()
        {
            if (_delayedDisposeCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_delayedDisposeCoroutine);

            if (_delayedClosingAnimationCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_delayedClosingAnimationCoroutine);

            if (_delayedShowingAnimationCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_delayedShowingAnimationCoroutine);
        }

        private void UpdateLocalization()
        {
            UpdateTitleLocalization();
            UpdateMessageLocalization();
        }

        private void UpdateMessageLocalization()
        {
            _messageLabel.text = _messageKey.ToString();
        }

        private void UpdateTitleLocalization()
        {
            _titleLabel.text = _titleKey.ToString();
        }

        private void OnTitleKeyArgsChanged(TextKey obj)
        {
            UpdateTitleLocalization();
        }

        private void OnMessageKeyArgsChanged(TextKey obj)
        {
            UpdateMessageLocalization();
        }

        private void CreateButtons()
        {
            if (_buttonsInfo == null || _buttonsInfo.Length == 0)
                return;

            VisualElement buttonsParent = _root.Q<VisualElement>(DialogBoxButtonParentName);
            _createdButtons = new DialogBoxButton[GetButtonsAmount()];

            for (int i = 0; i < _buttonsInfo.Length; i++)
            {
                _uiButtonAsset.CloneTree(buttonsParent);
                Button createdButton = (Button)buttonsParent[buttonsParent.childCount - 1];

                if (i < _buttonsInfo.Length - 1)
                    createdButton.AddToClassList(ClassNamesConstants.Global.DialogBoxButtonNotLastInLayout);

                _createdButtons[i] = _dialogBoxButtonFactory.Create(_buttonsInfo[i], createdButton, this);
            }

            _buttonsInfo = null;
        }

        private void CreateAdditionalButton()
        {
            if (_additionalBottomButtonInfo == null)
                return;

            _uiButtonAsset.CloneTree(_elementsContainer);
            Button createdButton = (Button)_elementsContainer[_elementsContainer.childCount - 1];

            createdButton.AddToClassList(ClassNamesConstants.Global.DialogBoxButtonAdditional);

            _createdButtons[^1] = _dialogBoxButtonFactory.Create(_additionalBottomButtonInfo, createdButton, this);

            _additionalBottomButtonInfo = null;
        }

        private int GetButtonsAmount()
        {
            int buttonsAmount = _buttonsInfo == null ? 0 : _buttonsInfo.Length;
            buttonsAmount += _additionalBottomButtonInfo == null ? 0 : 1;

            return buttonsAmount;
        }

        public class Factory : PlaceholderFactory<DialogBoxCreationData, DialogBoxView> { }
    }
}
