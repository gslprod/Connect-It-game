using ConnectIt.Shop.Goods;
using System;
using System.Collections.Generic;

namespace ConnectIt.Shop.Customer.Storage
{
    public interface IStorage
    {
        IEnumerable<IProduct> Items { get; }

        event Action<IStorage> ItemsChanged;
        event Action<IStorage, IProduct> ItemAdded;
        event Action<IStorage, IProduct> ItemRemoved;

        void AddItem(IProduct item);
        int GetProductCountOfType<T>(bool onlySameType = false) where T : IProduct;
        int GetProductCountOfType(Type type, bool onlySameType = false);
        IEnumerable<ItemAmount> GetProductsAmountsOfType(Type type);
        IEnumerable<ItemAmount> GetProductsAmountsOfType<T>() where T : IProduct;
        void RemoveItem(IProduct item);
    }
}