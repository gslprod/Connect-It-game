using ConnectIt.Infrastructure.ViewAndModel;
using ConnectIt.Utilities;
using System.Collections.Generic;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class MonoBehaviourViewFromModelDIFactory<TModel, TView> : MonoBehaviourDIFactory<TView>, IViewFromModelFactory<TModel, TView> 
        where TView : MonoBehaviourView<TModel>
    {
        private readonly Dictionary<TModel, TView> _views = new();

        public MonoBehaviourViewFromModelDIFactory(IInstantiator instantiator) : base(instantiator) { }

        public virtual TView Create(TModel model)
        {
            Assert.IsNotNull(model);

            TView view = Create();
            view.Init(model);

            _views.Add(model, view);

            return view;
        }

        public virtual void Destroy(TModel model)
        {
            Assert.IsNotNull(model);

            TView view = _views[model];

            _views.Remove(model);

            Destroy(view);
        }
    }
}
