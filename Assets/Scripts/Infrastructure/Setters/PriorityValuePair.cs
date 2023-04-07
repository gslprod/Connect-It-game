using System;

namespace ConnectIt.Infrastructure.Setters
{
    public class PriorityValuePair<TValue>
    {
        public int Priority { get; }
        public TValue Value { get; }

        public PriorityValuePair(int priority, TValue value)
        {
            Priority = priority;
            Value = value;
        }
    }
}
