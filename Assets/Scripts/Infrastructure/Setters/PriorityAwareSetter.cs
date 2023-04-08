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

        private readonly List<ValueSource<TValue>> _valueSources = new();

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

        public void SetValue(TValue value, int priority, object source)
        {
            Assert.ArgIsNotNull(source);

            ValueSource<TValue> newPair = new(priority, value, source);

            if (_valueSources.Count == 0)
            {
                _valueSources.Add(newPair);
                UpdateValue();

                return;
            }

            int affectedIndex = -1;
            bool insertNeed = true;
            for (int i = 0; i < _valueSources.Count; i++)
            {
                ValueSource<TValue> valueSource = _valueSources[i];

                if (affectedIndex == -1 &&
                    valueSource.Priority <= priority)
                {
                    affectedIndex = i;
                }

                if (valueSource.Source == source)
                {
                    valueSource.Value = value;
                    valueSource.Priority = priority;

                    affectedIndex = i;
                    insertNeed = false;

                    break;
                }
            }

            if (insertNeed)
            {
                if (affectedIndex == -1)
                    affectedIndex = _valueSources.Count - 1;

                _valueSources.Insert(affectedIndex, newPair);
            }

            if (affectedIndex == 0)
                UpdateValue();
        }

        public void ResetValue(object source)
        {
            Assert.ArgIsNotNull(source);

            int affectedIndex = _valueSources.FindIndex(
                pair => pair.Source == source);

            _valueSources.RemoveAt(affectedIndex);

            if (affectedIndex == 0)
                UpdateValue();
        }

        private void Initialize()
        {
            SetValue(_defaultValue);
        }

        private void UpdateValue()
        {
            TValue toSet = _valueSources.Count > 0 ? _valueSources[0].Value : _defaultValue;

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
