using UnityEngine.UIElements;

namespace ConnectIt.UI.CustomControls
{
    public class ButtonToggle : Button, INotifyValueChanged<bool>
    {
        public new class UxmlFactory : UxmlFactory<ButtonToggle, UxmlTraits> { }

        public new class UxmlTraits : Button.UxmlTraits
        {
            private readonly UxmlBoolAttributeDescription m_Level = new()
            {
                name = "value",
                defaultValue = false
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ButtonToggle container = ve as ButtonToggle;
                container.value = m_Level.GetValueFromBag(bag, cc);
            }
        }

        public const string ButtonToggleClassName = "button-toggle";
        public const string ButtonToggleEnabledClassName = ButtonToggleClassName + "--enabled";
        public const string ButtonToggleDisabledClassName = ButtonToggleClassName + "--disabled";

        public bool value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                if (panel != null)
                {
                    using ChangeEvent<bool> changeEvent = ChangeEvent<bool>.GetPooled(_value, value);
                    changeEvent.target = this;
                    SetValueInternal(value);
                    SendEvent(changeEvent);
                }
                else
                {
                    SetValueInternal(value);
                }
            }
        }

        private bool _value;

        public ButtonToggle()
        {
            AddToClassList(ButtonToggleClassName);
            AddToClassList(ButtonToggleDisabledClassName);

            clicked += OnButtonClicked;
        }

        public void SetValueWithoutNotify(bool newValue)
        {
            if (_value == newValue)
                return;

            SetValueInternal(newValue);
        }

        private void SetValueInternal(bool newValue)
        {
            _value = newValue;
            UpdateView();
        }

        private void UpdateView()
        {
            ToggleInClassList(ButtonToggleEnabledClassName);
        }

        private void OnButtonClicked()
        {
            value = !value;
        }
    }
}
