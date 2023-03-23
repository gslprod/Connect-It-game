using ConnectIt.Utilities;
using UnityEngine;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class MonoBehaviourDIFactory<T> : IMonoBehaviourFactory<T>, IDIFactory where T : MonoBehaviour
    {
        protected readonly IInstantiator instantiator;
        protected readonly T prefab;

        public MonoBehaviourDIFactory(IInstantiator instantiator,
            T prefab)
        {
            this.instantiator = instantiator;
            this.prefab = prefab;
        }

        public T Create()
        {
            return instantiator.InstantiatePrefabForComponent<T>(prefab);
        }

        public virtual T Create(T original)
        {
            Assert.IsNotNull(original);

            return instantiator.InstantiatePrefabForComponent<T>(original);
        }

        public T Create(Transform parent)
        {
            Assert.IsNotNull(parent);

            return instantiator.InstantiatePrefabForComponent<T>(prefab, parent);
        }

        public virtual T Create(T original, Transform parent)
        {
            Assert.IsNotNull(original);
            Assert.IsNotNull(parent);

            return instantiator.InstantiatePrefabForComponent<T>(original, parent);
        }

        public T Create(Transform parent, Vector3 position, Quaternion rotation)
        {
            Assert.IsNotNull(parent);

            return instantiator.InstantiatePrefabForComponent<T>(prefab, parent);
        }

        public virtual T Create(T original, Transform parent, Vector3 position, Quaternion rotation)
        {
            Assert.IsNotNull(original);
            Assert.IsNotNull(parent);

            return instantiator.InstantiatePrefabForComponent<T>(original, position, rotation, parent);
        }

        public void Validate()
        {
            instantiator.InstantiatePrefabForComponent<T>(prefab);
        }
    }
}
