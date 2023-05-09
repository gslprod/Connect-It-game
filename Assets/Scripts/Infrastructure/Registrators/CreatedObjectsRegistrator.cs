using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.Dispose;
using System;
using System.Collections.Generic;
using Zenject;

namespace ConnectIt.Infrastructure.Registrators
{
    public class CreatedObjectsRegistrator<TRegistrable> : IRegistrator<TRegistrable>, IInitializable, IDisposable
    {
        public IEnumerable<TRegistrable> Registrations => registrations;

        protected readonly ICreatedObjectNotifier<TRegistrable> createdObjectNotifier;
        protected readonly List<TRegistrable> registrations = new();

        public CreatedObjectsRegistrator(ICreatedObjectNotifier<TRegistrable> createdObjectNotifier)
        {
            this.createdObjectNotifier = createdObjectNotifier;
        }

        public void Initialize()
        {
            createdObjectNotifier.Created += OnObjectCreated;
        }

        public void Dispose()
        {
            createdObjectNotifier.Created -= OnObjectCreated;

            foreach (var registration in registrations)
            {
                if (registration is IDisposeNotifier disposeNotifier)
                    UnsubscribeFromDisposeNotifier(disposeNotifier);
            }
        }

        private void OnObjectCreated(TRegistrable obj)
        {
            registrations.Add(obj);

            if (obj is IDisposeNotifier disposeNotifier)
                SubscribeToDisposeNotifier(disposeNotifier);
        }

        private void OnDisposeNotify(IDisposeNotifier obj)
        {
            UnsubscribeFromDisposeNotifier(obj);

            registrations.Remove((TRegistrable)obj);
        }

        private void SubscribeToDisposeNotifier(IDisposeNotifier disposeNotifier)
        {
            disposeNotifier.Disposing += OnDisposeNotify;
        }

        private void UnsubscribeFromDisposeNotifier(IDisposeNotifier disposeNotifier)
        {
            disposeNotifier.Disposing -= OnDisposeNotify;
        }
    }
}
