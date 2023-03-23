using ConnectIt.Infrastructure.ModelAndView;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public interface IViewFromModelFactory<in TModel, out TView> : IFactory<TModel, TView> where TView : IView<TModel> { }
}
