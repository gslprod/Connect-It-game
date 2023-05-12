using ConnectIt.Shop.Goods;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Utilities;
using System;

namespace ConnectIt.Save.SaveNames
{
    public static class SaveNames
    {
        public const string Separator = ".";

        #region Boosts

        public const string BoostBaseName = "Boost";

        public const string SkipLevelBoostName = "SkipLevel";

        #endregion

        public static Type GetTypeBySaveName(string saveName)
        {
            Assert.ThatArgIs(!string.IsNullOrEmpty(saveName));

            string[] splitted = saveName.Split(Separator);
            if (splitted[0] == BoostBaseName)
                return GetBoostTypeBySaveName(splitted[1]);

            throw Assert.GetFailException();
        }

        public static string GetSaveName(Type type)
        {
            Assert.ArgIsNotNull(type);

            if (typeof(Boost).IsAssignableFrom(type))
                return BoostBaseName + Separator + GetBoostName(type);

            throw Assert.GetFailException();
        }

        public static string GetSaveName<T>() where T : IProduct
            => GetSaveName(typeof(T));

        private static string GetBoostName(Type type)
        {
            if (type.IsEquivalentTo(typeof(SkipLevelBoost)))
                return SkipLevelBoostName;

            throw Assert.GetFailException();
        }

        private static Type GetBoostTypeBySaveName(string saveName)
        {
            if (saveName == SkipLevelBoostName)
                return typeof(SkipLevelBoost);

            throw Assert.GetFailException();
        }
    }
}
