using System;

namespace ConnectIt.Security.Encryption
{
    public class SimpleKeyGenerator : IKeyGenerator
    {
        public string Generate(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new char[length];
            var rnd = new Random();

            for (int i = 0; i < length; i++)
                result[i] = chars[rnd.Next(chars.Length)];

            return new string(result);
        }
    }
}
