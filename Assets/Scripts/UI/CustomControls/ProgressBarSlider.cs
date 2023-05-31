using UnityEngine.UIElements;

namespace ConnectIt.UI.CustomControls
{
    public class ProgressBarSlider : Slider
    {
        public new class UxmlFactory : UxmlFactory<ProgressBarSlider, UxmlTraits> { }

        public new class UxmlTraits : Slider.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                ProgressBarSlider progressBarSlider = (ProgressBarSlider)ve;
                base.Init(ve, bag, cc);
                progressBarSlider.UpdateProgressBar();
            }
        }

        public const string ProgressBarSliderUssClassName = "progress-bar-slider";

        public new float lowValue
        {
            get => base.lowValue;
            set
            {
                if (base.lowValue != value)
                    _progressBar.LowValue = value;

                base.lowValue = value;
            }
        }

        public new float highValue
        {
            get => base.highValue;
            set
            {
                if (base.highValue != value)
                    _progressBar.HighValue = value;

                base.highValue = value;
            }
        }

        private ProgressBar _progressBar;

        public ProgressBarSlider()
            : this(null) { }

        public ProgressBarSlider(string label, float start = 0f, float end = 10f, SliderDirection direction = SliderDirection.Horizontal, float pageSize = 0f)
            : base(label, start, end, direction, pageSize)
        {
            AddToClassList(ProgressBarSliderUssClassName);

            _progressBar = new ProgressBar();
            Add(_progressBar);
            _progressBar.SendToBack();

            this.RegisterValueChangedCallback(OnValueChanged);
        }

        public override void SetValueWithoutNotify(float newValue)
        {
            base.SetValueWithoutNotify(newValue);
            UpdateProgressBarValue();
        }

        private void UpdateProgressBarValue()
        {
            _progressBar.value = value;
        }

        private void OnValueChanged(ChangeEvent<float> changeEvent)
        {
            UpdateProgressBarValue();
        }

        private void UpdateProgressBar()
        {
            _progressBar.LowValue = lowValue;
            _progressBar.HighValue = highValue;
            _progressBar.value = value;
        }
    }
}
