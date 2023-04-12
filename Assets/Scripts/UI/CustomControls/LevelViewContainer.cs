using UnityEngine.UIElements;

namespace ConnectIt.UI.CustomControls
{
    public class LevelViewContainer : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<LevelViewContainer, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlBoolAttributeDescription m_AutoConnectViews = new()
            {
                name = "auto-connect-views",
                defaultValue = false
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                LevelViewContainer container = ve as LevelViewContainer;
                container.AutoConnectViews = m_AutoConnectViews.GetValueFromBag(bag, cc);
            }
        }

        public bool AutoConnectViews
        {
            get => _autoConnectViews;
            set
            {
                if (_autoConnectViews == value)
                    return;

                _autoConnectViews = value;

                if (_autoConnectViews)
                    CreateConnections();
                else
                    DestroyConnections();
            }
        }

        private bool _autoConnectViews = false;

        public LevelViewContainer() { }

        public void UpdateConnections()
        {
            if (!AutoConnectViews)
                return;

            DestroyConnections();
            CreateConnections();
        }

        private void CreateConnections()
        {
            LevelView last = null;
            int count = childCount;
            for (int i = 0; i < count; i++)
            {
                if (this[i] is not LevelView current)
                    continue;

                if (last != null)
                {
                    LevelViewConnection connection = new(last, current);
                    Add(connection);
                }

                last = current;
            }
        }

        private void DestroyConnections()
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                if (this[i] is not LevelViewConnection connection)
                    continue;

                connection.Disconnect();
                connection.RemoveFromHierarchy();
            }
        }
    }
}
