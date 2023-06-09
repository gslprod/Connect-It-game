﻿using ConnectIt.Localization;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
using ConnectIt.Utilities.Extensions;
using Zenject;

namespace ConnectIt.Shop.Goods.Boosts
{
    public class SkipLevelBoost : Boost
    {
        public override TextKey Name => _nameTextKey;
        public override TextKey Description => _descriptionTextKey;

        private readonly TextKey.Factory _textKeyFactory;

        private TextKey _nameTextKey;
        private TextKey _descriptionTextKey;

        public SkipLevelBoost(
            TextKey.Factory textKeyFactory)
        {
            _textKeyFactory = textKeyFactory;
        }

        public override void Initialize()
        {
            base.Initialize();

            _nameTextKey = _textKeyFactory.Create(TextKeysConstants.Items.Product_Boost_SkipLevel_Name);
            _descriptionTextKey = _textKeyFactory.Create(TextKeysConstants.Items.Product_Boost_SkipLevel_Description);
        }

        public override void Use(BoostUsageContext usageContext)
        {
            base.Use(usageContext);

            SkipLevelBoostUsageContext context = (SkipLevelBoostUsageContext)usageContext;

            context.LevelEndHandler.SkipLevel();

            Dispose();
        }

        public class Factory : PlaceholderFactory<SkipLevelBoost> { }
    }
}
