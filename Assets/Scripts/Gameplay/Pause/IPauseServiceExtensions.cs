namespace ConnectIt.Gameplay.Pause
{
    public static class IPauseServiceExtensions
    {
        public static void SetPause(this IPauseService invokeSource, bool isPause, PauseEnablePriority priority, object source)
        {
            invokeSource.SetPause(isPause, (int)priority, source);
        }
    }
}
