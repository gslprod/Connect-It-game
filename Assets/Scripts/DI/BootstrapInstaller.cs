using ConnectIt.Config;
using ConnectIt.Config.ScriptableObjects;
using ConnectIt.Coroutines;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Localization;
using ConnectIt.Scenes;
using ConnectIt.Scenes.Switchers;
using ConnectIt.Time;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Global.MonoWrappers;
using ConnectIt.UI.LoadingScreen;
using ConnectIt.Utilities.Formatters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.DI.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameplayLogicConfigSO _gameplayLogicConfig;
        [SerializeField] private GameplayViewConfigSO _gameplayViewConfig;
        [SerializeField] private FormatterConfig _formatterConfig;
        [SerializeField] private VisualTreeAsset _dialogBoxAsset;
        [SerializeField] private VisualTreeAsset _dialogBoxButtonAsset;
        [SerializeField] private CoroutinesGlobalContainer _coroutinesGlobalContainerPrefab;
        [SerializeField] private GlobalUIDocumentMonoWrapper _globalUIDocumentPrefab;
        [SerializeField] private VisualTreeAsset _loadingScreenAsset;

        public override void InstallBindings()
        {
            BindRenderCameraProvider();
            BindConfig();
            BindFormatter();
            BindTime();
            BindLocalization();
            BindDialogBox();
            BindCoroutinesGlobalContainer();
            BindScenesLoader();
            BindGlobalUIDocument();
            BindLoadingScreen();
            BindSceneSwitcher();
        }

        private void BindSceneSwitcher()
        {
            Container.Bind<ISceneSwitcher>()
                     .To<SceneSwitcherWithLoadingScreen>()
                     .AsSingle();
        }

        private void BindLoadingScreen()
        {
            Container.BindFactory<LoadingScreenCreationData, LoadingScreenView, LoadingScreenView.Factory>()
                     .FromFactory<PrimitiveDIFactory<LoadingScreenCreationData, LoadingScreenView>>();

            Container.BindInstance(_loadingScreenAsset)
                     .AsCached()
                     .WhenInjectedInto<LoadingScreenView>();
        }

        private void BindGlobalUIDocument()
        {
            Container.Bind<GlobalUIDocumentMonoWrapper>()
                     .FromComponentInNewPrefab(_globalUIDocumentPrefab)
                     .AsSingle()
                     .NonLazy();
        }

        private void BindScenesLoader()
        {
            Container.Bind<IScenesLoader>()
                     .To<ScenesLoader>()
                     .AsSingle();
        }

        private void BindCoroutinesGlobalContainer()
        {
            Container.Bind<ICoroutinesGlobalContainer>()
                     .To<CoroutinesGlobalContainer>()
                     .FromComponentInNewPrefab(_coroutinesGlobalContainerPrefab)
                     .AsSingle();
        }

        private void BindDialogBox()
        {
            BindFactories();
            BindAssets();

            void BindFactories()
            {
                Container.BindFactory<DialogBoxButtonInfo, Button, DialogBoxView, DialogBoxButton, DialogBoxButton.Factory>()
                         .FromFactory<PrimitiveDIFactory<DialogBoxButtonInfo, Button, DialogBoxView, DialogBoxButton>>();

                Container.BindFactory<DialogBoxCreationData, DialogBoxView, DialogBoxView.Factory>()
                         .FromFactory<PrimitiveDIFactory<DialogBoxCreationData, DialogBoxView>>();
            }

            void BindAssets()
            {
                Container.BindInstance(_dialogBoxAsset)
                         .WithId(DialogBoxView.DialogBoxAssetId)
                         .AsCached()
                         .WhenInjectedInto<DialogBoxView>();

                Container.BindInstance(_dialogBoxButtonAsset)
                         .WithId(DialogBoxView.DialogBoxButtonAssetId)
                         .AsCached()
                         .WhenInjectedInto<DialogBoxView>();
            }
        }

        private void BindLocalization()
        {
            Container.BindInterfacesTo<LocalizationProvider>()
                     .AsSingle();

            Container.BindFactory<string, IEnumerable<object>, TextKey, TextKey.Factory>()
                     .FromFactory<PrimitiveDIFactory<string, IEnumerable<object>, TextKey>>();
        }

        private void BindTime()
        {
            Container.BindInterfacesTo<TimeProvider>()
                     .AsSingle();
        }

        private void BindFormatter()
        {
            Container.Bind<IFormatter>()
                     .To<Formatter>()
                     .AsSingle()
                     .WithArguments(_formatterConfig);
        }

        private void BindConfig()
        {
            BindGameplayConfig();
        }

        private void BindGameplayConfig()
        {
            Container.Bind<GameplayLogicConfig>()
                     .AsSingle()
                     .WithArguments(_gameplayLogicConfig);

            Container.Bind<GameplayViewConfig>()
                     .AsSingle()
                     .WithArguments(_gameplayViewConfig);
        }

        private void BindRenderCameraProvider()
        {
            Container.Bind<RenderCameraProvider>()
                     .AsSingle();
        }
    }
}