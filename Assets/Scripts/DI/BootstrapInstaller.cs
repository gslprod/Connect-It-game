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
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Customer.Storage;
using ConnectIt.Shop.Customer.Wallet;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
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
        [SerializeField] private ShopConfigSO _shopConfig;
        [SerializeField] private VisualTreeAsset _customDialogBoxAsset;

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
            BindWallet();
            BindStorage();
            BindCustomer();
            BindShop();
            BindShopGoods();
        }

        private void BindShopGoods()
        {
            BindBoosts();

            void BindBoosts()
            {
                BindFactories();

                void BindFactories()
                {
                    Container.BindFactory<SkipLevelBoost, SkipLevelBoost.Factory>()
                             .FromFactory<PrimitiveDIFactory<SkipLevelBoost>>();
                }
            }
        }

        private void BindShop()
        {
            Container.BindInterfacesTo<Shop.Shop>()
                     .AsSingle();
        }

        private void BindCustomer()
        {
            Container.Bind<ICustomer>()
                     .To<Customer>()
                     .AsSingle();
        }

        private void BindStorage()
        {
            Container.Bind<IStorage>()
                     .To<Storage>()
                     .AsTransient();
        }

        private void BindWallet()
        {
            Container.Bind<IWallet>()
                     .To<Wallet>()
                     .AsTransient();
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

                Container.BindFactory<Button, Action, TextKey, DefaultLocalizedButtonView, DefaultLocalizedButtonView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Button, Action, TextKey, DefaultLocalizedButtonView>>();

                Container.BindFactory<Label, CoinsView, CoinsView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, CoinsView>>();

                Container.BindFactory<Button, Action, ClickableCoinsView, ClickableCoinsView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Button, Action, ClickableCoinsView>>();
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
                Container.BindFactory<DialogBoxButtonInfo, Button, IDialogBoxView, DialogBoxButton, DialogBoxButton.Factory>()
                         .FromFactory<PrimitiveDIFactory<DialogBoxButtonInfo, Button, IDialogBoxView, DialogBoxButton>>();

                Container.BindFactory<DialogBoxCreationData, DialogBoxView, DialogBoxView.Factory>()
                         .FromFactory<PrimitiveDIFactory<DialogBoxCreationData, DialogBoxView>>();

                Container.BindFactory<CustomDialogBoxCreationData, CustomDialogBoxView, CustomDialogBoxView.Factory>()
                         .FromFactory<PrimitiveDIFactory<CustomDialogBoxCreationData, CustomDialogBoxView>>();
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

                Container.BindInstance(_customDialogBoxAsset)
                         .WithId(CustomDialogBoxView.CustomDialogBoxAssetId)
                         .AsCached()
                         .WhenInjectedInto<CustomDialogBoxView>();

                Container.BindInstance(_dialogBoxButtonAsset)
                         .WithId(CustomDialogBoxView.DialogBoxButtonAssetId)
                         .AsCached()
                         .WhenInjectedInto<CustomDialogBoxView>();
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
            BindShopConfig();

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

            void BindShopConfig()
            {
                Container.Bind<ShopConfig>()
                         .AsSingle()
                         .WithArguments(_shopConfig);
            }
        }

        private void BindRenderCameraProvider()
        {
            Container.Bind<RenderCameraProvider>()
                     .AsSingle();
        }
    }
}