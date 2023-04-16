namespace ConnectIt.Utilities.Extensions.GameplayInputRouter
{
    public static class GameplayInputRouterExtensions
    {
        public static void SetEnable(this Input.GameplayInputRouter invokeSource, bool enable, GameplayInputRouterEnablePriority priority, object source)
        {
            invokeSource.SetEnable(enable, (int)priority, source);
        }
    }
}
