using ConnectIt.Gameplay.GameStateHandlers.GameEnd;
using Zenject;

namespace ConnectIt.Shop.Goods.Boosts.UsageContext
{
    public class SimplifyLevelBoostUsageContext : BoostUsageContext
    {
        public ILevelEndHandler LevelEndHandler { get; }

        public SimplifyLevelBoostUsageContext(
            CommonUsageData commonData,
            ILevelEndHandler levelEndHandler) : base(commonData)
        {
            LevelEndHandler = levelEndHandler;
        }

        public new class Factory : PlaceholderFactory<CommonUsageData, SimplifyLevelBoostUsageContext> { }
    }
}