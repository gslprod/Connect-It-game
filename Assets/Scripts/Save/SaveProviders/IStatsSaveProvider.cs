using ConnectIt.Save.SaveProviders.SaveData;
using System;

namespace ConnectIt.Save.SaveProviders
{
    public interface IStatsSaveProvider
    {
        event Action StatsSaveDataChanged;

        void SaveStatsData(StatsSaveData saveData);
        StatsSaveData LoadStatsData();
    }
}
