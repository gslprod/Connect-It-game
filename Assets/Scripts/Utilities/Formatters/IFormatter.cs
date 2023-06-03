using System;

namespace ConnectIt.Utilities.Formatters
{
    public interface IFormatter
    {
        string FormatDetailedGameplayElapsedTime(TimeSpan time);
        string FormatGameplayElapsedTime(TimeSpan time);
        string FormatGameplayLevelProgress(float progress);
        string FormatSceneLoadingProgress(float progress);
        string FormatVersion(string applicationVersion, string additionalVersionInfo);
    }
}