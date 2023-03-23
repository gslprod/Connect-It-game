using ConnectIt.Infrastructure.ModelAndView;
using ConnectIt.Utilities;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class PrimitiveDIViewFromModelFactory<TModel, TView> : DIFactoryBase<TView>, IViewFromModelFactory<TModel, TView> where TView : IView<TModel>
    {
        public PrimitiveDIViewFromModelFactory(IInstantiator instantiator) : base(instantiator) { }

        public virtual TView Create(TModel param)
        {
            Assert.IsNotNull(param);

            TView view = CreateInternal();
            view.Init(param);

            return view;
        }
    }
}
