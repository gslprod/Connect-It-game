using ConnectIt.Gameplay.GameStateHandlers.GameEnd;
using Zenject;

namespace ConnectIt.Shop.Goods.Boosts.UsageContext
{
    public class SkipLevelBoostUsageContext : BoostUsageContext
    {
        public ILevelEndHandler LevelEndHandler { get; }

        public SkipLevelBoostUsageContext(
            CommonUsageData commonData,
            ILevelEndHandler levelEndHandler) : base(commonData)
        {
            LevelEndHandler = levelEndHandler;
        }

        public new class Factory : PlaceholderFactory<CommonUsageData, SkipLevelBoostUsageContext> { }
    }
}
