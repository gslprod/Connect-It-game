﻿using ConnectIt.Shop.Goods;
using ConnectIt.Utilities;
using System;
using System.Collections.Generic;

namespace ConnectIt.Shop.Customer
{
    public class Storage : IStorage
    {
        public event Action<IStorage> ItemsChanged;

        public IEnumerable<IProduct> Items => _items;

        private List<IProduct> _items = new();

        public void AddItem(IProduct item)
        {
            Assert.ArgIsNotNull(item);

            _items.Add(item);
            ItemsChanged?.Invoke(this);
        }

        public void RemoveItem(IProduct item)
        {
            Assert.ArgIsNotNull(item);

            Assert.That(
                _items.Remove(item));
            ItemsChanged?.Invoke(this);
        }
    }
}
