using ConnectIt.Shop.Goods;
using ConnectIt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectIt.Shop.Customer.Storage
{
    public class Storage : IStorage
    {
        public event Action<IStorage> ItemsChanged;

        public IEnumerable<IProduct> Items => _items;

        private readonly List<IProduct> _items = new();
        private readonly Dictionary<Type, int> _itemsCounts = new();

        public void AddItem(IProduct item)
        {
            Assert.ArgIsNotNull(item);

            _items.Add(item);

            Type type = item.GetType();
            if (_itemsCounts.ContainsKey(type))
                _itemsCounts[type]++;
            else
                _itemsCounts.Add(type, 1);

            ItemsChanged?.Invoke(this);
        }

        public void RemoveItem(IProduct item)
        {
            Assert.ArgIsNotNull(item);

            Assert.That(
                _items.Remove(item));

            Type type = item.GetType();
            if (_itemsCounts[type] > 1)
                _itemsCounts[type]--;
            else
                _itemsCounts.Remove(type);


            ItemsChanged?.Invoke(this);
        }

        public int GetProductCountOfType<T>() where T : IProduct
            => GetProductCountOfTypeInternal(typeof(T));

        public int GetProductCountOfType(Type type)
        {
            ValidateInputType(type);

            return GetProductCountOfTypeInternal(type);
        }

        public IEnumerable<ItemAmount> GetProductsAmountsOfType<T>() where T : IProduct
            => GetProductsAmountsOfTypeInternal(typeof(T));

        public IEnumerable<ItemAmount> GetProductsAmountsOfType(Type type)
        {
            ValidateInputType(type);

            return GetProductsAmountsOfTypeInternal(type);
        }

        private int GetProductCountOfTypeInternal(Type type)
        {
            return _itemsCounts
                .Where(pair => type.IsAssignableFrom(pair.Key))
                .Sum(pair => pair.Value);
        }

        private IEnumerable<ItemAmount> GetProductsAmountsOfTypeInternal(Type type)
        {
            return _itemsCounts
                .Where(pair => type.IsAssignableFrom(pair.Key))
                .Select(pair => new ItemAmount(pair.Key, pair.Value));
        }

        private void ValidateInputType(Type type)
        {
            if (!typeof(IProduct).IsAssignableFrom(type))
                Assert.Fail();
        }
    }
}
