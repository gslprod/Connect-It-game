namespace ConnectIt.Security.Encryption
{
    public interface ISymmetricEncryptor
    {
        string Decrypt(string cipherText);
        string Decrypt(string key, string cipherText);
        string Encrypt(string plainText);
        string Encrypt(string key, string plainText);
    }
}