using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Menu.Views.ShopMenu;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.StatsMenu
{
    public class ShopMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly FramesSwitcher<VisualElement> _framesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedTextElementView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly CoinsView.Factory _coinsViewFactory;
        private readonly GoodsView.Factory _goodsViewFactory;

        private DefaultButtonView _backButton;
        private DefaultLocalizedTextElementView _titleLabel;
        private CoinsView _coinsView;
        private DefaultLocalizedTextElementView _coinsInfoLabel;
        private DefaultLocalizedTextElementView _goodsTitleLabel;
        private GoodsView _goodsView;

        public ShopMenuView(VisualElement viewRoot,
            VisualElement mainRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedTextElementView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            CoinsView.Factory coinsViewFactory,
            GoodsView.Factory goodsViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _framesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _coinsViewFactory = coinsViewFactory;
            _goodsViewFactory = goodsViewFactory;
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
                _textKeyFactory.Create(TextKeysConstants.Menu.ShopMenu.Title));

            _coinsView = _coinsViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.ShopMenu.CoinsLabel));

            _coinsInfoLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.ShopMenu.CoinsInfoLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.ShopMenu.CoinsInfoLabel_Text));

            _goodsTitleLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.ShopMenu.GoodsTitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.ShopMenu.GoodsTitleLabel_Text));

            _goodsView = _goodsViewFactory.Create(
                _viewRoot.Q<VisualElement>(NameConstants.ShopMenu.GoodsScrollViewContainer), _mainRoot);
        }

        private void DisposeDisposableViews()
        {
            _backButton.Dispose();
            _titleLabel.Dispose();
            _coinsView.Dispose();
            _coinsInfoLabel.Dispose();
            _goodsTitleLabel.Dispose();
            _goodsView.Dispose();
        }

        #region BackButton

        private void OnBackButtonClick()
        {
            _framesSwitcher.SwitchBackOrToDefault(_menuUIDocumentMonoWrapper.MainMenuContainer);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, ShopMenuView> { }
    }
}
