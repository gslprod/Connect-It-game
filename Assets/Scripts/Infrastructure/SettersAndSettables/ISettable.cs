namespace ConnectIt.Infrastructure.SettersAndSettables
{
    public interface ISettable<TValue>
    {
        public TValue SettableValue { get; set; }
    }
}
