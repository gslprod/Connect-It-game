using ConnectIt.Localization;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System;
using Zenject;

namespace ConnectIt.Stats.Data
{
    public class ApplicationRunningTimeStatsData : StatsDataBase<double>, IInitializable
    {
        public override event Action<IStatsData> ValueChanged;
        public override event Action<StatsDataBase<double>> RawValueChanged;

        public override TextKey Name => _name;
        public override TextKey Description => _description;
        public override TextKey Value => _value;
        public override bool OftenUpdating => true;
        public override double RawValue
        {
            get
            {
                return _rawValue.TotalSeconds;
            }
            set
            {
                Assert.ThatArgIs(value >= 0);

                _rawValue = CreateTimeSpanBySeconds(value);
                RawValueChanged?.Invoke(this);
                UpdateValue();
            }
        }

        private readonly TextKey.Factory _textKeyFactory;

        private TextKey _name;
        private TextKey _description;
        private TextKey _value;
        private TimeSpan _rawValue;

        public ApplicationRunningTimeStatsData(
            TextKey.Factory textKeyFactory)
        {
            _textKeyFactory = textKeyFactory;
        }

        public void Initialize()
        {
            _name = _textKeyFactory.Create(TextKeysConstants.Items.StatsData_ApplicationRunningTime_Name);
            _description = _textKeyFactory.Create(TextKeysConstants.Items.StatsData_ApplicationRunningTime_Description);
            _value = _textKeyFactory.Create(TextKeysConstants.Items.StatsData_ApplicationRunningTime_Value);
        }

        public void InscreaseRawValue(double sec)
        {
            Assert.ThatArgIs(sec >= 0);

            _rawValue = _rawValue.Add(CreateTimeSpanBySeconds(sec));
            RawValueChanged?.Invoke(this);
            UpdateValue();
        }

        private static TimeSpan CreateTimeSpanBySeconds(double sec)
        {
            return new TimeSpan(Convert.ToInt64(TimeSpan.TicksPerSecond * sec));
        }

        private void UpdateValue()
        {
            _value.SetArgs(new object[]
            {
                string.Format("{0:0}", _rawValue.TotalHours),
                _rawValue.Minutes
            });

            ValueChanged?.Invoke(this);
        }

        public class Factory : PlaceholderFactory<ApplicationRunningTimeStatsData> { }
    }
}
