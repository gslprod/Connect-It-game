using System;

namespace ConnectIt.Infrastructure.Dispose
{
    public interface IDisposeNotifier : IDisposable
    {
        public event Action<IDisposeNotifier> Disposing;
    }

    public interface IDisposeNotifier<T> : IDisposable
    {
        public event Action<T> Disposing;
    }
}
