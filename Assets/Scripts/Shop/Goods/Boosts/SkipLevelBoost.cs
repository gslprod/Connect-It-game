using ConnectIt.Localization;
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

        public SkipLevelBoost(TextKey.Factory textKeyFactory)
        {
            _textKeyFactory = textKeyFactory;
        }

        public override void Initialize()
        {
            base.Initialize();

            _nameTextKey = _textKeyFactory.Create(TextKeysConstants.Items.Boosts_SkipLevel_Name, null);
            _descriptionTextKey = _textKeyFactory.Create(TextKeysConstants.Items.Boosts_SkipLevel_Description, null);
        }

        public override void Use()
        {
            base.Use();


        }

        public class Factory : PlaceholderFactory<SkipLevelBoost> { }
    }
}
