using System;
using System.Collections;
using System.Collections.Generic;

namespace ConnectIt.Utilities.Collections
{
    public class ClosedList<T> : IEnumerable<T>
    {
        private readonly List<T> _elementsList;

        public ClosedList(IEnumerable<T> elements,
            Action<T> addAction = null,
            Action<T> removeAction = null,
            Action<T, int> insertAction = null)
        {
            _elementsList = new(elements);

            if (addAction != null)
                addAction += AddItem;
            if (removeAction != null)
                removeAction += RemoveItem;
            if (insertAction != null)
                insertAction += InsertItem;
        }

        public ClosedList(T firstElement,
            Action<T> addAction = null,
            Action<T> removeAction = null,
            Action<T, int> insertAction = null)
        {
            _elementsList = new() { firstElement };

            if (addAction != null)
                addAction += AddItem;
            if (removeAction != null)
                removeAction += RemoveItem;
            if (insertAction != null)
                insertAction += InsertItem;
        }

        public ClosedList(Action<T> addAction = null,
            Action<T> removeAction = null,
            Action<T, int> insertAction = null)
        {
            _elementsList = new();

            if (addAction != null)
                addAction += AddItem;
            if (removeAction != null)
                removeAction += RemoveItem;
            if (insertAction != null)
                insertAction += InsertItem;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => _elementsList.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => _elementsList.GetEnumerator();

        public T this[int index]
            => _elementsList[index];

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
