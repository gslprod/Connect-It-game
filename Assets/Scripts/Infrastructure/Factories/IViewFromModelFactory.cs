using ConnectIt.Infrastructure.ViewAndModel;

namespace ConnectIt.Infrastructure.Factories
{
    public interface IViewFromModelFactory<TModel, TView> where TView : IView<TModel>
    {
        TView Create(TModel model);
        void Destroy(TModel model);
    }
}
