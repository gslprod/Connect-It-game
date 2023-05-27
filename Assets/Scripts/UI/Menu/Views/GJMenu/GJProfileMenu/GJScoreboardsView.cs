using ConnectIt.Coroutines;
using ConnectIt.Coroutines.CustomYieldInstructions;
using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.ExternalServices.GameJolt.Objects;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.GJMenu.GJProfileMenu
{
    public class GJScoreboardsView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly GameJoltAPIProvider _gjApiProvider;
        private readonly GJScoreboardView.Factory _gjScoresViewFactory;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DialogBoxView.Factory _dialogBoxViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly GJTopPositionLabel.Factory _gjTopPositionLabelViewFactory;

        private VisualElement _scoreboardsContainer;
        private DefaultButtonView _changeTableButtonView;
        private GJTopPositionLabel _topPositionLabel;

        private readonly List<GJScoreboardView> _gjScoreboardViews = new();
        private readonly TransitionsStopWaiter _transitionsStopWaiter = new();
        private FramesSwitcher<GJScoreboardView> _scoreboardsFrames;

        public GJScoreboardsView(
            VisualElement viewRoot,
            VisualElement mainRoot,
            GameJoltAPIProvider gjApiProvider,
            GJScoreboardView.Factory gjScoresViewFactory,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DialogBoxView.Factory dialogBoxViewFactory,
            TextKey.Factory textKeyFactory,
            ICoroutinesGlobalContainer coroutinesGlobalContainer,
            ILocalizationProvider localizationProvider,
            GJTopPositionLabel.Factory gjTopPositionLabelViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _gjApiProvider = gjApiProvider;
            _gjScoresViewFactory = gjScoresViewFactory;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _dialogBoxViewFactory = dialogBoxViewFactory;
            _textKeyFactory = textKeyFactory;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
            _localizationProvider = localizationProvider;
            _gjTopPositionLabelViewFactory = gjTopPositionLabelViewFactory;
        }

        public void Initialize()
        {
            CreateViews();

            _gjApiProvider.TablesChanged += OnTablesChanged;
        }

        public void Dispose()
        {
            _gjApiProvider.TablesChanged -= OnTablesChanged;

            DisposeDisposableViews();
        }

        private void CreateViews()
        {
            _scoreboardsContainer = _viewRoot.Q<VisualElement>(NameConstants.GJMenu.GJProfileMenu.ScoreboardsContainer);

            _changeTableButtonView = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.GJMenu.GJProfileMenu.ChangeTableButton),
                OnChangeTableButtonClick);

            CreateScoreboardViews();

            _topPositionLabel = _gjTopPositionLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJMenu.GJProfileMenu.TopUserPositionLabel));
        }

        private void CreateScoreboardViews()
        {
            IEnumerable<TableInfo> tables = _gjApiProvider.Tables;
            if (tables.Count() == 0)
                return;

            foreach (TableInfo table in tables)
            {
                GJScoreboardView view = CreateViewByTable(table);

                _gjScoreboardViews.Add(view);
            }

            CreateFramesSwitcher();
        }

        #region FramesSwitcher

        private void CreateFramesSwitcher()
        {
            Frame<GJScoreboardView>[] frames = _gjScoreboardViews.Select(view => new Frame<GJScoreboardView>(view)).ToArray();
            _scoreboardsFrames = new(frames,
                EnableFrame,
                DisableFrame);

            _topPositionLabel.SetFramesSwitcher(_scoreboardsFrames);

            _scoreboardsFrames.FrameSwitched += OnFrameSwitched;

            _coroutinesGlobalContainer.DelayedAction(() => _scoreboardsFrames.SwitchTo(frames[0].Element), new WaitForFrames(2));
        }

        private void EnableFrame(GJScoreboardView view)
        {
            view.ViewRoot.RemoveFromClassList(ClassNamesConstants.MenuView.ContentContainerFrameClosed);
        }

        private void DisableFrame(GJScoreboardView view)
        {
            view.ViewRoot.AddToClassList(ClassNamesConstants.MenuView.ContentContainerFrameClosed);
        }

        private void OnFrameSwitched(GJScoreboardView view)
        {
            if (view.TargetTable.WasUpdatedAtLeastOnce)
                return;

            _gjApiProvider.UpdateScoresForTable(view.TargetTable);
        }

        #endregion

        private void DisposeDisposableViews()
        {
            _changeTableButtonView.Dispose();
            _topPositionLabel.Dispose();

            _scoreboardsFrames.FrameSwitched += OnFrameSwitched;

            DisposeScoreboardViews();
        }

        private void DisposeScoreboardViews()
        {
            foreach (GJScoreboardView view in _gjScoreboardViews)
                view.Dispose();

            _gjScoreboardViews.Clear();
        }

        private void OnTablesChanged()
        {
            if (_gjScoreboardViews.Count == 0)
            {
                CreateScoreboardViews();
                return;
            }

            IEnumerable<TableInfo> tables = _gjApiProvider.Tables;
            IEnumerable<TableInfo> created = _gjScoreboardViews.Select(view => view.TargetTable);

            IEnumerable<TableInfo> toCreate = tables.Except(created);
            foreach (TableInfo table in toCreate)
            {
                GJScoreboardView view = CreateViewByTable(table);

                _gjScoreboardViews.Add(view);
            }

            IEnumerable<TableInfo> toDestroy = created.Except(toCreate);
            foreach (TableInfo table in toDestroy)
            {
                GJScoreboardView view = _gjScoreboardViews.Find(item => item.TargetTable.Equals(table));

                if (view == _scoreboardsFrames.Current)
                {
                    DisableFrame(view);
                    _coroutinesGlobalContainer.DelayedAction(() => _transitionsStopWaiter.AbortCurrentAndWait(() =>
                    {
                        _gjScoreboardViews.Remove(view);
                        view.Dispose();
                    }, view.ViewRoot));

                    continue;
                }

                _gjScoreboardViews.Remove(view);
                view.Dispose();
            }

            CreateFramesSwitcher();
        }

        private GJScoreboardView CreateViewByTable(TableInfo table)
            => _gjScoresViewFactory.Create(table, _scoreboardsContainer, _mainRoot);

        #region SelectTableButton

        private void OnChangeTableButtonClick()
        {
            int tablesCount = _gjScoreboardViews.Count;
            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[tablesCount];
            for (int i = 0; i < tablesCount; i++)
            {
                GJScoreboardView view = _gjScoreboardViews[i];

                string tableNameTextKey = string.Format(TextKeysConstants.Menu.GJMenu.GJProfileMenu.ScoreboardNamePattern, view.TargetTable.GJTable.ID);
                TextKey textKey = _localizationProvider.HasKey(tableNameTextKey) ?
                    _textKeyFactory.Create(tableNameTextKey) :
                    _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJProfileMenu.Scoreboard_Unknown,
                    new object[] { view.TargetTable.GJTable.Name });

                buttonsInfo[i] = new(
                    textKey,
                    () => OnScoreboardButtonClick(view),
                    DialogBoxButtonType.Default,
                    true);
            }

            DialogBoxButtonInfo cancelButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Common.Close),
                null,
                DialogBoxButtonType.Dismiss,
                true);

            DialogBoxCreationData creationData = new(
                _mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.GJScoreboardChange_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.GJScoreboardChange_Message),
                buttonsInfo,
                cancelButtonInfo,
                true);

            _dialogBoxViewFactory.Create(creationData);

            void OnScoreboardButtonClick(GJScoreboardView view)
            {
                _scoreboardsFrames.SwitchTo(view);
            }
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, GJScoreboardsView> { }
    }
}
