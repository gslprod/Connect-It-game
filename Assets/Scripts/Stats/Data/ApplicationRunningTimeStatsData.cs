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
        private TimeSpan _rawValue = TimeSpan.Zero;

        private int _lastTotalHours = -1;
        private int _lastMinutes = -1;

        public ApplicationRunningTimeStatsData(
            TextKey.Factory textKeyFactory)
        {
            _textKeyFactory = textKeyFactory;
        }

        public void Initialize()
        {
            _name = _textKeyFactory.Create(TextKeysConstants.StatsData.ApplicationRunningTime_Name);
            _description = _textKeyFactory.Create(TextKeysConstants.StatsData.ApplicationRunningTime_Description);
            _value = _textKeyFactory.Create(TextKeysConstants.StatsData.ApplicationRunningTime_Value);

            UpdateValue();
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
            return new TimeSpan((long)Math.Round(TimeSpan.TicksPerSecond * sec));
        }

        private void UpdateValue()
        {
            int currentTotalHours = (int)_rawValue.TotalHours;

            if (_lastMinutes == _rawValue.Minutes && _lastTotalHours == currentTotalHours)
                return;

            _value.SetArgs(new object[]
            {
                currentTotalHours,
                _rawValue.Minutes
            });

            _lastMinutes = _rawValue.Minutes;
            _lastTotalHours = currentTotalHours;

            ValueChanged?.Invoke(this);
        }

        public class Factory : PlaceholderFactory<ApplicationRunningTimeStatsData> { }
    }
}
