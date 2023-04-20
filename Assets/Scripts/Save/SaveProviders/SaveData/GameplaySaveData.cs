using System;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    public class GameplaySaveData : IEquatable<GameplaySaveData>, ICloneable
    {
        public const string SaveKey = "Gameplay";

        public object Clone()
            => MemberwiseClone();

        public bool Equals(GameplaySaveData other)
            => false;
    }
}
