using System;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    [Serializable]
    public class ShopSaveData : IEquatable<ShopSaveData>
    {
        public const string SaveKey = "Shop";

        public ShopSaveData Clone()
            => (ShopSaveData)MemberwiseClone();

        public bool Equals(ShopSaveData other)
            => false;
    }
}
