namespace ConnectIt.Model
{
    public interface IConnectable
    {
        Connection Connection { get; }
        bool HasConnection { get; }
        int CompatibilityIndex { get; }

        void SetConnection(Connection connection);
        void ResetConnection();
    }
}