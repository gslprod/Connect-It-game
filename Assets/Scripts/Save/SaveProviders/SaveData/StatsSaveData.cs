using System;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    [Serializable]
    public class StatsSaveData : IEquatable<StatsSaveData>
    {
        public const string SaveKey = "Stats";

        public StatsSaveData Clone()
            => (StatsSaveData)MemberwiseClone();

        public bool Equals(StatsSaveData other)
            => false;
    }
}
