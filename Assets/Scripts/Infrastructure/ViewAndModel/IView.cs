namespace ConnectIt.Infrastructure.ViewAndModel
{
    public interface IView<TModel>
    {
        TModel Model { get; }

        public void Init(TModel model);
    }
}
