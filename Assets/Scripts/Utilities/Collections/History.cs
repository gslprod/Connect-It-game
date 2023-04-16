using System.Collections;
using System.Collections.Generic;

namespace ConnectIt.Utilities.Collections
{
    public class History<T> : IEnumerable<T>
    {
        private readonly List<T> _historyList;

        public History()
        {
            _historyList = new();
        }

        public History(int capacity)
        {
            _historyList = new(capacity);
        }

        public History(IEnumerable<T> collection)
        {
            _historyList = new(collection);
        }

        public IEnumerator<T> GetEnumerator()
            => _historyList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _historyList.GetEnumerator();

        public bool TryGetRecentAndForget(out T element)
        {
            if (!TryGetRecent(out element))
                return false;

            Forget(element);
            return true;
        }

        public bool TryGetRecent(out T element)
        {
            element = default;
            if (_historyList.Count == 0)
                return false;

            element = _historyList[^1];
            return true;
        }

        public bool TryGetOldestAndForget(out T element)
        {
            if (!TryGetOldest(out element))
                return false;

            Forget(element);
            return true;
        }

        public bool TryGetOldest(out T element)
        {
            element = default;
            if (_historyList.Count == 0)
                return false;

            element = _historyList[0];
            return true;
        }

        public void Save(T elementToSave)
        {
            int existingElementIndex = _historyList.FindIndex(element => EqualityComparer<T>.Default.Equals(elementToSave, element));
            if (existingElementIndex != -1)
                _historyList.RemoveAt(existingElementIndex);

            _historyList.Add(elementToSave);
        }

        public void Forget(T elementToForget)
        {
            Assert.That(
                _historyList.Remove(elementToForget));
        }

        public void ForgetAll()
        {
            _historyList.Clear();
        }
    }
}
