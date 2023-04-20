using System;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    public class ExternalServerSaveData : IEquatable<ExternalServerSaveData>, ICloneable
    {
        public const string SaveKey = "ExternalServer";

        public object Clone()
            => MemberwiseClone();

        public bool Equals(ExternalServerSaveData other)
            => false;
    }
}
