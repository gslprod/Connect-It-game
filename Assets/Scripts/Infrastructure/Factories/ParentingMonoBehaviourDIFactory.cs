using UnityEngine;

namespace ConnectIt.Infrastructure.Factories
{
    public class ParentingMonoBehaviourDIFactory<TValue> :
        PrimitiveMonoBehaviourDIFactory<TValue>
        where TValue : MonoBehaviour
    {
        protected readonly Transform parent;

        public ParentingMonoBehaviourDIFactory(Transform parent)
        {
            this.parent = parent;
        }

        public override TValue Create()
        {
            return CreateInternal(parent);
        }
    }

    public class ParentingMonoBehaviourDIFactory<TParam1, TValue> :
        PrimitiveMonoBehaviourDIFactory<TParam1, TValue>
        where TValue : MonoBehaviour
    {
        protected readonly Transform parent;

        public ParentingMonoBehaviourDIFactory(Transform parent)
        {
            this.parent = parent;
        }

        public override TValue Create(TParam1 param)
        {
            return CreateInternal(parent, new object[] { param });
        }
    }

    public class ParentingMonoBehaviourDIFactory<TParam1, TParam2, TValue> :
        PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TValue>
        where TValue : MonoBehaviour
    {
        protected readonly Transform parent;

        public ParentingMonoBehaviourDIFactory(Transform parent)
        {
            this.parent = parent;
        }

        public override TValue Create(TParam1 param1, TParam2 param2)
        {
            return CreateInternal(parent, new object[] { param1, param2 });
        }
    }

    public class ParentingMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TValue> :
        PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TValue>
        where TValue : MonoBehaviour
    {
        protected readonly Transform parent;

        public ParentingMonoBehaviourDIFactory(Transform parent)
        {
            this.parent = parent;
        }

        public override TValue Create(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return CreateInternal(parent, new object[] { param1, param2, param3 });
        }
    }

    public class ParentingMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TParam4, TValue> :
        PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TParam4, TValue>
        where TValue : MonoBehaviour
    {
        protected readonly Transform parent;

        public ParentingMonoBehaviourDIFactory(Transform parent)
        {
            this.parent = parent;
        }

        public override TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            return CreateInternal(parent, new object[] { param1, param2, param3, param4 });
        }
    }

    public class ParentingMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> :
        PrimitiveMonoBehaviourDIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        where TValue : MonoBehaviour
    {
        protected readonly Transform parent;

        public ParentingMonoBehaviourDIFactory(Transform parent)
        {
            this.parent = parent;
        }

        public override TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            return CreateInternal(parent, new object[] { param1, param2, param3, param4, param5 });
        }
    }
}
