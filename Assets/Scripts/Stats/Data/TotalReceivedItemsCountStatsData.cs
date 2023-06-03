using ConnectIt.Localization;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System;
using Zenject;

namespace ConnectIt.Stats.Data
{
    public class TotalReceivedItemsCountStatsData : StatsDataBase<long>, IInitializable
    {
        public override event Action<IStatsData> ValueChanged;
        public override event Action<StatsDataBase<long>> RawValueChanged;

        public override TextKey Name => _name;
        public override TextKey Description => _description;
        public override TextKey Value => _value;
        public override bool OftenUpdating => false;
        public override long RawValue
        {
            get
            {
                return _rawValue;
            }
            set
            {
                Assert.ThatArgIs(value >= 0);

                _rawValue = value;
                RawValueChanged?.Invoke(this);
                UpdateValue();
            }
        }

        private readonly TextKey.Factory _textKeyFactory;

        private TextKey _name;
        private TextKey _description;
        private TextKey _value;
        private long _rawValue;

        public TotalReceivedItemsCountStatsData(
            TextKey.Factory textKeyFactory)
        {
            _textKeyFactory = textKeyFactory;
        }

        public void Initialize()
        {
            _name = _textKeyFactory.Create(TextKeysConstants.StatsData.TotalReceivedItemsCount_Name);
            _description = _textKeyFactory.Create(TextKeysConstants.StatsData.TotalReceivedItemsCount_Description);
            _value = _textKeyFactory.Create(TextKeysConstants.StatsData.TotalReceivedItemsCount_Value);

            UpdateValue();
        }

        public void InscreaseRawValue(long coins)
        {
            Assert.ThatArgIs(coins >= 0);

            _rawValue += coins;
            RawValueChanged?.Invoke(this);
            UpdateValue();
        }

        private void UpdateValue()
        {
            _value.SetArgs(new object[]
            {
                _rawValue
            });

            ValueChanged?.Invoke(this);
        }

        public class Factory : PlaceholderFactory<TotalReceivedItemsCountStatsData> { }
    }
}
