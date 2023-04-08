namespace ConnectIt.Input
{
    public static class GameplayInputRouterExtensions
    {
        public static void SetEnable(this GameplayInputRouter invokeSource, bool enable, GameplayInputRouterEnablePriority priority, object source)
        {
            invokeSource.SetEnable(enable, (int)priority, source);
        }
    }
}
