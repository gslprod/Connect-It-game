using ConnectIt.Utilities;
using System;
using System.Collections.Generic;

namespace ConnectIt.Infrastructure.Setters
{
    public class PriorityAwareSetter<TValue>
    {
        private readonly Action<TValue> _setter;
        private readonly Func<TValue> _getter;
        private readonly TValue _defaultValue;

        private readonly List<PriorityValuePair<TValue>> _prioritiesAndValues = new();

        public PriorityAwareSetter(Action<TValue> setter,
            Func<TValue> getter,
            TValue defaultValue)
        {
            Assert.ArgsIsNotNull(getter, setter);

            _setter = setter;
            _getter = getter;
            _defaultValue = defaultValue;

            Initialize();
        }

        public void SetValue(TValue value, int priority)
        {
            PriorityValuePair<TValue> newPair = new(priority, value);

            if (_prioritiesAndValues.Count == 0)
            {
                _prioritiesAndValues.Add(newPair);
                UpdateValue();

                return;
            }

            int insertIndex = _prioritiesAndValues.FindIndex(
                pair => pair.Priority <= priority);

            Assert.That(priority != _prioritiesAndValues[insertIndex].Priority);

            _prioritiesAndValues.Insert(insertIndex, newPair);

            if (insertIndex == 0)
                UpdateValue();
        }

        public void ResetValueWithPriority(int priority)
        {
            int index = _prioritiesAndValues.FindIndex(
                pair => pair.Priority == priority);

            _prioritiesAndValues.RemoveAt(index);

            if (index == 0)
                UpdateValue();
        }

        private void Initialize()
        {
            SetValue(_defaultValue);
        }

        private void UpdateValue()
        {
            TValue toSet = _prioritiesAndValues.Count > 0 ? _prioritiesAndValues[0].Value : _defaultValue;

            if (EqualityComparer<TValue>.Default.Equals(toSet, GetValue()))
                return;

            SetValue(toSet);
        }

        private void SetValue(TValue value)
            => _setter(value);

        private TValue GetValue()
            => _getter();
    }
}
