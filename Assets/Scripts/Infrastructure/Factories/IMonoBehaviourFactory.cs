using UnityEngine;

namespace ConnectIt.Infrastructure.Factories
{
    public interface IMonoBehaviourFactory<T> where T : MonoBehaviour
    {
        bool PrefabLoaded { get; }

        T Create();
        T Create(Transform parent);
        T Create(Transform parent, Vector3 position, Quaternion rotation);
        void Destroy(T instance);
        void DestroyWithGameObject(T instance);
        void LoadPrefab(T prefab);
    }
}
