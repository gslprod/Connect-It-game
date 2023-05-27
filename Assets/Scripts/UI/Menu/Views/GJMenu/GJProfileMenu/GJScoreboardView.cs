using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.ExternalServices.GameJolt.Objects;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.GJMenu.GJProfileMenu
{
    public class GJScoreboardView : IInitializable, IDisposable
    {
        public TableInfo TargetTable => _targetTable;
        public VisualElement ViewRoot => _viewRoot;

        private readonly TableInfo _targetTable;
        private readonly VisualElement _creationRoot;
        private readonly VisualElement _mainRoot;
        private readonly GameJoltAPIProvider _gjApiProvider;
        private readonly GJScoreView.Factory _gjScoreViewFactory;
        private readonly VisualTreeAsset _asset;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedTextElementView.Factory _defaultLocalizedTextViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly ILocalizationProvider _localizationProvider;

        private VisualElement _viewRoot;
        private VisualElement _gjScoreViewsCreationRoot;
        private DefaultButtonView _updateButton;
        private DefaultLocalizedTextElementView _tableNameLabel;

        private readonly List<GJScoreView> _gjScoreViews = new();

        public GJScoreboardView(
            TableInfo targetTable,
            VisualElement creationRoot,
            VisualElement mainRoot,
            GameJoltAPIProvider gjApiProvider,
            GJScoreView.Factory gjScoreViewFactory,
            VisualTreeAsset asset,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedTextElementView.Factory defaultLocalizedTextViewFactory,
            TextKey.Factory textKeyFactory,
            ILocalizationProvider localizationProvider)
        {
            _targetTable = targetTable;
            _creationRoot = creationRoot;
            _mainRoot = mainRoot;
            _gjApiProvider = gjApiProvider;
            _gjScoreViewFactory = gjScoreViewFactory;
            _asset = asset;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultLocalizedTextViewFactory = defaultLocalizedTextViewFactory;
            _textKeyFactory = textKeyFactory;
            _localizationProvider = localizationProvider;
        }

        public void Initialize()
        {
            CreateViews();

            _gjApiProvider.TableScoresChanged += OnTableScoresChanged;
        }

        public void Dispose()
        {
            _gjApiProvider.TableScoresChanged -= OnTableScoresChanged;

            DisposeDisposableViews();
        }

        private void CreateViews()
        {
            _asset.CloneTree(_creationRoot);
            _viewRoot = _creationRoot.GetLastChild();

            _gjScoreViewsCreationRoot = _viewRoot.Q<VisualElement>(TemplatesNameConstants.GJScoreboard.ScoresScrollViewContainer);

            _updateButton = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(TemplatesNameConstants.GJScoreboard.UpdateButton),
                OnUpdateButtonClick);

            string tableNameTextKey = string.Format(TextKeysConstants.Menu.GJMenu.GJProfileMenu.ScoreboardNamePattern, TargetTable.GJTable.ID);
            TextKey textKey = _localizationProvider.HasKey(tableNameTextKey) ?
                _textKeyFactory.Create(tableNameTextKey) :
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJProfileMenu.Scoreboard_Unknown,
                new object[] { TargetTable.GJTable.Name });

            _tableNameLabel = _defaultLocalizedTextViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.GJScoreboard.ButtonNameLabel),
                textKey);

            CreateScoresViews();
        }

        private void CreateScoresViews()
        {
            foreach (var score in _gjApiProvider.ScoresInTables[TargetTable.GJTable.ID])
            {
                GJScoreView createdView =
                    _gjScoreViewFactory.Create(score, _gjScoreViewsCreationRoot, _mainRoot);

                _gjScoreViewsCreationRoot.GetLastChild().AddToClassList(ClassNamesConstants.Global.ScrollViewContainerChild);
                _gjScoreViews.Add(createdView);
            }

            if (_gjScoreViews.Count > 0)
                _gjScoreViewsCreationRoot.GetLastChild().AddToClassList(ClassNamesConstants.Global.ScrollViewContainerChildLast);
        }

        private void DisposeDisposableViews()
        {
            _updateButton.Dispose();
            _tableNameLabel.Dispose();

            DisposeScoresViews();
        }

        private void DisposeScoresViews()
        {
            for (int i = _gjScoreViews.Count - 1; i >= 0; i--)
            {
                GJScoreView scoreView = _gjScoreViews[i];

                scoreView.Dispose();
            }

            _gjScoreViews.Clear();
        }

        private void OnTableScoresChanged(TableInfo table)
        {
            if (!table.Equals(TargetTable))
                return;

            DisposeScoresViews();
            CreateScoresViews();
        }

        #region UpdateButton

        private void OnUpdateButtonClick()
        {
            _gjApiProvider.UpdateScoresForTable(_targetTable);
        }

        #endregion

        public class Factory : PlaceholderFactory<TableInfo, VisualElement, VisualElement, GJScoreboardView> { }
    }
}
