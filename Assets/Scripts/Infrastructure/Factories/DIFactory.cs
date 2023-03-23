using System.Collections.Generic;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class DIFactory<T> : IFactory<T>, IValidatable
    {
        protected readonly IInstantiator instantiator;

        public DIFactory(IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        public T Create() => instantiator.Instantiate<T>();

        public T Create(IEnumerable<object> args) => instantiator.Instantiate<T>(args);

        public virtual void Validate()
        {
            instantiator.Instantiate<T>();
        }
    }
}
