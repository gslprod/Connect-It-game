using System;

namespace ConnectIt.Gameplay.Model
{
    public interface IConnectable : IDisposable
    {
        Connection Connection { get; }
        bool HasConnection { get; }
        int CompatibilityIndex { get; }
        bool AllowIncompatibleConnections { get; set; }

        void SetConnection(Connection connection);
        void ResetConnection();
        bool CanBeConnectedWith(IConnectable other);
        bool CompatibleWith(IConnectable other);
    }
}