using System.Collections.Generic;

namespace ConnectIt.Infrastructure.Factories
{
    public interface IFactory<out T>
    {
        T Create();
        T Create(IEnumerable<object> args);
    }
}
