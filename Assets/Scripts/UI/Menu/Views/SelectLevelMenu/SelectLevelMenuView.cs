﻿using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.SelectLevelMenu
{
    public class SelectLevelMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly FramesSwitcher<VisualElement> _framesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly SelectLevelButtonsView.Factory _selectLevelButtonsViewFactory;
        private readonly DefaultLocalizedTextElementView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;

        private DefaultButtonView _backButton;
        private SelectLevelButtonsView _selectLevelButtonsView;
        private DefaultLocalizedTextElementView _titleLabel;

        public SelectLevelMenuView(VisualElement viewRoot,
            VisualElement mainRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory,
            SelectLevelButtonsView.Factory selectLevelButtonsViewFactory,
            DefaultLocalizedTextElementView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _framesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _selectLevelButtonsViewFactory = selectLevelButtonsViewFactory;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
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
                _viewRoot.Q<Button>(NameConstants.SelectLevelMenu.BackButton), OnBackButtonClick);

            _selectLevelButtonsView = _selectLevelButtonsViewFactory.Create(
                _viewRoot.Q<VisualElement>(NameConstants.SelectLevelMenu.LevelViewContainer),
                _mainRoot);

            _titleLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.SelectLevelMenu.TitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.SelectLevelMenu.Title));
        }

        private void DisposeDisposableViews()
        {
            _backButton.Dispose();
            _selectLevelButtonsView.Dispose();
            _titleLabel.Dispose();
        }

        #region BackButton

        private void OnBackButtonClick()
        {
            _framesSwitcher.SwitchBackOrToDefault(_menuUIDocumentMonoWrapper.MainMenuContainer);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, SelectLevelMenuView> { }
    }
}
