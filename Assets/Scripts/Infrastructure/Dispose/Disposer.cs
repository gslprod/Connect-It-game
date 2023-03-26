using ConnectIt.Infrastructure.Registrators;
using System;
using System.Linq;

namespace ConnectIt.Infrastructure.Dispose
{
    public class Disposer<TDisposable> : IDisposable where TDisposable : IDisposable
    {
        protected readonly IRegistrator<TDisposable> registrator;

        public Disposer(IRegistrator<TDisposable> registrator)
        {
            this.registrator = registrator;
        }

        public void Dispose()
        {
            int count = registrator.Registrations.Count() - 1;

            for (int i = count - 1; i >= 0; i--)
            {
                TDisposable disposable = registrator.Registrations.ElementAt(i);

                disposable.Dispose();
            }
        }
    }
}
