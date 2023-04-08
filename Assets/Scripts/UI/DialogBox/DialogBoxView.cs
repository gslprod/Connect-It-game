using ConnectIt.Coroutines;
using ConnectIt.Localization;
using ConnectIt.Utilities;
using System;
using System.Collections;
using System.Linq;
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
        private TextKey _titleKey;
        private TextKey _messageKey;
        private DialogBoxButtonInfo[] _buttonsInfo;

        private DialogBoxButton[] _createdButtons;
        private Label _titleLabel;
        private Label _messageLabel;

        private Coroutine _appearAnimationCoroutine;
        private Coroutine _disposeCoroutine;

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

            if (_creationData.ShowImmediately)
                Show();

            _creationData = null;
        }

        public void Show()
        {
            Assert.That(_root == null);

            _root = _uiAsset.CloneTree();
            _parent.Add(_root);
            _root.AddToClassList(ClassNamesConstants.DialogBoxRoot);
            _root.AddToClassList(ClassNamesConstants.DialogBoxRootClosed);

            _titleLabel = _root.Q<Label>(TitleLabelName);
            _messageLabel = _root.Q<Label>(MessageLabelName);
            _elementsContainer = _root.Q<VisualElement>(DialogBoxContainerName);

            CreateButtons();
            UpdateLocalization();

            _appearAnimationCoroutine = _coroutinesGlobalContainer.StartAndRegisterCoroutine(WaitOneFrameAndStartAppearAnimation());

            _titleKey.ArgsChanged += OnTitleKeyArgsChanged;
            _messageKey.ArgsChanged += OnMessageKeyArgsChanged;
            _localizationProvider.LocalizationChanged += UpdateLocalization;

            Showing?.Invoke(this);
        }

        public void Close()
        {
            Assert.IsNull(_disposeCoroutine);

            _elementsContainer.AddToClassList(ClassNamesConstants.DialogBoxContainerClosed);
            _root.AddToClassList(ClassNamesConstants.DialogBoxRootClosed);

            foreach (var button in _createdButtons)
                button.ReceiveButtonCallback(false);

            float closeDelaySec =
                _elementsContainer.resolvedStyle.transitionDuration
                .Concat(_root.resolvedStyle.transitionDuration)
                .Max(
                timeValue =>
                {
                    return timeValue.unit switch
                    {
                        TimeUnit.Second => timeValue.value,
                        TimeUnit.Millisecond => timeValue.value / 1000,

                        _ => throw Assert.GetFailException(),
                    };
                });

            _disposeCoroutine = _coroutinesGlobalContainer.StartAndRegisterCoroutine(WaitForDelayAndDisposeCoroutine(closeDelaySec));

            Closing?.Invoke(this);
        }

        public void Dispose()
        {
            _root.RemoveFromHierarchy();

            foreach (var button in _createdButtons)
                button.Dispose();

            _titleKey.ArgsChanged -= OnTitleKeyArgsChanged;
            _messageKey.ArgsChanged -= OnMessageKeyArgsChanged;
            _localizationProvider.LocalizationChanged -= UpdateLocalization;
        }

        private IEnumerator WaitOneFrameAndStartAppearAnimation()
        {
            yield return null;

            _elementsContainer.RemoveFromClassList(ClassNamesConstants.DialogBoxContainerClosed);
            _root.RemoveFromClassList(ClassNamesConstants.DialogBoxRootClosed);

            _coroutinesGlobalContainer.StopAndUnregisterCoroutine(_appearAnimationCoroutine);
        }

        private IEnumerator WaitForDelayAndDisposeCoroutine(float delaySec)
        {
            yield return new WaitForSeconds(delaySec);

            Dispose();
            _coroutinesGlobalContainer.StopAndUnregisterCoroutine(_disposeCoroutine);
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
            _createdButtons = new DialogBoxButton[_buttonsInfo.Length];

            for (int i = 0; i < _buttonsInfo.Length; i++)
            {
                TemplateContainer createdButtonAsset = _uiButtonAsset.CloneTree();
                buttonsParent.Add(createdButtonAsset);

                Button button =  createdButtonAsset.Q<Button>();

                _createdButtons[i] = _dialogBoxButtonFactory.Create(_buttonsInfo[i], button, this);
            }

            _buttonsInfo = null;
        }

        public class Factory : PlaceholderFactory<DialogBoxCreationData, DialogBoxView> { }
    }
}
