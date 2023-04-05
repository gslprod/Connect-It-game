using ConnectIt.Localization;
using ConnectIt.Utilities;
using UnityEngine.UIElements;

namespace ConnectIt.UI.DialogBox
{
    public class DialogBoxCreationData
    {
        public VisualElement Parent { get; }
        public TextKey TitleKey { get; }
        public TextKey MessageKey { get; }
        public DialogBoxButtonInfo[] Buttons { get; }
        public bool ShowImmediately { get; }

        public DialogBoxCreationData(VisualElement parent,
            TextKey titleKey,
            TextKey messageKey,
            DialogBoxButtonInfo[] buttons = null,
            bool showImmediately = true)
        {
            Assert.ArgsIsNotNull(parent, titleKey, messageKey);

            Parent = parent;
            TitleKey = titleKey;
            MessageKey = messageKey;
            Buttons = buttons;
            ShowImmediately = showImmediately;
        }
    }
}
