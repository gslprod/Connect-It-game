using ConnectIt.Utilities;
using UnityEngine;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public class MonoBehaviourDIFactory<T> : IMonoBehaviourFactory<T> where T : MonoBehaviour
    {
        public bool PrefabLoaded => _prefab != null;

        protected readonly IInstantiator instantiator;

        protected T _prefab;

        public MonoBehaviourDIFactory(IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        public virtual T Create()
        {
            Assert.That(PrefabLoaded);

            return instantiator.InstantiatePrefabForComponent<T>(_prefab);
        }

        public virtual T Create(Transform parent)
        {
            Assert.IsNotNull(parent);
            Assert.That(PrefabLoaded);

            return instantiator.InstantiatePrefabForComponent<T>(_prefab, parent);
        }

        public virtual T Create(Transform parent, Vector3 position, Quaternion rotation)
        {
            Assert.IsNotNull(parent);
            Assert.That(PrefabLoaded);

            return instantiator.InstantiatePrefabForComponent<T>(_prefab, position, rotation, parent);
        }

        public virtual void Destroy(T instance)
        {
            Assert.IsNotNull(instance);

            Object.Destroy(instance);
        }

        public virtual void DestroyWithGameObject(T instance)
        {
            Assert.IsNotNull(instance);

            Object.Destroy(instance.gameObject);
        }

        public virtual void LoadPrefab(T prefab)
        {
            Assert.IsNotNull(prefab);

            _prefab = prefab;
        }
    }
}
