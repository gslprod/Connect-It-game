using ConnectIt.Gameplay.Model;
using ConnectIt.Localization;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
using ConnectIt.Utilities.Extensions;
using Zenject;

namespace ConnectIt.Shop.Goods.Boosts
{
    public class AllowIncompatibleConnectionsBoost : Boost
    {
        public override TextKey Name => _nameTextKey;
        public override TextKey Description => _descriptionTextKey;

        private readonly TextKey.Factory _textKeyFactory;

        private TextKey _nameTextKey;
        private TextKey _descriptionTextKey;

        public AllowIncompatibleConnectionsBoost(
            TextKey.Factory textKeyFactory)
        {
            _textKeyFactory = textKeyFactory;
        }

        public override void Initialize()
        {
            base.Initialize();

            _nameTextKey = _textKeyFactory.Create(TextKeysConstants.Items.Product_Boost_AllowIncompatibleConnections_Name);
            _descriptionTextKey = _textKeyFactory.Create(TextKeysConstants.Items.Product_Boost_AllowIncompatibleConnections_Description);
        }

        public override void Use(BoostUsageContext usageContext)
        {
            base.Use(usageContext);

            AllowIncompatibleConnectionsBoostUsageContext context = (AllowIncompatibleConnectionsBoostUsageContext)usageContext;

            foreach (Port port in context.PortsRegistrator.Registrations)
                port.Connectable.AllowIncompatibleConnections = true;
        }

        public class Factory : PlaceholderFactory<AllowIncompatibleConnectionsBoost> { }
    }
}
