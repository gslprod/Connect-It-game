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

            First = first;
            first.SetConnection(this);
        }

        public Connection(IConnectable first, IConnectable second)
        {
            Assert.IsNotNull(first, second);

            First = first;
            first.SetConnection(this);

            Second = second;
            second.SetConnection(this);
        }

        public void CompleteConnection(IConnectable second)
        {
            Assert.IsNotNull(second);
            Assert.That(!ConnectionCompleted);

            Second = second;
            second.SetConnection(this);
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
