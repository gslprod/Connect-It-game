using ConnectIt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public abstract class DIFactoryBase<T> : IDIFactory
    {
        protected abstract IEnumerable<Type> ParamTypes { get; }

        protected IInstantiator Instantiator { get; private set; }

        private DiContainer _diContainer;

        [Inject]
        void Constructor(IInstantiator instantiator,
            DiContainer diContainer)
        {
            _diContainer = diContainer;
            Instantiator = instantiator;
        }

        protected T CreateInternal() => Instantiator.Instantiate<T>();

        protected T CreateInternal(IEnumerable<object> args)
        {
            Assert.That(ParamTypes.Count() == args.Count());

            int count = args.Count();
            TypeValuePair[] pairs = new TypeValuePair[count];

            for (int i = 0; i < count; i++)
                pairs[i] = new(ParamTypes.ElementAt(i), args.ElementAt(i));

            return _diContainer.InstantiateExplicit<T>(pairs.ToList());
        }

        public void Validate()
        {
            _diContainer.InstantiateExplicit(typeof(T), ValidationUtil.CreateDefaultArgs(ParamTypes.ToArray()));
        }
    }
}
