using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.ModelAndView;
using System;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class MonoBehaviourViewFromModelDIAutoFactory<TModel, TView> : MonoBehaviourViewFromModelDIFactory<TModel, TView>, IInitializable, IDisposable
        where TView : MonoBehaviourView<TModel>
    {
        protected readonly ICreatedObjectNotifier<TModel> createdModelNotifier;

        public MonoBehaviourViewFromModelDIAutoFactory(
            IInstantiator instantiator,
            TView prefab,
            ICreatedObjectNotifier<TModel> createdModelNotifier) : base(instantiator, prefab)
        {
            this.createdModelNotifier = createdModelNotifier;
        }

        public void Initialize()
        {
            createdModelNotifier.Created += OnModelCreated;
        }

        public void Dispose()
        {
            createdModelNotifier.Created -= OnModelCreated;
        }

        protected virtual void OnModelCreated(TModel model)
        {
            Create(model);
        }
    }
}
