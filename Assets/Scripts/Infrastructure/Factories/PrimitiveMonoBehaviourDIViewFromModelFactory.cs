using ConnectIt.Infrastructure.ModelAndView;
using ConnectIt.Utilities;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class PrimitiveMonoBehaviourDIViewFromModelFactory<TModel, TView> : MonoBehaviourDIFactory<TView>, IViewFromModelFactory<TModel, TView>
         where TView : MonoBehaviourView<TModel>
    {
        public PrimitiveMonoBehaviourDIViewFromModelFactory(IInstantiator instantiator, TView prefab) : base(instantiator, prefab) { }

        public virtual TView Create(TModel param)
        {
            Assert.IsNotNull(param);

            TView view = Create();
            view.Init(param);

            return view;
        }
    }
}
