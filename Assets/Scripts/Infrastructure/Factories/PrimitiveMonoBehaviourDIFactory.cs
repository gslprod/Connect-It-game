using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class PrimitiveMonoBehaviourDIFactory<TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TValue> 
        where TValue : MonoBehaviour
    {
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get { yield break; }
        }
    }

    public class PrimitiveMonoBehaviourDIFactory<TParam1, TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TParam1, TValue>
        where TValue : MonoBehaviour
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
            return CreateInternal(new object[] { param });
        }
    }

    public class PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TParam1, TParam2, TValue>
        where TValue : MonoBehaviour
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
            return CreateInternal(new object[] { param1, param2 });
        }
    }

    public class PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TValue>
        where TValue : MonoBehaviour
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
            return CreateInternal(new object[] { param1, param2, param3 });
        }
    }

    public class PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TParam4, TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TValue>
        where TValue : MonoBehaviour
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
            return CreateInternal(new object[] { param1, param2, param3, param4 });
        }
    }

    public class PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        where TValue : MonoBehaviour
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
            return CreateInternal(new object[] { param1, param2, param3, param4, param5 });
        }
    }
}
