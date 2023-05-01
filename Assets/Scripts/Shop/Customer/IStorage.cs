using ConnectIt.Shop.Goods;
using System.Collections.Generic;

namespace ConnectIt.Shop.Customer
{
    public interface IStorage
    {
        IEnumerable<IProduct> Items { get; }

        void AddItem(IProduct item);
        void RemoveItem(IProduct item);
    }
}