using ConnectIt.Save.SaveProviders.SaveData;
using System;

namespace ConnectIt.Save.SaveProviders
{
    public interface ISettingsSaveProvider
    {
        event Action SettingsSaveDataChanged;

        void SaveSettingsData(SettingsSaveData saveData);
        SettingsSaveData LoadSettingsData();
    }
}
