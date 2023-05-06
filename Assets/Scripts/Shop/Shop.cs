using ConnectIt.Config;
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Goods;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Utilities;
using System;
using System.Collections.Generic;
using Zenject;

namespace ConnectIt.Shop
{
    public class Shop : IShop, IInitializable
    {
        public event Action<IShop> GoodsChanged;

        public IEnumerable<ShowcaseProduct<IProduct>> Goods => _goods;

        private List<ShowcaseProduct<IProduct>> _goods;

        private readonly SkipLevelBoost.Factory _skipLevelBoostFactory;
        private readonly ShopConfig _config;

        public Shop(
            SkipLevelBoost.Factory skipLevelBoostFactory,
            ShopConfig config)
        {
            _skipLevelBoostFactory = skipLevelBoostFactory;
            _config = config;
        }

        public void Initialize()
        {
            _goods = new List<ShowcaseProduct<IProduct>>()
            {
                new(_skipLevelBoostFactory.Create(), _config.SkipLevelBoostPrice, () => _skipLevelBoostFactory.Create())
            };
        }

        public void Buy(ShowcaseProduct<IProduct> product, ICustomer customer)
        {
            Assert.That(IsProductInSale(product));

            customer.Wallet.Withdraw(product.Price);
            customer.Storage.AddItem(product.GetNewInstance());
        }

        public void SetGoods(IEnumerable<ShowcaseProduct<IProduct>> newGoods)
        {
            _goods.Clear();
            _goods.AddRange(newGoods);

            GoodsChanged?.Invoke(this);
        }

        private bool IsProductInSale(ShowcaseProduct<IProduct> product)
            => _goods.Contains(product);
    }
}
