using System;

namespace ConnectIt.Infrastructure.CreatedObjectNotifiers
{
    public interface ICreatedObjectNotifier<T>
    {
        public event Action<T> Created;

        public void SendNotification(T createdObject);
    }
}
