﻿using ConnectIt.Coroutines;
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
    public class LoadingScreenView : IInitializable, IDisposable
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
        private Label _progressBarLabel;
        private Label _titleLabel;
        private Label _messageLabel;
        private TextKey _titleKey;
        private TextKey _messageKey;

        private Coroutine _delayedShowingAnimationCoroutine;
        private Coroutine _appearAnimationCoroutine;
        private Coroutine _delayedClosingAnimationCoroutine;
        private Coroutine _delayedDisposeCoroutine;

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
            _progressBarLabel = _progressBar.Q<Label>(Custom.ProgressBar.LabelName);
            _elementsContainer = _root.Q<VisualElement>(LoadingScreenContainerName);

            _elementsContainer.AddToClassList(ClassNamesConstants.Global.LoadingScreenContainerClosed);
            _progressBarLabel.AddToClassList(ClassNamesConstants.Global.LoadingScreenProgressBarLabel);

            UpdateProgressValue(0);
            UpdateLocalization();

            _delayedShowingAnimationCoroutine = _coroutinesGlobalContainer.DelayedAction(StartShowingAnimation);

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
            Assert.IsNull(_delayedDisposeCoroutine);

            if (_appearAnimationCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_appearAnimationCoroutine);

            _delayedClosingAnimationCoroutine = _coroutinesGlobalContainer.DelayedAction(StartClosingAnimation);

            Closing?.Invoke(this);
        }

        public void Dispose()
        {
            _delayedDisposeCoroutine = null;

            _root.RemoveFromHierarchy();

            StopRunningCoroutines();

            _titleKey.ArgsChanged -= OnTitleKeyArgsChanged;
            _messageKey.ArgsChanged -= OnMessageKeyArgsChanged;
            _localizationProvider.LocalizationChanged -= UpdateLocalization;

            Disposing?.Invoke(this);
        }

        private void StartShowingAnimation()
        {
            _delayedShowingAnimationCoroutine = null;

            _elementsContainer.RemoveFromClassList(ClassNamesConstants.Global.LoadingScreenContainerClosed);
            _root.RemoveFromClassList(ClassNamesConstants.Global.LoadingScreenRootClosed);

            float appearAnimationLengthSec = Mathf.Max(
                _elementsContainer.resolvedStyle.CalculateMaxTransitionLengthSec(),
                _root.resolvedStyle.CalculateMaxTransitionLengthSec());

            _appearAnimationCoroutine = _coroutinesGlobalContainer.DelayedAction(OnShowingAnimationEnded, appearAnimationLengthSec);
        }

        private void StartClosingAnimation()
        {
            _delayedClosingAnimationCoroutine = null;

            _elementsContainer.AddToClassList(ClassNamesConstants.Global.LoadingScreenContainerClosed);
            _root.AddToClassList(ClassNamesConstants.Global.LoadingScreenRootClosed);

            float closeDelaySec = Mathf.Max(
                _elementsContainer.resolvedStyle.CalculateMaxTransitionLengthSec(),
                _root.resolvedStyle.CalculateMaxTransitionLengthSec());

            _delayedDisposeCoroutine = _coroutinesGlobalContainer.DelayedAction(Dispose, closeDelaySec);
        }

        private void OnShowingAnimationEnded()
        {
            _appearAnimationCoroutine = null;

            ShowingAnimationEnded?.Invoke(this);
        }

        private void StopRunningCoroutines()
        {
            if (_appearAnimationCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_appearAnimationCoroutine);

            if (_delayedClosingAnimationCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_delayedClosingAnimationCoroutine);

            if (_delayedDisposeCoroutine != null)
                _coroutinesGlobalContainer.StopCoroutine(_delayedDisposeCoroutine);

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

        public class Factory : PlaceholderFactory<LoadingScreenCreationData, LoadingScreenView> { }
    }
}
