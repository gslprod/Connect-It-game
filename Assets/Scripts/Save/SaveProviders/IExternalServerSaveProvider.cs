using ConnectIt.Save.SaveProviders.SaveData;
using System;

namespace ConnectIt.Save.SaveProviders
{
    public interface IExternalServerSaveProvider
    {
        event Action ExternalServerSaveDataChanged;

        void SaveExternalServerData(ExternalServerSaveData saveData);
        ExternalServerSaveData LoadExternalServerData();
    }
}
