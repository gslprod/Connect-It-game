using ConnectIt.Coroutines;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.DialogBox
{
    public class LoadingDialogBoxView : IDialogBoxView, IInitializable, IDisposable
    {
        public event Action<IDialogBoxView> Closing
        {
            add => _customDialogBoxView.Closing += value;
            remove => _customDialogBoxView.Closing -= value;
        }
        public event Action<IDialogBoxView> Showing
        {
            add => _customDialogBoxView.Showing += value;
            remove => _customDialogBoxView.Showing -= value;
        }
        public event Action<IDialogBoxView> Disposing
        {
            add => _customDialogBoxView.Disposing += value;
            remove => _customDialogBoxView.Disposing -= value;
        }

        private readonly CustomDialogBoxView.Factory _customDialogBoxViewFactory;
        private readonly VisualTreeAsset _uiAsset;
        private readonly DefaultLocalizedLabelView.Factory _defaultLocalizedLabelViewFactory;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;
        private LoadingDialogBoxViewCreationData _creationData;
        private TextKey _messageTextKey;

        private CustomDialogBoxView _customDialogBoxView;
        private DefaultLocalizedLabelView _messageLabelView;
        private VisualElement _spinningElement;

        public LoadingDialogBoxView(
            CustomDialogBoxView.Factory customDialogBoxViewFactory,
            LoadingDialogBoxViewCreationData creationData,
            VisualTreeAsset uiAsset,
            DefaultLocalizedLabelView.Factory defaultLocalizedLabelViewFactory,
            ICoroutinesGlobalContainer coroutinesGlobalContainer)
        {
            _customDialogBoxViewFactory = customDialogBoxViewFactory;
            _creationData = creationData;
            _uiAsset = uiAsset;
            _defaultLocalizedLabelViewFactory = defaultLocalizedLabelViewFactory;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
        }

        public void Initialize()
        {
            CreateDialogBox();
        }

        public void Dispose()
        {
            _customDialogBoxView.Dispose();
        }

        public void Show()
        {
            _customDialogBoxView.Show();
            CreateViews();
        }

        public void Close()
        {
            _customDialogBoxView.Close();
        }

        private void CreateDialogBox()
        {
            CustomDialogBoxCreationData customDialogBoxCreationData =
                new(
                _creationData.Parent,
                _creationData.TitleKey,
                _uiAsset,
                _creationData.BottomButton,
                false)
                {
                    AdditionalDialogBoxRootClass = ClassNamesConstants.Global.LoadingBoxRoot,
                    AdditionalDialogBoxRootClosedClass = ClassNamesConstants.Global.LoadingBoxRootClosed,
                    AdditionalDialogBoxContainerClass = ClassNamesConstants.Global.LoadingBoxContainer,
                    AdditionalDialogBoxContainerClosedClass = ClassNamesConstants.Global.LoadingBoxContainerClosed
                };

            _customDialogBoxView = _customDialogBoxViewFactory.Create(customDialogBoxCreationData);
            _messageTextKey = _creationData.MessageKey;

            _customDialogBoxView.Disposing += OnDialogBoxDisposing;

            if (_creationData.ShowImmediately)
                Show();

            _creationData = null;
        }

        private void CreateViews()
        {
            VisualElement root = _customDialogBoxView.ContentRoot;

            _messageLabelView = _defaultLocalizedLabelViewFactory.Create(
                _customDialogBoxView.ContentRoot.Q<Label>(TemplatesNameConstants.LoadingDialogBox.MessageLabel),
                _messageTextKey);

            SetupSpinning(root);
        }

        private void SetupSpinning(VisualElement root)
        {
            _spinningElement = root.Q<VisualElement>(TemplatesNameConstants.LoadingDialogBox.LoadingSpinningIcon);
            _coroutinesGlobalContainer.DelayedAction(() => _spinningElement.AddToClassList(ClassNamesConstants.Global.LoadingBoxSpinningIcon360Rotated));
            
            _spinningElement.RegisterCallback<TransitionEndEvent>(RestartSpin);
        }

        private void RestartSpin(TransitionEndEvent endEvent)
        {
            _spinningElement.RemoveFromClassList(ClassNamesConstants.Global.LoadingBoxSpinningIcon360Rotated);
            _coroutinesGlobalContainer.DelayedAction(() => _spinningElement.AddToClassList(ClassNamesConstants.Global.LoadingBoxSpinningIcon360Rotated));
        }

        private void DisposeDisposableViews()
        {
            _messageLabelView.Dispose();
        }

        private void OnDialogBoxDisposing(IDialogBoxView dialogBoxView)
        {
            _spinningElement.UnregisterCallback<TransitionEndEvent>(RestartSpin);
            _customDialogBoxView.Disposing -= OnDialogBoxDisposing;

            DisposeDisposableViews();
        }

        public class Factory : PlaceholderFactory<LoadingDialogBoxViewCreationData, LoadingDialogBoxView> { }
    }
}
