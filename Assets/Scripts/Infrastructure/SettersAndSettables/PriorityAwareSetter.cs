using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace ConnectIt.Infrastructure.SettersAndSettables
{
    public class PriorityAwareSetter<TSettable, TValue, TPriority>
        where TSettable : ISettable<TValue>
        where TPriority : Enum
    {
        public TValue Value => _settable.SettableValue;

        private readonly TSettable _settable;
        private readonly TValue _defaultValue;

        private List<PriorityValuePair<TPriority, TValue>> _prioritiesAndValues;

        public PriorityAwareSetter(TSettable settable,
            TValue defaultValue)
        {
            _settable = settable;
            _defaultValue = defaultValue;
        }

        public void SetValue(TValue value, TPriority priority)
        {
            PriorityValuePair<TPriority, TValue> newPair = new(priority, value);

            if (_prioritiesAndValues.Count == 0)
            {
                _prioritiesAndValues.Add(newPair);
                UpdateValue();

                return;
            }

            int insertIndex = _prioritiesAndValues.FindIndex(
                pair => pair.Priority.ToInt() <= priority.ToInt());

            _prioritiesAndValues.Insert(insertIndex, newPair);

            if (insertIndex == 0)
                UpdateValue();
        }

        public void ResetValueWithPriority(TPriority priority)
        {
            int index = _prioritiesAndValues.FindIndex(
                pair => EqualityComparer<TPriority>.Default.Equals(pair.Priority, priority));

            _prioritiesAndValues.RemoveAt(index);

            if (index == 0)
                UpdateValue();
        }

        private void UpdateValue()
        {
            TValue toSet = _prioritiesAndValues.Count > 0 ? _prioritiesAndValues[0].Value : _defaultValue;
            if (EqualityComparer<TValue>.Default.Equals(toSet, Value))
                return;

            _settable.SettableValue = toSet;
        }
    }
}
