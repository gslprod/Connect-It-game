using ConnectIt.Utilities;
using System;

namespace ConnectIt.Gameplay.Model
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
            Assert.ArgIsNotNull(connection);
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
            Assert.ArgIsNotNull(other);

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
            Assert.ArgIsNotNull(other);

            return
                this != other &&
                CompatibilityIndex == other.CompatibilityIndex;
        }

        public void Dispose()
        {
            if (HasConnection)
                ResetConnection();
        }
    }
}