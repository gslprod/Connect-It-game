using UnityEngine;

namespace ConnectIt.Infrastructure.Factories
{
    public interface IUnityObjectFactory<T> where T : Object
    {
        bool PrefabLoaded { get; }

        T Create();
        T Create(Transform parent);
        T Create(Transform parent, Vector3 position, Quaternion rotation);
        void LoadPrefab(T prefab);
    }
}
