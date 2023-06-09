﻿using ConnectIt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ConnectIt.Infrastructure.Factories
{
    public abstract class MonoBehaviourDIFactoryBase<T> : IDIFactory, IMonoBehaviourFactory<T> where T : MonoBehaviour
    {
        protected abstract IEnumerable<Type> ParamTypes { get; }

        protected IInstantiator Instantiator { get; private set; }
        protected T Prefab { get; private set; }

        private DiContainer _diContainer;

        [Inject]
        void Constructor([InjectOptional] T prefab,
            IInstantiator instantiator,
            DiContainer diContainer)
        {
            _diContainer = diContainer;
            Instantiator = instantiator;
            Prefab = prefab;
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
            return Instantiator.InstantiatePrefabForComponent<T>(Prefab);
        }

        protected T CreateInternal(IEnumerable<object> args)
        {
            Assert.ArgIsNotNull(args);

            return Instantiator.InstantiatePrefabForComponent<T>(Prefab, args);
        }

        protected T CreateInternal(T customPrefab)
        {
            Assert.ArgIsNotNull(customPrefab);

            return Instantiator.InstantiatePrefabForComponent<T>(customPrefab);
        }

        protected T CreateInternal(T customPrefab, IEnumerable<object> args)
        {
            Assert.ArgIsNotNull(customPrefab);
            Assert.ArgIsNotNull(args);

            return Instantiator.InstantiatePrefabForComponent<T>(customPrefab, args);
        }

        protected T CreateInternal(Transform parent)
        {
            Assert.ArgIsNotNull(parent);

            return Instantiator.InstantiatePrefabForComponent<T>(Prefab, parent);
        }

        protected T CreateInternal(Transform parent, IEnumerable<object> args)
        {
            Assert.ArgIsNotNull(parent);
            Assert.ArgIsNotNull(args);

            return Instantiator.InstantiatePrefabForComponent<T>(Prefab, parent, args);
        }

        protected T CreateInternal(T customPrefab, Transform parent)
        {
            Assert.ArgIsNotNull(parent);
            Assert.ArgIsNotNull(customPrefab);

            return Instantiator.InstantiatePrefabForComponent<T>(customPrefab, parent);
        }

        protected T CreateInternal(T customPrefab, Transform parent, IEnumerable<object> args)
        {
            Assert.ArgIsNotNull(parent);
            Assert.ArgIsNotNull(customPrefab);
            Assert.ArgIsNotNull(args);

            return Instantiator.InstantiatePrefabForComponent<T>(customPrefab, parent, args);
        }

        protected T CreateInternal(Transform parent, Vector3 position, Quaternion rotation)
        {
            Assert.ArgIsNotNull(parent);

            return Instantiator.InstantiatePrefabForComponent<T>(Prefab, position, rotation, parent);
        }

        protected T CreateInternal(Transform parent, Vector3 position, Quaternion rotation, IEnumerable<object> args)
        {
            Assert.ArgIsNotNull(parent);
            Assert.ArgIsNotNull(args);

            return Instantiator.InstantiatePrefabForComponent<T>(Prefab, position, rotation, parent, args);
        }

        protected T CreateInternal(T customPrefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            Assert.ArgIsNotNull(customPrefab);
            Assert.ArgIsNotNull(parent);

            return Instantiator.InstantiatePrefabForComponent<T>(customPrefab, position, rotation, parent);
        }

        protected T CreateInternal(T customPrefab, Transform parent, Vector3 position, Quaternion rotation, IEnumerable<object> args)
        {
            Assert.ArgIsNotNull(customPrefab);
            Assert.ArgIsNotNull(parent);
            Assert.ArgIsNotNull(args);

            return Instantiator.InstantiatePrefabForComponent<T>(customPrefab, position, rotation, parent, args);
        }

        public void Validate()
        {
            _diContainer.InstantiatePrefabForComponentExplicit(typeof(T), Prefab, ValidationUtil.CreateDefaultArgs(ParamTypes.ToArray()));
        }
    }
}