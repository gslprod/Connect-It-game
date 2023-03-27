using System;
using System.Collections.Generic;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class PrimitiveDIFactory<TValue> :
        DIFactoryBase<TValue>, IFactory<TValue>
    {
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get { yield break; }
        }

        public TValue Create()
        {
            TValue createdObject = CreateInternal();

            if (createdObject is IInitializable initializable)
                initializable.Initialize();

            return createdObject;
        }
    }

    public class PrimitiveDIFactory<TParam1, TValue> :
        DIFactoryBase<TValue>, IFactory<TParam1, TValue>
    {
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
            }
        }

        public virtual TValue Create(TParam1 param)
        {
            TValue createdObject = CreateInternal(new object[] { param });

            if (createdObject is IInitializable initializable)
                initializable.Initialize();

            return createdObject;
        }
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TValue> :
        DIFactoryBase<TValue>, IFactory<TParam1, TParam2, TValue>
    {
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
            }
        }

        public virtual TValue Create(TParam1 param1, TParam2 param2)
        {
            TValue createdObject = CreateInternal(new object[] { param1, param2 });

            if (createdObject is IInitializable initializable)
                initializable.Initialize();

            return createdObject;
        }
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TValue> :
        DIFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TValue>
    {
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
            }
        }

        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            TValue createdObject = CreateInternal(new object[] { param1, param2, param3 });

            if (createdObject is IInitializable initializable)
                initializable.Initialize();

            return createdObject;
        }
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TParam4, TValue> :
        DIFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TValue>
    {
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
            }
        }

        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            TValue createdObject = CreateInternal(new object[] { param1, param2, param3, param4 });

            if (createdObject is IInitializable initializable)
                initializable.Initialize();

            return createdObject;
        }
    }

    public class PrimitiveDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> :
        DIFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
    {
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield return typeof(TParam5);
            }
        }

        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            TValue createdObject = CreateInternal(new object[] { param1, param2, param3, param4, param5 });

            if (createdObject is IInitializable initializable)
                initializable.Initialize();

            return createdObject;
        }
    }
}