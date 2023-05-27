using ConnectIt.Localization;
using ConnectIt.Stats.Data;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.StatsMenu
{
    public class StatsElementView : IInitializable, IDisposable
    {
        private readonly IStatsData _statsData;
        private readonly VisualElement _creationRoot;
        private readonly VisualElement _mainRoot;
        private readonly DefaultLocalizedButtonView.Factory _defaultLocalizedButtonViewFactory;
        private readonly DefaultLocalizedTextElementView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly VisualTreeAsset _asset;
        private readonly DialogBoxView.Factory _dialogBoxFactory;

        private VisualElement _viewRoot;
        private DefaultLocalizedButtonView _nameButton;
        private DefaultLocalizedTextElementView _valueLabel;

        public StatsElementView(
            IStatsData statsData,
            VisualElement creationRoot,
            VisualElement mainRoot,
            DefaultLocalizedTextElementView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            VisualTreeAsset asset,
            DialogBoxView.Factory dialogBoxFactory,
            DefaultLocalizedButtonView.Factory defaultLocalizedButtonViewFactory)
        {
            _statsData = statsData;
            _creationRoot = creationRoot;
            _mainRoot = mainRoot;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _asset = asset;
            _dialogBoxFactory = dialogBoxFactory;
            _defaultLocalizedButtonViewFactory = defaultLocalizedButtonViewFactory;
        }

        public void Initialize()
        {
            CreateViews();
        }

        public void Dispose()
        {
            DisposeDisposableViews();

            _viewRoot.RemoveFromHierarchy();
        }

        private void CreateViews()
        {
            _asset.CloneTree(_creationRoot);
            _viewRoot = _creationRoot.GetLastChild();

            _nameButton = _defaultLocalizedButtonViewFactory.Create(
                _viewRoot.Q<Button>(TemplatesNameConstants.StatsRow.NameButton),
                OnDescriptionButtonClick,
                _statsData.Name);

            _valueLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.StatsRow.ValueLabel),
                _statsData.Value);
        }

        private void DisposeDisposableViews()
        {
            _nameButton.Dispose();
            _valueLabel.Dispose();
        }

        #region DescriptionButton

        private void OnDescriptionButtonClick()
        {
            _dialogBoxFactory.CreateDefaultOneButtonDialogBox(_mainRoot,
                _statsData.Name,
                _statsData.Description,
                _textKeyFactory,
                true);
        }

        #endregion

        public class Factory : PlaceholderFactory<IStatsData, VisualElement, VisualElement, StatsElementView> { }
    }
}
