using ConnectIt.Save.SaveProviders.SaveData;
using System;

namespace ConnectIt.Save.SaveProviders
{
    public interface ISecuritySaveProvider
    {
        event Action SecuritySaveDataChanged;

        void SaveSecurityData(SecuritySaveData saveData);
        SecuritySaveData LoadSecurityData();
    }
}
