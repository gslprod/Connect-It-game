using ConnectIt.Config;
using ConnectIt.Config.ScriptableObjects;
using ConnectIt.Coroutines;
using ConnectIt.Gameplay.Data;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Localization;
using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.Savers;
using ConnectIt.Save.Serializers;
using ConnectIt.Scenes;
using ConnectIt.Scenes.Switchers;
using ConnectIt.Time;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Global.MonoWrappers;
using ConnectIt.UI.LoadingScreen;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Formatters;
using System;
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
        [SerializeField] private GameVersionSO _gameVersionConfig;

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
            BindUIViews();
            BindUIBlocker();
            BindSave();
            BindLevelsPassDataProvider();
        }

        private void BindLevelsPassDataProvider()
        {
            Container.BindInterfacesTo<LevelsPassDataProvider>()
                     .AsSingle();
        }

        private void BindSave()
        {
            Container.BindInterfacesTo<GameSaveProvider>()
                     .AsSingle();

            Container.BindInterfacesTo<FileSaver>()
                     .AsSingle();

            Container.BindInitializableExecutionOrder<FileSaver>(ExecutionOrderConstants.Initializable.FileSaver);

            Container.Bind<ISerializer>()
                     .To<UnityJSONSerializer>()
                     .AsSingle();
        }

        private void BindUIBlocker()
        {
            Container.Bind<IUIBlocker>()
                     .To<GlobalUIDocumentMonoWrapper>()
                     .FromResolve()
                     .AsCached();
        }

        private void BindUIViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<Button, Action, DefaultButtonView, DefaultButtonView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Button, Action, DefaultButtonView>>();

                Container.BindFactory<Label, TextKey, DefaultLocalizedLabelView, DefaultLocalizedLabelView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, TextKey, DefaultLocalizedLabelView>>();
            }
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
                     .AsCached()
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
            BindGameVersionConfig();

            void BindGameplayConfig()
            {
                Container.Bind<GameplayLogicConfig>()
                         .AsSingle()
                         .WithArguments(_gameplayLogicConfig);

                Container.Bind<GameplayViewConfig>()
                         .AsSingle()
                         .WithArguments(_gameplayViewConfig);
            }

            void BindGameVersionConfig()
            {
                Container.Bind<GameVersion>()
                         .AsSingle()
                         .WithArguments(_gameVersionConfig);
            }
        }

        private void BindRenderCameraProvider()
        {
            Container.Bind<RenderCameraProvider>()
                     .AsSingle();
        }
    }
}