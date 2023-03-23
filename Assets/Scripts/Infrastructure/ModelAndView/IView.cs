namespace ConnectIt.Infrastructure.ModelAndView
{
    public interface IView<TModel>
    {
        TModel Model { get; }

        public void Init(TModel model);
    }
}
