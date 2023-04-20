using ConnectIt.Save.SaveProviders.SaveData;
using System;

namespace ConnectIt.Save.SaveProviders
{
    public interface IShopSaveProvider
    {
        event Action ShopSaveDataChanged;

        void SaveShopData(ShopSaveData saveData);
        ShopSaveData LoadShopData();
    }
}
