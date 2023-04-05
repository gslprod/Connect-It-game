using ConnectIt.Localization;
using ConnectIt.Utilities;
using System;

namespace ConnectIt.UI.DialogBox
{
    public enum DialogBoxButtonType
    {
        Default = 0,
        Accept,
        Dismiss
    }

    public class DialogBoxButtonInfo
    {
        public TextKey TitleKey { get; private set; }
        public DialogBoxButtonType Type { get; private set; }
        public Action OnClick { get; private set; }
        public bool ClosesDialogBox { get; private set; }

        public DialogBoxButtonInfo(TextKey titleKey,
            Action onClick,
            DialogBoxButtonType type = DialogBoxButtonType.Default,
            bool closesDialogBox = false)
        {
            Assert.ArgIsNotNull(titleKey);

            TitleKey = titleKey;
            OnClick = onClick;
            Type = type;
            ClosesDialogBox = closesDialogBox;
        }
    }
}
