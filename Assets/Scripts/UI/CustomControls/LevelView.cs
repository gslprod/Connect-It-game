using UnityEngine.UIElements;

namespace ConnectIt.UI.CustomControls
{
    public class LevelView : Button
    {
        public new class UxmlFactory : UxmlFactory<LevelView, UxmlTraits> { }

        public new class UxmlTraits : Button.UxmlTraits
        {
            private readonly UxmlIntAttributeDescription m_Level = new()
            {
                name = "level",
                defaultValue = 0
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                LevelView container = ve as LevelView;
                container.Level = m_Level.GetValueFromBag(bag, cc);
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                if (_level == value)
                    return;

                _level = value;
            }
        }

        private int _level;

        public LevelView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent attachToPanelEvent)
        {
            if (parent is LevelViewContainer container)
                container.UpdateConnections();
        }
    }
}
