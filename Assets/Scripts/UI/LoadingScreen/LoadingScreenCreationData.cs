using ConnectIt.Localization;
using UnityEngine.UIElements;

namespace ConnectIt.UI.LoadingScreen
{
    public class LoadingScreenCreationData
    {
        public VisualElement Parent { get; }
        public TextKey TitleKey { get; }
        public TextKey MessageKey { get; }
        public bool ShowImmediately { get; }

        public LoadingScreenCreationData(VisualElement parent,
            TextKey titleKey,
            TextKey messageKey,
            bool showImmediately = true)
        {
            Parent = parent;
            TitleKey = titleKey;
            MessageKey = messageKey;
            ShowImmediately = showImmediately;
        }
    }
}
