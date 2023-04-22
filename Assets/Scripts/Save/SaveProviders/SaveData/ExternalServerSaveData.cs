using System;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    [Serializable]
    public class ExternalServerSaveData : IEquatable<ExternalServerSaveData>
    {
        public const string SaveKey = "ExternalServer";

        public ExternalServerSaveData Clone()
            => (ExternalServerSaveData)MemberwiseClone();

        public bool Equals(ExternalServerSaveData other)
            => false;
    }
}
