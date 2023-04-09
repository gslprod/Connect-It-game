using System;

namespace ConnectIt.Utilities.Formatters
{
    public interface IFormatter
    {
        string FormatGameplayElapsedTime(TimeSpan time);
        string FormatGameplayLevelProgress(float progress);
        string FormatSceneLoadingProgress(float progress);
    }
}