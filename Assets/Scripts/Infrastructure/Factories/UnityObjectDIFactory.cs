using ConnectIt.Utilities;
using UnityEngine;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class UnityObjectDIFactory<T> : IUnityObjectFactory<T> where T : Object
    {
        public bool PrefabLoaded => _prefab != null;

        protected readonly IInstantiator instantiator;

        private T _prefab;

        public UnityObjectDIFactory(IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        public T Create()
        {
            Assert.That(PrefabLoaded);

            return instantiator.InstantiatePrefabForComponent<T>(_prefab);
        }

        public T Create(Transform parent)
        {
            Assert.IsNotNull(parent);
            Assert.That(PrefabLoaded);

            return instantiator.InstantiatePrefabForComponent<T>(_prefab, parent);
        }

        public T Create(Transform parent, Vector3 position, Quaternion rotation)
        {
            Assert.IsNotNull(parent);
            Assert.That(PrefabLoaded);

            return instantiator.InstantiatePrefabForComponent<T>(_prefab, position, rotation, parent);
        }

        public void LoadPrefab(T prefab)
        {
            Assert.IsNotNull(prefab);

            _prefab = prefab;
        }
    }
}
