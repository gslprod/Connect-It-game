using ConnectIt.Shop.Goods;
using System;
using System.Collections.Generic;

namespace ConnectIt.Shop.Customer.Storage
{
    public interface IStorage
    {
        IEnumerable<IProduct> Items { get; }

        event Action<IStorage> ItemsChanged;

        void AddItem(IProduct item);
        int GetProductCountOfType<T>() where T : IProduct;
        int GetProductCountOfType(Type type);
        IEnumerable<ItemAmount> GetProductsAmountsOfType(Type type);
        IEnumerable<ItemAmount> GetProductsAmountsOfType<T>() where T : IProduct;
        void RemoveItem(IProduct item);
    }
}