using ConnectIt.Utilities;

namespace ConnectIt.Model
{
    public class Connection
    {
        public IConnectable First { get; }
        public IConnectable Second { get; }

        public Connection(IConnectable first, IConnectable second)
        {
            Assert.IsNotNull(first, second);

            first.SetConnection(this);
            second.SetConnection(this);

            First = first;
            Second = second;
        }

        public void RemoveConnection()
        {
            First.ResetConnection();
            Second.ResetConnection();
        }
    }
}
