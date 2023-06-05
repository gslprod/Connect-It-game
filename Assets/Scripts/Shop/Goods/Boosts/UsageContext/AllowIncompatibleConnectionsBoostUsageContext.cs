using ConnectIt.Gameplay.Model;
using ConnectIt.Infrastructure.Registrators;
using Zenject;

namespace ConnectIt.Shop.Goods.Boosts.UsageContext
{
    public class AllowIncompatibleConnectionsBoostUsageContext : BoostUsageContext
    {
        public IRegistrator<Port> PortsRegistrator { get; }

        public AllowIncompatibleConnectionsBoostUsageContext(
            CommonUsageData commonData,
            IRegistrator<Port> portsRegistrator) : base(commonData)
        {
            PortsRegistrator = portsRegistrator;
        }

        public new class Factory : PlaceholderFactory<CommonUsageData, AllowIncompatibleConnectionsBoostUsageContext> { }
    }
}
