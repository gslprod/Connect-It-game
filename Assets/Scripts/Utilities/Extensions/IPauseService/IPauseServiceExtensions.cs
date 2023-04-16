namespace ConnectIt.Utilities.Extensions.IPauseService
{
    public static class IPauseServiceExtensions
    {
        public static void SetPause(this Gameplay.Pause.IPauseService invokeSource, bool isPause, PauseEnablePriority priority, object source)
        {
            invokeSource.SetPause(isPause, (int)priority, source);
        }
    }
}
