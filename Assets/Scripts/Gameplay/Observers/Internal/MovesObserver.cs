using ConnectIt.Gameplay.Model;
using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using System;
using Zenject;

namespace ConnectIt.Gameplay.Observers.Internal
{
    public class MovesObserver : IInitializable, IDisposable
    {
        public event Action MovesCountChanged;

        public int MovesCount { get; private set; } = 0;

        private readonly ICreatedObjectNotifier<ConnectionLine> _createdConnectionLineNotifier;

        public MovesObserver(
            ICreatedObjectNotifier<ConnectionLine> createdConnectionLineNotifier)
        {
            _createdConnectionLineNotifier = createdConnectionLineNotifier;
        }

        public void Initialize()
        {
            _createdConnectionLineNotifier.Created += OnConnectionLineCreated;
        }

        public void Dispose()
        {
            _createdConnectionLineNotifier.Created -= OnConnectionLineCreated;
        }

        private void OnConnectionLineCreated(ConnectionLine line)
        {
            MovesCount++;
            MovesCountChanged?.Invoke();
        }
    }
}
