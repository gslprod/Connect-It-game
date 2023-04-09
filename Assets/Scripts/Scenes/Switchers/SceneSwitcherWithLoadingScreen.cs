using ConnectIt.Localization;
using ConnectIt.UI.Global.MonoWrappers;
using ConnectIt.UI.LoadingScreen;
using ConnectIt.Utilities;
using UnityEngine;

namespace ConnectIt.Scenes.Switchers
{
    public class SceneSwitcherWithLoadingScreen : ISceneSwitcher
    {
        private readonly IScenesLoader _scenesLoader;
        private readonly LoadingScreenView.Factory _loadingScreenViewFactory;
        private readonly GlobalUIDocumentMonoWrapper _globalUIDocument;
        private readonly TextKey.Factory _textKeyFactory;

        private SceneType? _sceneToLoad;
        private LoadingScreenView _activeLoadingScreen;

        public SceneSwitcherWithLoadingScreen(
            IScenesLoader scenesLoader,
            LoadingScreenView.Factory loadingScreenViewFactory,
            GlobalUIDocumentMonoWrapper globalUIDocument,
            TextKey.Factory textKeyFactory)
        {
            _scenesLoader = scenesLoader;
            _loadingScreenViewFactory = loadingScreenViewFactory;
            _globalUIDocument = globalUIDocument;
            _textKeyFactory = textKeyFactory;
        }

        public bool TryReloadActiveScene()
        {
            return TryGoToScene(_scenesLoader.ActiveScene);
        }

        public bool TryGoToScene(SceneType sceneType)
        {
            if (_scenesLoader.SceneLoading || _sceneToLoad != null)
                return false;

            _sceneToLoad = sceneType;

            _activeLoadingScreen = CreateLoadingScreenViewByScene(sceneType);
            _activeLoadingScreen.ShowingAnimationEnded += OnLoadingScreenShowingAnimationEnded;
            _activeLoadingScreen.Show();

            return true;
        }

        private void OnLoadingScreenShowingAnimationEnded(LoadingScreenView loadingScreen)
        {
            loadingScreen.ShowingAnimationEnded -= OnLoadingScreenShowingAnimationEnded;

            _scenesLoader.OnNewSceneAsyncLoading += OnNewSceneAsyncLoading;
            _scenesLoader.OnNewSceneAsyncLoadFinished += OnNewSceneAsyncLoadFinished;

            Assert.That(_scenesLoader.TryGoToSceneAsync(_sceneToLoad.Value));
        }

        private void OnNewSceneAsyncLoading(SceneType scene, float progress)
        {
            _activeLoadingScreen.UpdateProgressValue(progress * 100);
        }

        private void OnNewSceneAsyncLoadFinished(SceneType obj)
        {
            _activeLoadingScreen.UpdateProgressValue(100);

            _scenesLoader.OnNewSceneAsyncLoadFinished -= OnNewSceneAsyncLoadFinished;
            _scenesLoader.OnNewSceneAsyncLoading -= OnNewSceneAsyncLoading;

            _activeLoadingScreen.Disposing += OnLoadingScreenDisposing;

            _activeLoadingScreen.Close();
        }

        private void OnLoadingScreenDisposing(LoadingScreenView obj)
        {
            _activeLoadingScreen.Disposing -= OnLoadingScreenDisposing;

            _activeLoadingScreen = null;
            _sceneToLoad = null;
        }

        private LoadingScreenView CreateLoadingScreenViewByScene(SceneType sceneType, bool showImmediately = false)
        {
            LoadingScreenCreationData creationData = new(
                _globalUIDocument.LoadingScreenCreationRoot,
                GetTitleKeyByScene(sceneType),
                GetMessageKeyByScene(sceneType),
                showImmediately);

            return _loadingScreenViewFactory.Create(creationData);
        }

        private TextKey GetTitleKeyByScene(SceneType sceneType)
            => sceneType switch
            {
                SceneType.GameScene => _textKeyFactory.Create(TextKeysConstants.LoadingScreen.SwitchingToGameScene_Title, null),

                _ => throw Assert.GetFailException(),
            };

        private TextKey GetMessageKeyByScene(SceneType sceneType)
            => sceneType switch
            {
                SceneType.GameScene => _textKeyFactory.Create(TextKeysConstants.LoadingScreen.SwitchingToGameScene_Message, null),

                _ => throw Assert.GetFailException(),
            };
    }
}
