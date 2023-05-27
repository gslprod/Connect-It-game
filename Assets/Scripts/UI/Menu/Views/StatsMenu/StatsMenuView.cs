using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.StatsMenu
{
    public class StatsMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly FramesSwitcher<VisualElement> _framesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedTextElementView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly StatsElementsListView.Factory _statsElementsListViewFactory;

        private DefaultButtonView _backButton;
        private DefaultLocalizedTextElementView _titleLabel;
        private StatsElementsListView _statsElementsListView;

        public StatsMenuView(
            VisualElement viewRoot,
            VisualElement mainRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedTextElementView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            StatsElementsListView.Factory statsElementsListViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _framesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _statsElementsListViewFactory = statsElementsListViewFactory;
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
            _backButton = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.StatsMenu.BackButton), OnBackButtonClick);

            _titleLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.StatsMenu.TitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.StatsMenu.Title));

            _statsElementsListView = _statsElementsListViewFactory.Create(
                _viewRoot.Q<VisualElement>(NameConstants.StatsMenu.StatsListScrollViewContainer),
                _mainRoot);
        }

        private void DisposeDisposableViews()
        {
            _backButton.Dispose();
            _titleLabel.Dispose();
            _statsElementsListView.Dispose();
        }

        #region BackButton

        private void OnBackButtonClick()
        {
            _framesSwitcher.SwitchBackOrToDefault(_menuUIDocumentMonoWrapper.MainMenuContainer);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, StatsMenuView> { }
    }
}
