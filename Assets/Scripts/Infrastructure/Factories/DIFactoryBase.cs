using System.Collections.Generic;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public abstract class DIFactoryBase<T> : IDIFactory
    {
        protected readonly IInstantiator instantiator;

        public DIFactoryBase(IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        protected T CreateInternal() => instantiator.Instantiate<T>();

        protected T CreateInternal(IEnumerable<object> args) => instantiator.Instantiate<T>(args);

        public virtual void Validate()
        {
            instantiator.Instantiate<T>();
        }
    }
}
