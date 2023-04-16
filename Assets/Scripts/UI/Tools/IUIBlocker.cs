namespace ConnectIt.UI.Tools
{
    public interface IUIBlocker
    {
        public bool Blocked { get; }

        public void SetBlock(bool isBlock, int priority, object source);
        public void ResetBlock(object source);
    }
}
