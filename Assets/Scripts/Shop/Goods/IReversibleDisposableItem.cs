using System;

namespace ConnectIt.Shop.Goods
{
    public interface IReversibleDisposableItem : IDisposable
    {
        bool ReversibleDisposed { get; }

        public void ReversibleDispose();
        public void CancelReversibleDispose();
    }
}
