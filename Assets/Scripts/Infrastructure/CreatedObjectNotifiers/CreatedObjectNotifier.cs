using System;

namespace ConnectIt.Infrastructure.CreatedObjectNotifiers
{
    public class CreatedObjectNotifier<T> : ICreatedObjectNotifier<T>
    {
        public event Action<T> Created;

        public void SendNotification(T createdObject)
        {
            Created?.Invoke(createdObject);
        }
    }
}
