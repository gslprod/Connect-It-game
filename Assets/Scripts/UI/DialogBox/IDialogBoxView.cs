using System;

namespace ConnectIt.UI.DialogBox
{
    public interface IDialogBoxView
    {
        event Action<IDialogBoxView> Closing;
        event Action<IDialogBoxView> Showing;
        event Action<IDialogBoxView> Disposing;

        void Close();
        void Show();
    }
}