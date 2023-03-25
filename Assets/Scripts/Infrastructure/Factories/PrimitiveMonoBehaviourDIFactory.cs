using UnityEngine;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class PrimitiveMonoBehaviourDIFactory<TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TValue> 
        where TValue : MonoBehaviour
    {
        public PrimitiveMonoBehaviourDIFactory(IInstantiator instantiator,
            TValue prefab) : base(instantiator, prefab) { }
    }

    public class PrimitiveMonoBehaviourDIFactory<TParam1, TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TParam1, TValue>
        where TValue : MonoBehaviour
    {
        public PrimitiveMonoBehaviourDIFactory(IInstantiator instantiator,
            TValue prefab) : base(instantiator, prefab) { }

        public TValue Create(TParam1 param)
        {
            return CreateInternal(new object[] { param });
        }
    }

    public class PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TParam1, TParam2, TValue>
        where TValue : MonoBehaviour
    {
        public PrimitiveMonoBehaviourDIFactory(IInstantiator instantiator,
            TValue prefab) : base(instantiator, prefab) { }

        public TValue Create(TParam1 param1, TParam2 param2)
        {
            return CreateInternal(new object[] { param1, param2 });
        }
    }

    public class PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TValue>
        where TValue : MonoBehaviour
    {
        public PrimitiveMonoBehaviourDIFactory(IInstantiator instantiator,
            TValue prefab) : base(instantiator, prefab) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return CreateInternal(new object[] { param1, param2, param3 });
        }
    }

    public class PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TParam4, TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TValue>
        where TValue : MonoBehaviour
    {
        public PrimitiveMonoBehaviourDIFactory(IInstantiator instantiator,
            TValue prefab) : base(instantiator, prefab) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            return CreateInternal(new object[] { param1, param2, param3, param4 });
        }
    }

    public class PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> :
        MonoBehaviourDIFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        where TValue : MonoBehaviour
    {
        public PrimitiveMonoBehaviourDIFactory(IInstantiator instantiator,
            TValue prefab) : base(instantiator, prefab) { }

        public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            return CreateInternal(new object[] { param1, param2, param3, param4, param5 });
        }
    }
}
