using ConnectIt.Localization;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
using ConnectIt.Utilities.Extensions;
using Zenject;

namespace ConnectIt.Shop.Goods.Boosts
{
    public class SimplifyLevelBoost : Boost
    {
        public override TextKey Name => _nameTextKey;
        public override TextKey Description => _descriptionTextKey;

        private readonly TextKey.Factory _textKeyFactory;

        private TextKey _nameTextKey;
        private TextKey _descriptionTextKey;

        public SimplifyLevelBoost(
            TextKey.Factory textKeyFactory)
        {
            _textKeyFactory = textKeyFactory;
        }

        public override void Initialize()
        {
            base.Initialize();

            _nameTextKey = _textKeyFactory.Create(TextKeysConstants.Items.Product_Boost_SimplifyLevel_Name);
            _descriptionTextKey = _textKeyFactory.Create(TextKeysConstants.Items.Product_Boost_SimplifyLevel_Description);
        }

        public override void Use(BoostUsageContext usageContext)
        {
            base.Use(usageContext);

            SimplifyLevelBoostUsageContext skipLevelUsageContext = (SimplifyLevelBoostUsageContext)usageContext;

            skipLevelUsageContext.LevelEndHandler.ProgressPercentsToWin = 80f;
        }

        public class Factory : PlaceholderFactory<SimplifyLevelBoost> { }
    }
}
