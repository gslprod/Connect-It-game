using ConnectIt.Infrastructure.Dispose;
using ConnectIt.Localization;
using ConnectIt.Utilities;
using System;
using Zenject;

namespace ConnectIt.Shop.Goods.Boosts
{
    public abstract class Boost : IProduct, IReversibleDisposableItem, IDisposeNotifier<Boost>, IInitializable
    {
        public event Action<Boost> Disposing;
        public event Action<Boost> ReversibleDisposeChanged;

        public abstract TextKey Name { get; }
        public abstract TextKey Description { get; }

        public bool ReversibleDisposed { get; private set; }

        public virtual void Use()
        {
            ReversibleDispose();
        }

        public virtual void Initialize() { }

        public virtual void Dispose()
        {
            Disposing?.Invoke(this);
        }

        public void CancelReversibleDispose()
        {
            Assert.That(ReversibleDisposed);

            ReversibleDisposed = false;
            ReversibleDisposeChanged?.Invoke(this);
        }

        public void ReversibleDispose()
        {
            Assert.That(!ReversibleDisposed);

            ReversibleDisposed = true;
            ReversibleDisposeChanged?.Invoke(this);
        }
    }
}
