using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Goods;
using System;
using System.Collections.Generic;

namespace ConnectIt.Shop
{
    public interface IShop
    {
        IEnumerable<ShowcaseProduct<IProduct>> Goods { get; }

        event Action<IShop> GoodsChanged;

        void Buy(ShowcaseProduct<IProduct> product, ICustomer customer);
    }
}