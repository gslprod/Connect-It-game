using ConnectIt.Stats;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.StatsMenu
{
    public class StatsElementsListView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly IStatsCenter _statsCenter;
        private readonly StatsElementView.Factory _statsElementViewFactory;

        private readonly List<StatsElementView> _statsElementViews = new();

        public StatsElementsListView(
            VisualElement viewRoot,
            VisualElement mainRoot,
            IStatsCenter statsCenter,
            StatsElementView.Factory statsElementViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _statsCenter = statsCenter;
            _statsElementViewFactory = statsElementViewFactory;
        }

        public void Initialize()
        {
            CreateViews();
        }

        public void Dispose()
        {
            DisposeDisposableViews();
        }

        private void CreateViews()
        {
            foreach (var product in _statsCenter.StatsData)
            {
                StatsElementView createdView =
                    _statsElementViewFactory.Create(product, _viewRoot, _mainRoot);

                _viewRoot.GetLastChild().AddToClassList(ClassNamesConstants.Global.ScrollViewContainerChild);
                _statsElementViews.Add(createdView);
            }

            if (_statsElementViews.Count > 0)
                _viewRoot.GetLastChild().AddToClassList(ClassNamesConstants.Global.ScrollViewContainerChildLast);
        }

        private void DisposeDisposableViews()
        {
            for (int i = _statsElementViews.Count - 1; i >= 0; i--)
            {
                StatsElementView statsElementView = _statsElementViews[i];

                statsElementView.Dispose();
            }

            _statsElementViews.Clear();
        }

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, StatsElementsListView> { }
    }
}
