namespace ConnectIt.Security.Encryption
{
    public interface IKeyGenerator
    {
        string Generate(int length);
    }
}