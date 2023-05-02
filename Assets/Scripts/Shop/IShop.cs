using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Goods;
using System.Collections.Generic;

namespace ConnectIt.Shop
{
    public interface IShop
    {
        IEnumerable<ShowcaseProduct<IProduct>> Goods { get; }

        void Buy(ShowcaseProduct<IProduct> product, ICustomer customer);
    }
}