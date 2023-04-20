using System;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    public class StatsSaveData : IEquatable<StatsSaveData>, ICloneable
    {
        public const string SaveKey = "Stats";

        public object Clone()
            => MemberwiseClone();

        public bool Equals(StatsSaveData other)
            => false;
    }
}
