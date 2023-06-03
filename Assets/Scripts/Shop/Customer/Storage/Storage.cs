using ConnectIt.Infrastructure.Dispose;
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
        public event Action<IStorage, IProduct> ItemAdded;
        public event Action<IStorage, IProduct> ItemRemoved;

        public IEnumerable<IProduct> Items => items;

        protected readonly List<IProduct> items = new();
        protected readonly Dictionary<Type, int> itemsCounts = new();

        public void AddItem(IProduct item)
        {
            Assert.ArgIsNotNull(item);

            AddItemWithoutNotify(item);

            ItemsChanged?.Invoke(this);
            ItemAdded?.Invoke(this, item);
        }

        public void RemoveItem(IProduct item)
        {
            Assert.ArgIsNotNull(item);

            RemoveItemWithoutNotify(item);

            ItemsChanged?.Invoke(this);
            ItemRemoved?.Invoke(this, item);
        }

        public int GetProductCountOfType<T>(bool onlySameType = false) where T : IProduct
            => GetProductCountOfTypeInternal(typeof(T), onlySameType);

        public int GetProductCountOfType(Type type, bool onlySameType = false)
        {
            ValidateInputType(type);

            return GetProductCountOfTypeInternal(type, onlySameType);
        }

        public IEnumerable<ItemAmount> GetProductsAmountsOfType<T>() where T : IProduct
            => GetProductsAmountsOfTypeInternal(typeof(T));

        public IEnumerable<ItemAmount> GetProductsAmountsOfType(Type type)
        {
            ValidateInputType(type);

            return GetProductsAmountsOfTypeInternal(type);
        }

        protected void AddItemWithoutNotify(IProduct item)
        {
            items.Add(item);

            Type type = item.GetType();
            if (itemsCounts.ContainsKey(type))
                itemsCounts[type]++;
            else
                itemsCounts.Add(type, 1);

            SubscribeIfDisposeNotifierItem(item);
        }

        protected void RemoveItemWithoutNotify(IProduct item)
        {
            Assert.That(
                items.Remove(item));

            Type type = item.GetType();
            if (itemsCounts[type] > 1)
                itemsCounts[type]--;
            else
                itemsCounts.Remove(type);

            UnsubscribeIfDisposeNotifierItem(item);
        }

        protected void InvokeItemsChangedEvent()
        {
            ItemsChanged?.Invoke(this);
        }

        private int GetProductCountOfTypeInternal(Type type, bool onlySameType)
        {
            if (!onlySameType)
                return itemsCounts
                    .Where(pair => type.IsAssignableFrom(pair.Key))
                    .Sum(pair => pair.Value);

            return itemsCounts.ContainsKey(type) ? itemsCounts[type] : 0;
        }

        private IEnumerable<ItemAmount> GetProductsAmountsOfTypeInternal(Type type)
        {
            return itemsCounts
                .Where(pair => type.IsAssignableFrom(pair.Key))
                .Select(pair => new ItemAmount(pair.Key, pair.Value));
        }

        private void ValidateInputType(Type type)
        {
            if (!typeof(IProduct).IsAssignableFrom(type))
                Assert.Fail();
        }

        private void SubscribeIfDisposeNotifierItem(IProduct item)
        {
            if (item is IDisposeNotifier disposeNotifier)
                disposeNotifier.Disposing += OnItemDisposing;
        }

        private void UnsubscribeIfDisposeNotifierItem(IProduct item)
        {
            if (item is IDisposeNotifier disposeNotifier)
                disposeNotifier.Disposing -= OnItemDisposing;
        }

        private void OnItemDisposing(IDisposeNotifier disposeNotifier)
        {
            RemoveItem((IProduct)disposeNotifier);
        }
    }
}
