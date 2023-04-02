using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConnectIt.UI.CustomControls
{
    public class ProgressBar : BindableElement, INotifyValueChanged<float>
    {
        public new class UxmlFactory : UxmlFactory<ProgressBar, UxmlTraits> { }

        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            private readonly UxmlFloatAttributeDescription m_LowValue = new()
            {
                name = "low-value",
                defaultValue = 0f
            };

            private readonly UxmlFloatAttributeDescription m_HighValue = new()
            {
                name = "high-value",
                defaultValue = 100f
            };

            private readonly UxmlFloatAttributeDescription m_Value = new()
            {
                name = "value",
                defaultValue = 0f
            };

            private readonly UxmlStringAttributeDescription m_Title = new()
            {
                name = "title",
                defaultValue = string.Empty
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ProgressBar progressBar = ve as ProgressBar;
                progressBar.LowValue = m_LowValue.GetValueFromBag(bag, cc); 
                progressBar.HighValue = m_HighValue.GetValueFromBag(bag, cc);
                progressBar.value = m_Value.GetValueFromBag(bag, cc);
                progressBar.Title = m_Title.GetValueFromBag(bag, cc);
            }
        }

        public static readonly string UssClassName = "progress-bar";
        public static readonly string ContainerUssClassName = UssClassName + "__container";
        public static readonly string TitleUssClassName = UssClassName + "__title";
        public static readonly string TitleContainerUssClassName = UssClassName + "__title-container";
        public static readonly string ProgressUssClassName = UssClassName + "__progress";
        public static readonly string BackgroundUssClassName = UssClassName + "__background";

        private readonly VisualElement _background;
        private readonly VisualElement _progress;
        private readonly Label _title;

        private float _lowValue;
        private float _highValue = 100f;
        private float _value;

        public string Title
        {
            get
            {
                return _title.text;
            }
            set
            {
                _title.text = value;
            }
        }

        public float LowValue
        {
            get
            {
                return _lowValue;
            }
            set
            {
                _lowValue = value;
                SetProgress(_value);
            }
        }

        public float HighValue
        {
            get
            {
                return _highValue;
            }
            set
            {
                _highValue = value;
                SetProgress(_value);
            }
        }

        public virtual float value
        {
            get
            {
                return _value;
            }
            set
            {
                if (EqualityComparer<float>.Default.Equals(_value, value))
                {
                    return;
                }

                if (panel != null)
                {
                    using ChangeEvent<float> changeEvent = ChangeEvent<float>.GetPooled(_value, value);
                    changeEvent.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(changeEvent);
                }
                else
                {
                    SetValueWithoutNotify(value);
                }
            }
        }

        public ProgressBar()
        {
            AddToClassList(UssClassName);
            VisualElement visualElement = new()
            {
                name = UssClassName
            };

            _background = new VisualElement();
            _background.AddToClassList(BackgroundUssClassName);
            visualElement.Add(_background);

            _progress = new VisualElement();
            _progress.AddToClassList(ProgressUssClassName);
            _background.Add(_progress);

            VisualElement visualElement2 = new();
            visualElement2.AddToClassList(TitleContainerUssClassName);
            _background.Add(visualElement2);

            _title = new Label();
            _title.AddToClassList(TitleUssClassName);
            visualElement2.Add(_title);

            visualElement.AddToClassList(ContainerUssClassName);

            hierarchy.Add(visualElement);

            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        public void SetValueWithoutNotify(float newValue)
        {
            _value = newValue;
            SetProgress(value);
        }

        private void OnGeometryChanged(GeometryChangedEvent e)
        {
            SetProgress(value);
        }

        private void SetProgress(float p)
        {
            float width = ((p < LowValue) ? LowValue : ((!(p > HighValue)) ? p : HighValue));
            width = CalculateProgressWidth(width);
            if (width >= 0f)
            {
                _progress.style.right = width;
            }
        }

        private float CalculateProgressWidth(float width)
        {
            if (_background == null || _progress == null)
            {
                return 0f;
            }

            if (float.IsNaN(_background.layout.width))
            {
                return 0f;
            }

            float num = _background.layout.width - 2f;
            return num - Mathf.Max(num * width / HighValue, 1f);
        }
    }
}
