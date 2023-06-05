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

        private readonly ShopConfig _config;
        private readonly SkipLevelBoost.Factory _skipLevelBoostFactory;
        private readonly SimplifyLevelBoost.Factory _simplifyLevelBoostFactory;
        private readonly AllowIncompatibleConnectionsBoost.Factory _allowIncompatibleConnectionsBoostFactory;

        public Shop(
            ShopConfig config,
            SkipLevelBoost.Factory skipLevelBoostFactory,
            SimplifyLevelBoost.Factory simplifyLevelBoostFactory,
            AllowIncompatibleConnectionsBoost.Factory allowIncompatibleConnectionsBoostFactory)
        {
            _config = config;
            _skipLevelBoostFactory = skipLevelBoostFactory;
            _simplifyLevelBoostFactory = simplifyLevelBoostFactory;
            _allowIncompatibleConnectionsBoostFactory = allowIncompatibleConnectionsBoostFactory;
        }

        public void Initialize()
        {
            _goods = new List<ShowcaseProduct<IProduct>>()
            {
                new(_skipLevelBoostFactory.Create(), _config.SkipLevelBoostPrice, _skipLevelBoostFactory.Create),
                new(_simplifyLevelBoostFactory.Create(), _config.SimplifyLevelBoostPrice, _simplifyLevelBoostFactory.Create),
                new(_allowIncompatibleConnectionsBoostFactory.Create(), _config.AllowIncompatibleConnectionsBoostPrice, _allowIncompatibleConnectionsBoostFactory.Create)
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
