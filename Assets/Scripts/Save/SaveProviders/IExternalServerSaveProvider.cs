using ConnectIt.Save.SaveProviders.SaveData;
using System;

namespace ConnectIt.Save.SaveProviders
{
    public interface IExternalServerSaveProvider
    {
        event Action ExternalServerSaveDataChanged;

        void SaveExtrenalServerData(ExternalServerSaveData saveData);
        ExternalServerSaveData LoadExternalServerData();
    }
}
