using ConnectIt.Utilities;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public abstract class MonoBehaviourDIFactoryBase<T> : IDIFactory, IMonoBehaviourFactory<T> where T : MonoBehaviour
    {
        protected readonly IInstantiator instantiator;
        protected readonly T prefab;

        public MonoBehaviourDIFactoryBase(IInstantiator instantiator,
            T prefab)
        {
            this.instantiator = instantiator;
            this.prefab = prefab;
        }

        public virtual T Create()
        {
            return CreateInternal();
        }

        public virtual T Create(T original)
        {
            return CreateInternal(original);
        }

        public virtual T Create(Transform parent)
        {
            return CreateInternal(parent);
        }

        public virtual T Create(T original, Transform parent)
        {
            return CreateInternal(original, parent);
        }

        public virtual T Create(Transform parent, Vector3 position, Quaternion rotation)
        {
            return CreateInternal(parent, position, rotation);
        }

        public virtual T Create(T original, Transform parent, Vector3 position, Quaternion rotation)
        {
            return CreateInternal(original, parent, position, rotation);
        }

        protected T CreateInternal()
        {
            return instantiator.InstantiatePrefabForComponent<T>(prefab);
        }

        protected T CreateInternal(IEnumerable<object> args)
        {
            Assert.IsNotNull(args);

            return instantiator.InstantiatePrefabForComponent<T>(prefab, args);
        }

        protected T CreateInternal(T customPrefab)
        {
            Assert.IsNotNull(customPrefab);

            return instantiator.InstantiatePrefabForComponent<T>(customPrefab);
        }

        protected T CreateInternal(T customPrefab, IEnumerable<object> args)
        {
            Assert.IsNotNull(customPrefab);
            Assert.IsNotNull(args);

            return instantiator.InstantiatePrefabForComponent<T>(customPrefab, args);
        }

        protected T CreateInternal(Transform parent)
        {
            Assert.IsNotNull(parent);

            return instantiator.InstantiatePrefabForComponent<T>(prefab, parent);
        }

        protected T CreateInternal(Transform parent, IEnumerable<object> args)
        {
            Assert.IsNotNull(parent);
            Assert.IsNotNull(args);

            return instantiator.InstantiatePrefabForComponent<T>(prefab, parent, args);
        }

        protected T CreateInternal(T customPrefab, Transform parent)
        {
            Assert.IsNotNull(parent);
            Assert.IsNotNull(customPrefab);

            return instantiator.InstantiatePrefabForComponent<T>(customPrefab, parent);
        }

        protected T CreateInternal(T customPrefab, Transform parent, IEnumerable<object> args)
        {
            Assert.IsNotNull(parent);
            Assert.IsNotNull(customPrefab);
            Assert.IsNotNull(args);

            return instantiator.InstantiatePrefabForComponent<T>(customPrefab, parent, args);
        }

        protected T CreateInternal(Transform parent, Vector3 position, Quaternion rotation)
        {
            Assert.IsNotNull(parent);

            return instantiator.InstantiatePrefabForComponent<T>(prefab, position, rotation, parent);
        }

        protected T CreateInternal(Transform parent, Vector3 position, Quaternion rotation, IEnumerable<object> args)
        {
            Assert.IsNotNull(parent);
            Assert.IsNotNull(args);

            return instantiator.InstantiatePrefabForComponent<T>(prefab, position, rotation, parent, args);
        }

        protected T CreateInternal(T customPrefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            Assert.IsNotNull(customPrefab);
            Assert.IsNotNull(parent);

            return instantiator.InstantiatePrefabForComponent<T>(customPrefab, position, rotation, parent);
        }

        protected T CreateInternal(T customPrefab, Transform parent, Vector3 position, Quaternion rotation, IEnumerable<object> args)
        {
            Assert.IsNotNull(customPrefab);
            Assert.IsNotNull(parent);
            Assert.IsNotNull(args);

            return instantiator.InstantiatePrefabForComponent<T>(customPrefab, position, rotation, parent, args);
        }

        public virtual void Validate()
        {
            instantiator.InstantiatePrefabForComponent<T>(prefab);
        }
    }
}