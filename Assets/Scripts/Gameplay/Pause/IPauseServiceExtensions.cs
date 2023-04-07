namespace ConnectIt.Gameplay.Pause
{
    public static class IPauseServiceExtensions
    {
        public static void SetPause(this IPauseService source, bool isPause, PauseEnablePriority priority)
        {
            source.SetPause(isPause, (int)priority);
        }

        public static void ResetPauseWithPriority(this IPauseService source, PauseEnablePriority priority)
        {
            source.ResetPauseWithPriority((int)priority);
        }
    }
}
