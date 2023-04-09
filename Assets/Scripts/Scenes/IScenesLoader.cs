using System;

namespace ConnectIt.Scenes
{
    public interface IScenesLoader
    {
        SceneType ActiveScene { get; }
        bool SceneLoading { get; }

        event Action<SceneType> OnNewSceneAsyncLoadFinished;
        event Action<SceneType, float> OnNewSceneAsyncLoading;
        event Action<SceneType> OnNewSceneAsyncLoadStarted;

        bool TryGoToSceneAsync(SceneType sceneType);
    }
}