using UnityEngine;

namespace ConnectIt.Infrastructure.Factories
{
    public interface IMonoBehaviourFactory<T> : IUnityObjectFactory<T> where T : MonoBehaviour
    {
        T Create(Transform parent);
        T Create(T original, Transform parent);
        T Create(Transform parent, Vector3 position, Quaternion rotation);
        T Create(T original, Transform parent, Vector3 position, Quaternion rotation);
    }
}
