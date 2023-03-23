using UnityEngine;

namespace ConnectIt.Infrastructure.ViewAndModel
{
    public abstract class MonoBehaviourView<TModel> : MonoBehaviour, IView<TModel>
    {
        public abstract TModel Model { get; }

        public abstract void Init(TModel model);
    }
}