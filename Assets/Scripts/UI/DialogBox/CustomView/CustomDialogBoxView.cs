using ConnectIt.Coroutines;
using ConnectIt.Localization;
using System;
using UnityEngine.UIElements;
using UnityEngine;
using Zenject;
using ConnectIt.Utilities.Extensions;
using ConnectIt.Utilities;

namespace ConnectIt.UI.DialogBox
{
    public class CustomDialogBoxView : IDialogBoxView, IInitializable, IDisposable
    {
        public const string CustomDialogBoxAssetId = "CustomDialogBoxAsset";
        public const string DialogBoxButtonAssetId = "DialogBoxButtonAsset";

        public event Action<IDialogBoxView> Showing;
        public event Action<IDialogBoxView> Closing;
        public event Action<IDialogBoxView> Disposing;

        public VisualElement ContentRoot => _contentRoot;

        private readonly ILocalizationProvider _localizationProvider;
        private readonly VisualTreeAsset _uiAsset;
        private readonly VisualTreeAsset _uiButtonAsset;
        private readonly DialogBoxButton.Factory _dialogBoxButtonFactory;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;
        private CustomDialogBoxCreationData _creationData;

        private VisualElement _parent;
        private TemplateContainer _root;
        private VisualElement _elementsContainer;
        private VisualElement _contentRoot;
        private Label _titleLabel;
        private TextKey _titleKey;
        private VisualElement _content;
        private VisualTreeAsset _contentAsset;
        private DialogBoxButtonInfo _additionalBottomButtonInfo;
        private DialogBoxButton _createdButton;

        private Coroutine _delayedShowingAnimationCoroutine;
        private Coroutine _delayedClosingAnimationCoroutine;
        private Coroutine _delayedDisposeCoroutine;

        public CustomDialogBoxView(ILocalizationProvider localizationProvider,
            [Inject(Id = CustomDialogBoxAssetId)] VisualTreeAsset uiAsset,
            [Inject(Id = DialogBoxButtonAssetId)] VisualTreeAsset uiButtonAsset,
            CustomDialogBoxCreationData creationData,
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
            _additionalBottomButtonInfo = _creationData.AdditionalBottomButton;
            _content = _creationData.Content;
            _contentAsset = _creationData.ContentAsset;

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

            _titleLabel = _root.Q<Label>(TemplatesNameConstants.CustomDialogBox.TitleLabel);
            _elementsContainer = _root.Q<VisualElement>(TemplatesNameConstants.CustomDialogBox.DialogBoxContainer);
            _contentRoot = _root.Q<VisualElement>(TemplatesNameConstants.CustomDialogBox.ContentRoot);

            _elementsContainer.AddToClassList(ClassNamesConstants.Global.DialogBoxContainerClosed);

            AddContent();
            CreateAdditionalButton();
            UpdateLocalization();

            _delayedShowingAnimationCoroutine = _coroutinesGlobalContainer.DelayedAction(StartShowingAnimation);

            _titleKey.ArgsChanged += OnTitleKeyArgsChanged;
            _localizationProvider.LocalizationChanged += UpdateLocalization;

            Showing?.Invoke(this);
        }

        public void Close()
        {
            Assert.IsNull(_delayedDisposeCoroutine);

            _createdButton.ReceiveButtonCallback(false);

            _delayedClosingAnimationCoroutine = _coroutinesGlobalContainer.DelayedAction(StartClosingAnimation);

            Closing?.Invoke(this);
        }

        public void Dispose()
        {
            _delayedDisposeCoroutine = null;

            _root.RemoveFromHierarchy();

            StopRunningCoroutines();

            _createdButton.Dispose();

            _titleKey.ArgsChanged -= OnTitleKeyArgsChanged;
            _localizationProvider.LocalizationChanged -= UpdateLocalization;

            Disposing?.Invoke(this);
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
                _elementsContainer.resolvedStyle.CalculateMaxTransitionLengthSec(),
                _root.resolvedStyle.CalculateMaxTransitionLengthSec());

            _delayedDisposeCoroutine = _coroutinesGlobalContainer.DelayedAction(Dispose, closeDelaySec);
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
        }

        private void UpdateTitleLocalization()
        {
            _titleLabel.text = _titleKey.ToString();
        }

        private void OnTitleKeyArgsChanged(TextKey obj)
        {
            UpdateTitleLocalization();
        }

        private void AddContent()
        {
            if (_content != null)
            {
                _contentRoot.Add(_content);
                return;
            }

            _contentAsset.CloneTree(_contentRoot);
        }

        private void CreateAdditionalButton()
        {
            if (_additionalBottomButtonInfo == null)
                return;

            _uiButtonAsset.CloneTree(_elementsContainer);
            Button createdButton = (Button)_elementsContainer.GetLastChild();

            createdButton.AddToClassList(ClassNamesConstants.Global.DialogBoxButtonAdditional);

            _createdButton = _dialogBoxButtonFactory.Create(_additionalBottomButtonInfo, createdButton, this);

            _additionalBottomButtonInfo = null;
        }

        public class Factory : PlaceholderFactory<CustomDialogBoxCreationData, CustomDialogBoxView> { }
    }
}