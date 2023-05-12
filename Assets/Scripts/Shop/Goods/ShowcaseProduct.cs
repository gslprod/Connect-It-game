using System;

namespace ConnectIt.Shop.Goods
{
    public class ShowcaseProduct<T>
    {
        public long Price { get; }
        public T ShowcaseItem { get; }

        private readonly Func<T> _newItemsGetter;

        public ShowcaseProduct(
            T item,
            long price,
            Func<T> newItemsGetter)
        {
            ShowcaseItem = item;
            Price = price;
            _newItemsGetter = newItemsGetter;
        }

        public T GetNewInstance()
        {
            return _newItemsGetter();
        }
    }
}
