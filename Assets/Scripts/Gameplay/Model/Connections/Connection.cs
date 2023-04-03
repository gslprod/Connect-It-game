using ConnectIt.Utilities;
using System;

namespace ConnectIt.Gameplay.Model
{
    public class Connection : IDisposable
    {
        public bool ConnectionCompleted => First != null && Second != null;

        public IConnectable First { get; private set; }
        public IConnectable Second { get; private set; }

        public Connection(IConnectable first)
        {
            Assert.ArgIsNotNull(first);

            First = first;
            first.SetConnection(this);
        }

        public Connection(IConnectable first, IConnectable second)
        {
            Assert.ArgsIsNotNull(first, second);

            First = first;
            first.SetConnection(this);

            Second = second;
            second.SetConnection(this);
        }

        public void CompleteConnection(IConnectable second)
        {
            Assert.ArgIsNotNull(second);
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

        public void Dispose()
        {
            First?.ResetConnection();
            Second?.ResetConnection();
        }
    }
}
