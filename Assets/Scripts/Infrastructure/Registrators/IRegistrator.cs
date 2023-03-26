using System.Collections.Generic;

namespace ConnectIt.Infrastructure.Registrators
{
    public interface IRegistrator<TRegistrable>
    {
        public IEnumerable<TRegistrable> Registrations { get; }
    }
}
