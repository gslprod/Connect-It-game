using ConnectIt.Shop.Goods;
using System;
using System.Collections.Generic;

namespace ConnectIt.Shop.Customer
{
    public interface IStorage
    {
        IEnumerable<IProduct> Items { get; }

        event Action<IStorage> ItemsChanged;

        void AddItem(IProduct item);
        void RemoveItem(IProduct item);
    }
}