using System;

namespace ConnectIt.Infrastructure.Dispose
{
    public interface IDisposeNotifier : IDisposable
    {
        event Action<IDisposeNotifier> Disposing;
    }

    public interface IDisposeNotifier<T> : IDisposeNotifier
        where T : IDisposeNotifier
    {
        new event Action<T> Disposing;
        event Action<IDisposeNotifier> IDisposeNotifier.Disposing { add => Disposing += (boost) => { value(boost); }; remove => Disposing -= (boost) => { value(boost); }; }
    }
}
