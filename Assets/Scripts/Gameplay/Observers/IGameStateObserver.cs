using ConnectIt.Shop.Goods.Boosts.UsageContext;
using System;

namespace ConnectIt.Gameplay.Observers
{
    public interface IGameStateObserver
    {
        event Action GameCompleteProgressPercentsChanged;
        event Action MovesCountChanged;
        event Action<BoostUsageContext> BoostUsed;

        float GameCompleteProgressPercents { get; }
        int MovesCount { get; }
        int BoostsUsageCount { get; }
        bool AnyBoostWasUsed { get; }
    }
}