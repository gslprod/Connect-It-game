using ConnectIt.Coroutines;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

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

        private readonly VisualTreeAsset _uiAsset;
        private readonly VisualTreeAsset _uiButtonAsset;
        private readonly DialogBoxButton.Factory _dialogBoxButtonFactory;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;
        private readonly DefaultLocalizedLabelView.Factory _defaultLocalizedLabelViewFactory;
        private CustomDialogBoxCreationData _creationData;

        private VisualElement _parent;
        private TemplateContainer _root;
        private VisualElement _elementsContainer;
        private VisualElement _contentRoot;
        private DefaultLocalizedLabelView _titleLabel;
        private TextKey _titleKey;
        private VisualElement _content;
        private VisualTreeAsset _contentAsset;
        private DialogBoxButtonInfo _additionalBottomButtonInfo;
        private DialogBoxButton _createdButton;
        private string _additionalDialogBoxRootClass;
        private string _additionalDialogBoxContainerClass;
        private string _additionalDialogBoxRootClosedClass;
        private string _additionalDialogBoxContainerClosedClass;

        private TransitionsStopWaiter _transitionsStopWaiter = new();
        private Coroutine _delayedShowingAnimationCoroutine;
        private Coroutine _delayedClosingAnimationCoroutine;

        public CustomDialogBoxView(
            [Inject(Id = CustomDialogBoxAssetId)] VisualTreeAsset uiAsset,
            [Inject(Id = DialogBoxButtonAssetId)] VisualTreeAsset uiButtonAsset,
            CustomDialogBoxCreationData creationData,
            DialogBoxButton.Factory dialogBoxButtonFactory,
            ICoroutinesGlobalContainer coroutinesGlobalContainer,
            DefaultLocalizedLabelView.Factory defaultLocalizedLabelViewFactory)
        {
            _uiAsset = uiAsset;
            _uiButtonAsset = uiButtonAsset;
            _creationData = creationData;
            _dialogBoxButtonFactory = dialogBoxButtonFactory;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
            _defaultLocalizedLabelViewFactory = defaultLocalizedLabelViewFactory;
        }

        public void Initialize()
        {
            _parent = _creationData.Parent;
            _titleKey = _creationData.TitleKey;
            _additionalBottomButtonInfo = _creationData.AdditionalBottomButton;
            _content = _creationData.Content;
            _contentAsset = _creationData.ContentAsset;
            _additionalDialogBoxRootClass = _creationData.AdditionalDialogBoxRootClass;
            _additionalDialogBoxRootClosedClass = _creationData.AdditionalDialogBoxRootClosedClass;
            _additionalDialogBoxContainerClass = _creationData.AdditionalDialogBoxContainerClass;
            _additionalDialogBoxContainerClosedClass = _creationData.AdditionalDialogBoxContainerClosedClass;

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

            if (!string.IsNullOrEmpty(_additionalDialogBoxRootClass))
                _root.AddToClassList(_additionalDialogBoxRootClass);
            if (!string.IsNullOrEmpty(_additionalDialogBoxRootClosedClass))
                _root.AddToClassList(_additionalDialogBoxRootClosedClass);

            _titleLabel = _defaultLocalizedLabelViewFactory.Create(
                _root.Q<Label>(TemplatesNameConstants.CustomDialogBox.TitleLabel),
                _titleKey);

            _elementsContainer = _root.Q<VisualElement>(TemplatesNameConstants.CustomDialogBox.DialogBoxContainer);
            _contentRoot = _root.Q<VisualElement>(TemplatesNameConstants.CustomDialogBox.ContentRoot);

            _elementsContainer.AddToClassList(ClassNamesConstants.Global.DialogBoxContainerClosed);

            if (!string.IsNullOrEmpty(_additionalDialogBoxContainerClass))
                _elementsContainer.AddToClassList(_additionalDialogBoxContainerClass);
            if (!string.IsNullOrEmpty(_additionalDialogBoxContainerClosedClass))
                _elementsContainer.AddToClassList(_additionalDialogBoxContainerClosedClass);

            AddContent();
            CreateAdditionalButton();

            _delayedShowingAnimationCoroutine = _coroutinesGlobalContainer.DelayedAction(StartShowingAnimation);

            Showing?.Invoke(this);
        }

        public void Close()
        {
            _createdButton?.ReceiveButtonCallback(false);

            _delayedClosingAnimationCoroutine = _coroutinesGlobalContainer.DelayedAction(StartClosingAnimation);

            Closing?.Invoke(this);
        }

        public void Dispose()
        {
            _root.RemoveFromHierarchy();

            StopRunningCoroutines();
            _transitionsStopWaiter.AbortIfWaiting();

            _titleLabel.Dispose();
            _createdButton?.Dispose();

            Disposing?.Invoke(this);
        }

        private void StartShowingAnimation()
        {
            _delayedShowingAnimationCoroutine = null;

            _elementsContainer.RemoveFromClassList(ClassNamesConstants.Global.DialogBoxContainerClosed);
            _root.RemoveFromClassList(ClassNamesConstants.Global.DialogBoxRootClosed);

            if (!string.IsNullOrEmpty(_additionalDialogBoxContainerClosedClass))
                _elementsContainer.RemoveFromClassList(_additionalDialogBoxContainerClosedClass);
            if (!string.IsNullOrEmpty(_additionalDialogBoxRootClosedClass))
                _root.RemoveFromClassList(_additionalDialogBoxRootClosedClass);
        }

        private void StartClosingAnimation()
        {
            _delayedClosingAnimationCoroutine = null;

            _elementsContainer.AddToClassList(ClassNamesConstants.Global.DialogBoxContainerClosed);
            _root.AddToClassList(ClassNamesConstants.Global.DialogBoxRootClosed);

            if (!string.IsNullOrEmpty(_additionalDialogBoxContainerClosedClass))
                _elementsContainer.AddToClassList(_additionalDialogBoxContainerClosedClass);
            if (!string.IsNullOrEmpty(_additionalDialogBoxRootClosedClass))
                _root.AddToClassList(_additionalDialogBoxRootClosedClass);

            _coroutinesGlobalContainer.DelayedAction(() => _transitionsStopWaiter.AbortCurrentAndWait(Dispose, _elementsContainer, _root));
        }

        private void StopRunningCoroutines()
        {
            if (_delayedClosingAnimationCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_delayedClosingAnimationCoroutine);

            if (_delayedShowingAnimationCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_delayedShowingAnimationCoroutine);
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