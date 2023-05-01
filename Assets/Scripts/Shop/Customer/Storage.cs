using ConnectIt.Shop.Goods;
using ConnectIt.Utilities;
using System.Collections.Generic;

namespace ConnectIt.Shop.Customer
{
    public class Storage : IStorage
    {
        public IEnumerable<IProduct> Items => _items;

        private List<IProduct> _items;

        public void AddItem(IProduct item)
        {
            Assert.ArgIsNotNull(item);

            _items.Add(item);
        }

        public void RemoveItem(IProduct item)
        {
            Assert.ArgIsNotNull(item);

            Assert.That(
                _items.Remove(item));
        }
    }
}
