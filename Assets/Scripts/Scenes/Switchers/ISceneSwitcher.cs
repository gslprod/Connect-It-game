namespace ConnectIt.Scenes.Switchers
{
    public interface ISceneSwitcher
    {
        bool TryGoToScene(SceneType sceneType);
        bool TryReloadActiveScene();
    }
}