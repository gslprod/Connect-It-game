using ConnectIt.Coroutines;
using ConnectIt.Localization;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using ConnectIt.Utilities.Formatters;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Custom = ConnectIt.UI.CustomControls;

namespace ConnectIt.UI.LoadingScreen
{
    public class LoadingScreenView
    {
        public const string LoadingScreenContainerName = "container";
        public const string TitleLabelName = "title";
        public const string MessageLabelName = "message";
        public const string ProgressBarName = "progress-bar";

        public event Action<LoadingScreenView> Showing;
        public event Action<LoadingScreenView> ShowingAnimationEnded;
        public event Action<LoadingScreenView> Closing;
        public event Action<LoadingScreenView> Disposing;

        private readonly ILocalizationProvider _localizationProvider;
        private readonly VisualTreeAsset _uiAsset;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;
        private readonly IFormatter _formatter;
        private LoadingScreenCreationData _creationData;

        private VisualElement _parent;
        private TemplateContainer _root;
        private VisualElement _elementsContainer;
        private Custom.ProgressBar _progressBar;
        private Label _titleLabel;
        private Label _messageLabel;
        private TextKey _titleKey;
        private TextKey _messageKey;

        private Coroutine _appearAnimationCoroutine;
        private Coroutine _disposeCoroutine;

        public LoadingScreenView(ILocalizationProvider localizationProvider,
            VisualTreeAsset uiAsset,
            LoadingScreenCreationData creationData,
            ICoroutinesGlobalContainer coroutinesGlobalContainer,
            IFormatter formatter)
        {
            _localizationProvider = localizationProvider;
            _uiAsset = uiAsset;
            _creationData = creationData;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
            _formatter = formatter;
        }

        public void Initialize()
        {
            _parent = _creationData.Parent;
            _titleKey = _creationData.TitleKey;
            _messageKey = _creationData.MessageKey;

            if (_creationData.ShowImmediately)
                Show();

            _creationData = null;
        }

        public void Show()
        {
            Assert.That(_root == null);

            _root = _uiAsset.CloneTree();
            _parent.Add(_root);
            _root.AddToClassList(ClassNamesConstants.Global.LoadingScreenRoot);
            _root.AddToClassList(ClassNamesConstants.Global.LoadingScreenRootClosed);

            _titleLabel = _root.Q<Label>(TitleLabelName);
            _messageLabel = _root.Q<Label>(MessageLabelName);
            _progressBar = _root.Q<Custom.ProgressBar>(ProgressBarName);
            _elementsContainer = _root.Q<VisualElement>(LoadingScreenContainerName);

            UpdateLocalization();

            _coroutinesGlobalContainer.DelayedAction(StartShowingAnimation);

            _titleKey.ArgsChanged += OnTitleKeyArgsChanged;
            _messageKey.ArgsChanged += OnMessageKeyArgsChanged;
            _localizationProvider.LocalizationChanged += UpdateLocalization;

            Showing?.Invoke(this);
        }

        public void UpdateProgressValue(float progress)
        {
            Assert.ThatArgIs(progress >= 0, progress <= 100);

            _progressBar.value = progress;
            _progressBar.Title = _formatter.FormatSceneLoadingProgress(progress);
        }

        public void Close()
        {
            Assert.IsNull(_disposeCoroutine);

            if (_appearAnimationCoroutine != null)
                _coroutinesGlobalContainer.StopAndUnregisterCoroutine(_appearAnimationCoroutine);

            _elementsContainer.AddToClassList(ClassNamesConstants.Global.LoadingScreenContainerClosed);
            _root.AddToClassList(ClassNamesConstants.Global.LoadingScreenRootClosed);

            float closeDelaySec = Mathf.Max(
                _elementsContainer.resolvedStyle.CalculateMaxTransitionLength(),
                _root.resolvedStyle.CalculateMaxTransitionLength());

            _disposeCoroutine = _coroutinesGlobalContainer.DelayedAction(Dispose, new WaitForSeconds(closeDelaySec));

            Closing?.Invoke(this);
        }

        public void Dispose()
        {
            _root.RemoveFromHierarchy();

            _titleKey.ArgsChanged -= OnTitleKeyArgsChanged;
            _messageKey.ArgsChanged -= OnMessageKeyArgsChanged;
            _localizationProvider.LocalizationChanged -= UpdateLocalization;

            Disposing?.Invoke(this);
        }

        private void StartShowingAnimation()
        {
            _elementsContainer.RemoveFromClassList(ClassNamesConstants.Global.LoadingScreenContainerClosed);
            _root.RemoveFromClassList(ClassNamesConstants.Global.LoadingScreenRootClosed);

            float appearAnimationLengthSec = Mathf.Max(
                _elementsContainer.resolvedStyle.CalculateMaxTransitionLength(),
                _root.resolvedStyle.CalculateMaxTransitionLength());

            _appearAnimationCoroutine = _coroutinesGlobalContainer.DelayedAction(OnShowingAnimationEnded, new WaitForSeconds(appearAnimationLengthSec));
        }

        private void OnShowingAnimationEnded()
        {
            ShowingAnimationEnded?.Invoke(this);
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

        public class Factory : PlaceholderFactory<LoadingScreenCreationData, LoadingScreenView> { }
    }
}
