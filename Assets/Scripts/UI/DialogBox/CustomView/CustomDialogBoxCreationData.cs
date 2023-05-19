using ConnectIt.Localization;
using ConnectIt.Utilities;
using UnityEngine.UIElements;

namespace ConnectIt.UI.DialogBox
{
    public class CustomDialogBoxCreationData
    {
        public VisualElement Parent { get; }
        public TextKey TitleKey { get; }
        public VisualElement Content { get; }
        public VisualTreeAsset ContentAsset { get; }
        public DialogBoxButtonInfo AdditionalBottomButton { get; }
        public bool ShowImmediately { get; }

        public string AdditionalDialogBoxRootClass { get; set; }
        public string AdditionalDialogBoxContainerClass { get; set; }
        public string AdditionalDialogBoxRootClosedClass { get; set; }
        public string AdditionalDialogBoxContainerClosedClass { get; set; }

        public CustomDialogBoxCreationData(
            VisualElement parent,
            TextKey titleKey,
            VisualElement content,
            DialogBoxButtonInfo additionalBottomButton = null,
            bool showImmediately = true)
        {
            Assert.ArgsIsNotNull(parent, titleKey);

            Parent = parent;
            TitleKey = titleKey;
            Content = content;
            AdditionalBottomButton = additionalBottomButton;
            ShowImmediately = showImmediately;
        }

        public CustomDialogBoxCreationData(VisualElement parent,
            TextKey titleKey,
            VisualTreeAsset contentAsset,
            DialogBoxButtonInfo additionalBottomButton = null,
            bool showImmediately = true)
        {
            Assert.ArgsIsNotNull(parent, titleKey);

            Parent = parent;
            TitleKey = titleKey;
            ContentAsset = contentAsset;
            AdditionalBottomButton = additionalBottomButton;
            ShowImmediately = showImmediately;
        }
    }
}
