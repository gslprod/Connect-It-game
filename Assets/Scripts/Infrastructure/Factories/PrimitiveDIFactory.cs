using Assets.Scripts.Infrastructure.Factories;

namespace ConnectIt.Infrastructure.Factories
{
    public class PrimitiveDIFactory<TValue> :

        DIFactoryBase<TValue>,
        IFactory<TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create()

            => CreateInternal();
    }

    public class PrimitiveDIFactory<TParam1, TValue> :

        DIFactoryBase<TValue>,
        IFactory<TParam1, TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create(TParam1 param)

            => CreateInternal(new object[] { param });
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TValue> :

        DIFactoryBase<TValue>,
        IFactory<TParam1, TParam2, TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create(TParam1 param1, TParam2 param2)

            => CreateInternal(new object[] { param1, param2 });
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TValue> :

        DIFactoryBase<TValue>,
        IFactory<TParam1, TParam2, TParam3, TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3)

            => CreateInternal(new object[] { param1, param2, param3 });
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TParam4, TValue> :

        DIFactoryBase<TValue>,
        IFactory<TParam1, TParam2, TParam3, TParam4, TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)

            => CreateInternal(new object[] { param1, param2, param3, param4 });
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> :

        DIFactoryBase<TValue>,
        IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4,
            TParam5 param5)

            => CreateInternal(new object[] { param1, param2, param3, param4, param5 });
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6,
        TValue> :

        DIFactoryBase<TValue>,
        IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4,
            TParam5 param5, TParam6 param6)

            => CreateInternal(new object[] { param1, param2, param3, param4, param5, param6 });
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6,
        TParam7, TValue> :

        DIFactoryBase<TValue>,
        IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4,
            TParam5 param5, TParam6 param6, TParam7 param7)

            => CreateInternal(new object[] { param1, param2, param3, param4, param5, param6,
                param7 });
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6,
        TParam7, TParam8, TValue> :

        DIFactoryBase<TValue>,
        IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
            TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4,
            TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8)

            => CreateInternal(new object[] { param1, param2, param3, param4, param5, param6,
                param7, param8 });
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6,
        TParam7, TParam8, TParam9, TValue> :

        DIFactoryBase<TValue>,
        IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
            TParam9, TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4,
            TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9)

            => CreateInternal(new object[] { param1, param2, param3, param4, param5, param6,
                param7, param8, param9 });
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6,
        TParam7, TParam8, TParam9, TParam10, TValue> :

        DIFactoryBase<TValue>,
        IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
            TParam9, TParam10, TValue>
    {
        public PrimitiveDIFactory(Zenject.IInstantiator instantiator) : base(instantiator) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4,
            TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9,
            TParam10 param10)

            => CreateInternal(new object[] { param1, param2, param3, param4, param5, param6,
                param7, param8, param9, param10 });
    }
}
