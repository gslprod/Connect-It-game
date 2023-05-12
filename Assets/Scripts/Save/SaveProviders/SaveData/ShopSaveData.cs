using System;
using UnityEngine;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    [Serializable]
    public class ShopSaveData : IEquatable<ShopSaveData>
    {
        public const string SaveKey = "Shop";

        [SerializeField] internal long Coins;
        [SerializeField] internal StorageItemSaveData[] StorageItems;

        public ShopSaveData()
        {
            Coins = 0;
            StorageItems = null;
        }

        public ShopSaveData Clone()
        {
            ShopSaveData clonedObject = (ShopSaveData)MemberwiseClone();

            if (StorageItems != null)
            {
                clonedObject.StorageItems = new StorageItemSaveData[StorageItems.Length];
                for (int i = 0; i < StorageItems.Length; i++)
                    clonedObject.StorageItems[i] = StorageItems[i].Clone();
            }

            return clonedObject;
        }

        public bool Equals(ShopSaveData other)
        {
            if (StorageItems == null != (other.StorageItems == null))
                return false;

            if (StorageItems != null)
            {
                if (StorageItems.Length != other.StorageItems.Length)
                    return false;

                for (int i = 0; i < StorageItems.Length; i++)
                {
                    if (!StorageItems[i].Equals(other.StorageItems[i]))
                        return false;
                }
            }

            return Coins == other.Coins;
        }

        [Serializable]
        public class StorageItemSaveData : IEquatable<StorageItemSaveData>
        {
            [SerializeField] internal string ItemSaveName;
            [SerializeField] internal long Amount;

            public bool Equals(StorageItemSaveData other)
                =>
                ItemSaveName == other.ItemSaveName &&
                Amount == other.Amount;

            public StorageItemSaveData Clone()
                => (StorageItemSaveData)MemberwiseClone();
        }
    }
}
