using System;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    public class ShopSaveData : IEquatable<ShopSaveData>, ICloneable
    {
        public const string SaveKey = "Shop";

        public object Clone()
            => MemberwiseClone();

        public bool Equals(ShopSaveData other)
            => false;
    }
}
