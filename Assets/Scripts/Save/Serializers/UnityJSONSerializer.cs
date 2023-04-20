using System;
using UnityEngine;

namespace ConnectIt.Save.Serializers
{
    public class UnityJSONSerializer : ISerializer
    {
        private const bool _prettyPrint = true;

        public object Deserialize(string serializedObj, Type objType)
        {
            return JsonUtility.FromJson(serializedObj, objType);
        }

        public T Deserialize<T>(string serializedObj)
        {
            return JsonUtility.FromJson<T>(serializedObj);
        }

        public string Serialize(object obj)
        {
            return JsonUtility.ToJson(obj, _prettyPrint);
        }
    }
}
