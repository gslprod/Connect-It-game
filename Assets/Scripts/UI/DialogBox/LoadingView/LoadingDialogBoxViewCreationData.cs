using ConnectIt.Localization;
using ConnectIt.Utilities;
using UnityEngine.UIElements;

namespace ConnectIt.UI.DialogBox
{
    public class LoadingDialogBoxViewCreationData
    {
        public VisualElement Parent { get; }
        public TextKey TitleKey { get; }
        public TextKey MessageKey { get; }
        public DialogBoxButtonInfo BottomButton { get; }
        public bool ShowImmediately { get; }

        public LoadingDialogBoxViewCreationData(
            VisualElement parent,
            TextKey titleKey,
            TextKey messageKey,
            DialogBoxButtonInfo bottomButton = null,
            bool showImmediately = true)
        {
            Assert.ArgsIsNotNull(parent, titleKey, messageKey);

            Parent = parent;
            TitleKey = titleKey;
            MessageKey = messageKey;
            BottomButton = bottomButton;
            ShowImmediately = showImmediately;
        }
    }
}
