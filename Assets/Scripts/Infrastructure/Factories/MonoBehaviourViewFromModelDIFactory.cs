using ConnectIt.Infrastructure.ModelAndView;
using ConnectIt.Utilities;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class MonoBehaviourViewFromModelDIFactory<TModel, TView> : MonoBehaviourDIFactory<TView>, IViewFromModelFactory<TModel, TView> 
        where TView : MonoBehaviourView<TModel>
    {
        public MonoBehaviourViewFromModelDIFactory(
            IInstantiator instantiator,
            TView prefab) : base(instantiator, prefab) { }

        public virtual TView Create(TModel model)
        {
            Assert.IsNotNull(model);

            TView view = Create();
            view.Init(model);

            return view;
        }
    }
}
