using ConnectIt.Utilities;
using System;

namespace ConnectIt.Model
{
    public class Connectable : IConnectable
    {
        public event Action<Connectable> ConnectionChanged;

        public bool HasConnection => Connection != null;

        public Connection Connection { get; private set; }
        public int CompatibilityIndex { get; }

        public Connectable(int compatibilityIndex)
        {
            CompatibilityIndex = compatibilityIndex;
        }

        public void SetConnection(Connection connection)
        {
            Assert.IsNotNull(connection);
            Assert.That(!HasConnection);

            Connection = connection;

            ConnectionChanged?.Invoke(this);
        }

        public void ResetConnection()
        {
            Assert.That(HasConnection);

            Connection = null;

            ConnectionChanged?.Invoke(this);
        }
    }
}