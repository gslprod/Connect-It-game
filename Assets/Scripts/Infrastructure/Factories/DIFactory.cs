using System.Collections.Generic;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class DIFactory<T> : IFactory<T>
    {
        protected readonly IInstantiator instantiator;

        public DIFactory(IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        public T Create() => CreateInternal();

        public T Create(IEnumerable<object> args) => CreateInternal(args);

        protected T CreateInternal() => instantiator.Instantiate<T>();

        protected T CreateInternal(IEnumerable<object> args) => instantiator.Instantiate<T>(args);
    }
}
