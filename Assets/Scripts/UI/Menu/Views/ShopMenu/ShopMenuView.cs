using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Tools;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.StatsMenu
{
    public class ShopMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly FramesSwitcher<VisualElement> _framesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedLabelView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly CoinsView.Factory _coinsViewFactory;

        private DefaultButtonView _backButton;
        private DefaultLocalizedLabelView _titleLabel;
        private CoinsView _coinsView;
        private DefaultLocalizedLabelView _coinsInfoLabel;

        public ShopMenuView(VisualElement viewRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedLabelView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            CoinsView.Factory coinsViewFactory)
        {
            _viewRoot = viewRoot;
            _framesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _coinsViewFactory = coinsViewFactory;
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
                _viewRoot.Q<Button>(NameConstants.ShopMenu.BackButton), OnBackButtonClick);

            _titleLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.ShopMenu.TitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.ShopMenu.Title, null));

            _coinsView = _coinsViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.ShopMenu.CoinsLabel));

            _coinsInfoLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.ShopMenu.CoinsInfoLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.ShopMenu.CoinsInfoLabel_Title, null));
        }

        private void DisposeDisposableViews()
        {
            _backButton.Dispose();
            _titleLabel.Dispose();
            _coinsView.Dispose();
            _coinsInfoLabel.Dispose();
        }

        #region BackButton

        private void OnBackButtonClick()
        {
            _framesSwitcher.SwitchBackOrToDefault(_menuUIDocumentMonoWrapper.MainMenuContainer);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, ShopMenuView> { }
    }
}
