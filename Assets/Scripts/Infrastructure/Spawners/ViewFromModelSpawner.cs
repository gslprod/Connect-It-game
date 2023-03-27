using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using System;
using Zenject;

namespace ConnectIt.Infrastructure.Spawners
{
    public class ViewFromModelSpawner<TModel, TView, TViewFactory> : IInitializable, IDisposable
        where TViewFactory : PlaceholderFactory<TModel, TView>
    {
        protected readonly TViewFactory viewFactory;
        protected readonly ICreatedObjectNotifier<TModel> createdModelNotifier;

        public ViewFromModelSpawner(
            TViewFactory viewFactory,
            ICreatedObjectNotifier<TModel> createdModelNotifier)
        {
            this.viewFactory = viewFactory;
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
            viewFactory.Create(model);
        }
    }
}
