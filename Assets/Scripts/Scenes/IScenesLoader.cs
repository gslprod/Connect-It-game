using System;

namespace ConnectIt.Scenes
{
    public interface IScenesLoader
    {
        event Action<SceneType> OnNewSceneAsyncLoadFinished;
        event Action<SceneType, float> OnNewSceneAsyncLoading;
        event Action<SceneType> OnNewSceneAsyncLoadStarted;

        bool TryGoToSceneAsync(SceneType sceneType);
    }
}