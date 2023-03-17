using ConnectIt.Utilities;

namespace ConnectIt.Model
{
    public class Connectable : IConnectable
    {
        public bool Connected => Connection != null;

        public Connection Connection { get; private set; }
        public int CompatibilityIndex { get; }

        public Connectable(int compatibilityIndex)
        {
            CompatibilityIndex = compatibilityIndex;
        }

        public void SetConnection(Connection connection)
        {
            Assert.IsNotNull(connection);
            Assert.That(!Connected);

            Connection = connection;
        }

        public void ResetConnection()
        {
            Assert.That(Connected);

            Connection = null;
        }
    }
}