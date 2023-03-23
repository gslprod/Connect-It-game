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

            if (connection.ConnectionCompleted)
            {
                IConnectable other = connection.GetOther(this);

                Assert.That(CompatibleWith(other));
            }

            Connection = connection;

            ConnectionChanged?.Invoke(this);
        }

        public void ResetConnection()
        {
            Assert.That(HasConnection);

            Connection = null;

            ConnectionChanged?.Invoke(this);
        }

        public bool CanBeConnectedWith(IConnectable other)
        {
            Assert.IsNotNull(other);

            bool alreadyConnected = HasConnection && Connection.ConnectionCompleted;
            if (alreadyConnected)
                return false;

            bool otherAlreadyConnected = other.HasConnection && other.Connection.ConnectionCompleted;
            if (otherAlreadyConnected)
                return false;

            return CompatibleWith(other);
        }

        public bool CompatibleWith(IConnectable other)
        {
            Assert.IsNotNull(other);

            return
                this != other &&
                CompatibilityIndex == other.CompatibilityIndex;
        }
    }
}