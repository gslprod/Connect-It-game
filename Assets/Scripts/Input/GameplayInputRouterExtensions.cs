namespace ConnectIt.Input
{
    public static class GameplayInputRouterExtensions
    {
        public static void SetEnable(this GameplayInputRouter source, bool enable, GameplayInputRouterEnablePriority priority)
        {
            source.SetEnable(enable, (int)priority);
        }

        public static void ResetEnableWithPriority(this GameplayInputRouter source, GameplayInputRouterEnablePriority priority)
        {
            source.ResetEnableWithPriority((int)priority);
        }
    }
}
