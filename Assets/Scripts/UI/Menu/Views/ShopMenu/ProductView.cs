using ConnectIt.Localization;
using ConnectIt.Shop;
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Customer.Storage;
using ConnectIt.Shop.Goods;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.ShopMenu
{
    public class ProductView : IInitializable, IDisposable
    {
        private readonly ShowcaseProduct<IProduct> _showcaseProduct;
        private readonly VisualElement _creationRoot;
        private readonly VisualElement _mainRoot;
        private readonly DefaultLocalizedButtonView.Factory _defaultLocalizedButtonViewFactory;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedLabelView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly VisualTreeAsset _asset;
        private readonly ICustomer _playerCustomer;
        private readonly IShop _shop;
        private readonly DialogBoxView.Factory _dialogBoxFactory;

        private VisualElement _viewRoot;
        private DefaultLocalizedLabelView _nameLabel;
        private DefaultLocalizedLabelView _descriptionLabel;
        private DefaultLocalizedLabelView _amountLabel;
        private DefaultLocalizedLabelView _priceLabel;
        private DefaultButtonView _descriptionButton;
        private DefaultLocalizedButtonView _buyButton;

        public ProductView(
            ShowcaseProduct<IProduct> showcaseProduct,
            VisualElement creationRoot,
            VisualElement mainRoot,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedLabelView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            VisualTreeAsset asset,
            ICustomer playerCustomer,
            IShop shop,
            DialogBoxView.Factory dialogBoxFactory,
            DefaultLocalizedButtonView.Factory defaultLocalizedButtonViewFactory)
        {
            _showcaseProduct = showcaseProduct;
            _creationRoot = creationRoot;
            _mainRoot = mainRoot;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _asset = asset;
            _playerCustomer = playerCustomer;
            _shop = shop;
            _dialogBoxFactory = dialogBoxFactory;
            _defaultLocalizedButtonViewFactory = defaultLocalizedButtonViewFactory;
        }

        public void Initialize()
        {
            CreateViews();

            _playerCustomer.Storage.ItemsChanged += OnStorageItemsChanged;
        }

        public void Dispose()
        {
            _playerCustomer.Storage.ItemsChanged -= OnStorageItemsChanged;

            DisposeDisposableViews();

            _viewRoot.RemoveFromHierarchy();
        }

        private void CreateViews()
        {
            _asset.CloneTree(_creationRoot);
            _viewRoot = _creationRoot.GetLastChild();

            _nameLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.Product.NameLabel),
                _showcaseProduct.ShowcaseItem.Name);

            _descriptionLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.Product.DescriptionLabel),
                _showcaseProduct.ShowcaseItem.Description);

            _amountLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.Product.AmountLabel),
                _textKeyFactory.Create(
                    TextKeysConstants.Menu.ShopMenu.Product_AmountLabel_Text,
                    new object[] { GetProductAmountFromCustomer() }));

            _priceLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.Product.PriceLabel),
                _textKeyFactory.Create(
                    TextKeysConstants.Menu.ShopMenu.Product_PriceLabel_Text,
                    new object[] { _showcaseProduct.Price }));

            _descriptionButton = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(TemplatesNameConstants.Product.DescriptionContainerButton),
                OnDescriptionButtonClick);

            _buyButton = _defaultLocalizedButtonViewFactory.Create(
                _viewRoot.Q<Button>(TemplatesNameConstants.Product.BuyButton),
                OnBuyButtonClick,
                _textKeyFactory.Create(
                    TextKeysConstants.Common.Buy));
        }

        private void DisposeDisposableViews()
        {
            _nameLabel.Dispose();
            _descriptionLabel.Dispose();
            _amountLabel.Dispose();
            _priceLabel.Dispose();
            _descriptionButton.Dispose();
            _buyButton.Dispose();
        }

        private int GetProductAmountFromCustomer()
            => _playerCustomer.Storage.GetProductCountOfType(_showcaseProduct.ShowcaseItem.GetType());

        private void OnStorageItemsChanged(IStorage storage)
        {
            _amountLabel.SetArgs(GetProductAmountFromCustomer());
        }

        #region DescriptionButton

        private void OnDescriptionButtonClick()
        {
            _dialogBoxFactory.CreateDefaultOneButtonDialogBox(_mainRoot,
                _showcaseProduct.ShowcaseItem.Name,
                _showcaseProduct.ShowcaseItem.Description,
                _textKeyFactory.Create(TextKeysConstants.Common.Close),
                true);
        }

        #endregion

        #region BuyButton

        private void OnBuyButtonClick()
        {
            if (!IsEnoughCoins())
                return;

            _dialogBoxFactory.CreateDefaultConfirmCancelDialogBox(_mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.ConfirmBuy_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.ConfirmBuy_Message),
                _textKeyFactory.Create(TextKeysConstants.Common.Cancel),
                _textKeyFactory.Create(TextKeysConstants.Common.Confirm),
                OnConfirmBuyButtonClick,
                true);
        }

        private bool IsEnoughCoins()
        {
            if (_playerCustomer.Wallet.Coins >= _showcaseProduct.Price)
                return true;

            _dialogBoxFactory.CreateDefaultOneButtonDialogBox(_mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.NotEnoughCoins_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.NotEnoughCoins_Message),
                _textKeyFactory.Create(TextKeysConstants.Common.Close),
                true);

            return false;
        }

        #endregion

        #region ConfirmBuyButton

        private void OnConfirmBuyButtonClick()
        {
            if (!IsEnoughCoins())
                return;

            _shop.Buy(_showcaseProduct, _playerCustomer);
        }

        #endregion

        public class Factory : PlaceholderFactory<ShowcaseProduct<IProduct>, VisualElement, VisualElement, ProductView> { }
    }
}
