using System;
using UnityEngine;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    [Serializable]
    public class ExternalServerSaveData : IEquatable<ExternalServerSaveData>
    {
        public const string SaveKey = "ExternalServer";

        [SerializeField] internal string Username;
        [SerializeField] internal string Token;

        public ExternalServerSaveData()
        {
            Username = null;
            Token = null;
        }

        public ExternalServerSaveData Clone()
            => (ExternalServerSaveData)MemberwiseClone();

        public bool Equals(ExternalServerSaveData other)
            =>
            Username == other.Username &&
            Token == other.Username;
    }
}
