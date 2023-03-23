using UnityEngine;

namespace ConnectIt.Infrastructure.Factories
{
    public interface IUnityObjectFactory<T> where T : Object
    {
        T Create();
        T Create(T original);
    }
}