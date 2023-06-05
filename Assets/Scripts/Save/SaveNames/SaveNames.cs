using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Shop.Goods;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Stats.Data;
using ConnectIt.Utilities;
using System;
using System.Linq;

namespace ConnectIt.Save.Names
{
    public static class SaveNames
    {
        public const string Separator = ".";

        private static readonly ItemName[] _names = new ItemName[]
        {
            #region Products
            new ItemName(typeof(IProduct), "Product"),
            new ItemName(typeof(Boost), "Boost", typeof(IProduct)),
            new ItemName(typeof(SkipLevelBoost), "SkipLevel", typeof(Boost)),
            new ItemName(typeof(SimplifyLevelBoost), "SimplifyLevel", typeof(Boost)),
            new ItemName(typeof(AllowIncompatibleConnectionsBoost), "AllowIncompatibleConnections", typeof(Boost)),
            #endregion

            #region StatsData
            new ItemName(typeof(IStatsData), "StatsData"),
            new ItemName(typeof(ApplicationRunningTimeStatsData), "ApplicationRunningTime", typeof(IStatsData)),
            new ItemName(typeof(MovesCountStatsData), "MovesCount", typeof(IStatsData)),
            new ItemName(typeof(TotalEarnedCoinsStatsData), "TotalEarnedCoins", typeof(IStatsData)),
            new ItemName(typeof(TotalReceivedItemsCountStatsData), "TotalReceivedItemsCount", typeof(IStatsData)),
            new ItemName(typeof(FirstLaunchedVersionStatsData), "FirstLaunchedVersion", typeof(IStatsData)),
            new ItemName(typeof(BoostsUsageCountStatsData), "BoostsUsageCount", typeof(IStatsData)),
            #endregion

            #region SaveData
            new ItemName(typeof(GameplaySaveData), "GameplaySaveData"),
            new ItemName(typeof(ShopSaveData), "ShopSaveData"),
            new ItemName(typeof(StatsSaveData), "StatsSaveData"),
            new ItemName(typeof(ExternalServerSaveData), "ExternalServerSaveData"),
            new ItemName(typeof(SettingsSaveData), "SettingsSaveData")
            #endregion
        };

        public static Type GetTypeBySaveName(string saveName)
        {
            Assert.ThatArgIs(!string.IsNullOrEmpty(saveName));

            string[] splitted = saveName.Split(Separator);

            ItemName item = GetItemByNameAndParentType(splitted[0], null);
            for (int i = 1; i < splitted.Length; i++)
                item = GetItemByNameAndParentType(splitted[i], item.Type);

            return item.Type;
        }

        public static string GetSaveName(Type type)
        {
            Assert.ArgIsNotNull(type);

            ItemName item = GetItemByType(type);
            string name = item.Name;

            while (item.ParentType != null)
            {
                item = GetItemByType(item.ParentType);
                name = item.Name + Separator + name;
            }

            return name;
        }

        public static string GetSaveName<T>()
            => GetSaveName(typeof(T));

        private static ItemName GetItemByType(Type type)
            => _names.First(item => item.Type == type);

        private static ItemName GetItemByNameAndParentType(string name, Type parentType)
            => _names.First(item => item.Name == name && item.ParentType == parentType);
    }
}
