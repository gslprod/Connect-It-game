using ConnectIt.Utilities;

namespace ConnectIt.Model
{
    public class Connection
    {
        public bool ConnectionCompleted => First != null && Second != null;

        public IConnectable First { get; private set; }
        public IConnectable Second { get; private set; }

        public Connection(IConnectable first)
        {
            Assert.IsNotNull(first);

            first.SetConnection(this);

            First = first;
        }

        public Connection(IConnectable first, IConnectable second)
        {
            Assert.IsNotNull(first, second);

            first.SetConnection(this);
            second.SetConnection(this);

            First = first;
            Second = second;
        }

        public void CompleteConnection(IConnectable second)
        {
            Assert.IsNotNull(second);
            Assert.That(!ConnectionCompleted);

            second.SetConnection(this);
            Second = second;
        }

        public IConnectable GetOther(IConnectable original)
        {
            Assert.That(ConnectionCompleted);
            Assert.That(original == First || original == Second);

            return original == First ? Second : First;
        }

        public void RemoveConnection()
        {
            First?.ResetConnection();
            Second?.ResetConnection();
        }
    }
}
