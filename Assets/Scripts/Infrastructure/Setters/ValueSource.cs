namespace ConnectIt.Infrastructure.Setters
{
    public class ValueSource<TValue>
    {
        public TValue Value { get; set; }
        public int Priority { get; set; }
        public object Source { get; }

        public ValueSource(int priority, TValue value, object source)
        {
            Priority = priority;
            Value = value;
            Source = source;
        }
    }
}
