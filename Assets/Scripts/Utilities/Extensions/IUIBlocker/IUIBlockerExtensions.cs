namespace ConnectIt.Utilities.Extensions.IUIBlocker
{
    public static class IUIBlockerExtensions
    {
        public static void SetBlock(this UI.Tools.IUIBlocker invokeSource, bool isBlock, BlockPriority priority, object source)
        {
            invokeSource.SetBlock(isBlock, (int)priority, source);
        }
    }
}
