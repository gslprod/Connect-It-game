using System;
using System.Collections;
using System.Collections.Generic;

namespace ConnectIt.Utilities.Collections
{
    public class ClosedList<T> : IEnumerable<T>
    {
        private readonly List<T> _elementsList;

        public ClosedList(IEnumerable<T> elements,
            ref Action<T> addAction,
            ref Action<T> removeAction,
            ref Action<T, int> insertAction)
        {
            _elementsList = new(elements);

            addAction += AddItem;
            removeAction += RemoveItem;
            insertAction += InsertItem;
        }

        public ClosedList(T firstElement,
            ref Action<T> addAction,
            ref Action<T> removeAction,
            ref Action<T, int> insertAction)
        {
            _elementsList = new() { firstElement };

            addAction += AddItem;
            removeAction += RemoveItem;
            insertAction += InsertItem;
        }

        public ClosedList(ref Action<T> addAction,
            ref Action<T> removeAction,
            ref Action<T, int> insertAction)
        {
            _elementsList = new();

            addAction += AddItem;
            removeAction += RemoveItem;
            insertAction += InsertItem;
        }

        public T this[int index]
            => _elementsList[index];

        public int IndexOf(T element)
        {
            int index = _elementsList.IndexOf(element);

            Assert.That(index != -1);

            return index;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => _elementsList.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => _elementsList.GetEnumerator();

        private void AddItem(T item)
        {
            _elementsList.Add(item);
        }

        private void RemoveItem(T item)
        {
            Assert.That(_elementsList.Remove(item));
        }

        private void InsertItem(T item, int index)
        {
            _elementsList.Insert(index, item);
        }
    }
}
