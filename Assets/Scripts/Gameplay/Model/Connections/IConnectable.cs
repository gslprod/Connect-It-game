namespace ConnectIt.Model
{
    public interface IConnectable
    {
        Connection Connection { get; }
        bool Connected { get; }
        int CompatibilityIndex { get; }

        void SetConnection(Connection connection);
        void ResetConnection();
    }
}