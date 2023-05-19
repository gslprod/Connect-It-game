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
        private LoadingDialogBoxViewCreationData _creationData;
        private TextKey _messageTextKey;

        private CustomDialogBoxView _customDialogBoxView;
        private DefaultLocalizedLabelView _messageLabelView;

        public LoadingDialogBoxView(
            CustomDialogBoxView.Factory customDialogBoxViewFactory,
            LoadingDialogBoxViewCreationData creationData,
            VisualTreeAsset uiAsset,
            DefaultLocalizedLabelView.Factory defaultLocalizedLabelViewFactory)
        {
            _customDialogBoxViewFactory = customDialogBoxViewFactory;
            _creationData = creationData;
            _uiAsset = uiAsset;
            _defaultLocalizedLabelViewFactory = defaultLocalizedLabelViewFactory;
        }

        public void Initialize()
        {
            CreateDialogBox();
        }

        public void Dispose()
        {
            DisposeDisposableViews();
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

            if (_creationData.ShowImmediately)
                Show();

            _creationData = null;
        }

        private void CreateViews()
        {
            _messageLabelView = _defaultLocalizedLabelViewFactory.Create(
                _customDialogBoxView.ContentRoot.Q<Label>(TemplatesNameConstants.LoadingDialogBox.MessageLabel),
                _messageTextKey);
        }

        private void DisposeDisposableViews()
        {
            _customDialogBoxView.Dispose();
            _messageLabelView.Dispose();
        }

        public class Factory : PlaceholderFactory<LoadingDialogBoxViewCreationData, LoadingDialogBoxView> { }
    }
}
