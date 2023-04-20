using System;

namespace ConnectIt.Save.Serializers
{
    public interface ISerializer
    {
        string Serialize(object obj);
        object Deserialize(string serializedObj, Type objType);
        T Deserialize<T>(string serializedObj);
    }
}
