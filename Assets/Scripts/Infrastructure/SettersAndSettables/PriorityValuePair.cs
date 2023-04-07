using System;

namespace ConnectIt.Infrastructure.SettersAndSettables
{
    public class PriorityValuePair<TPriority, TValue>
        where TPriority : Enum
    {
        public TPriority Priority { get; }
        public TValue Value { get; }

        public PriorityValuePair(TPriority priority, TValue value)
        {
            Priority = priority;
            Value = value;
        }
    }
}
