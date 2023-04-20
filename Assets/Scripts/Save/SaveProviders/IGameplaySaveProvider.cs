using ConnectIt.Save.SaveProviders.SaveData;
using System;

namespace ConnectIt.Save.SaveProviders
{
    public interface IGameplaySaveProvider
    {
        event Action GameplaySaveDataChanged;

        void SaveGameplayData(GameplaySaveData saveData);
        GameplaySaveData LoadGameplayData();
    }
}
