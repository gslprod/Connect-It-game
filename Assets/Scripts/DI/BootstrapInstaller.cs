using ConnectIt.Audio.OST;
using ConnectIt.Audio.Sounds;
using ConnectIt.Config;
using ConnectIt.Config.ScriptableObjects;
using ConnectIt.Coroutines;
using ConnectIt.ErrorHandling;
using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.ExternalServices.GameJolt.Fixators.Scores;
using ConnectIt.Gameplay.Data;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Localization;
using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.Savers;
using ConnectIt.Save.Serializers;
using ConnectIt.Scenes;
using ConnectIt.Scenes.Switchers;
using ConnectIt.Security.ConfidentialData;
using ConnectIt.Security.Encryption;
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Customer.Storage;
using ConnectIt.Shop.Customer.Wallet;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Stats;
using ConnectIt.Stats.Data;
using ConnectIt.Stats.Modules;
using ConnectIt.Time;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.CustomControls;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Global.MonoWrappers;
using ConnectIt.UI.LoadingScreen;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities.Formatters;
using GameJolt.API;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
        [SerializeField] private ConfidentialValuesSO _realConfidentialValues;
        [SerializeField] private ConfidentialValuesSO _placeholderConfidentialValues;
        [SerializeField] private GameJoltAPI _gameJoltAPIPrefab;
        [SerializeField] private VisualTreeAsset _loadingDialogBoxAsset;
        [SerializeField] private OSTAudioSourceMonoWrapper _ostAudioSourceMonoWrapper;
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private SoundsAudioSourceMonoWrapper _soundsAudioSourceMonoWrapper;
        [SerializeField] private AudioConfigSO _audioConfig;

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
            BindStats();
            BindConfidentialData();
            BindGameJoltAPI();
            BindAudio();
            BindEncryptor();
            BindKeyGenerator();
            BindErrorHandling();
        }

        private void BindErrorHandling()
        {
            Container.BindInterfacesTo<SaveErrorHandler>()
                     .AsSingle();

            Container.BindInitializableExecutionOrder<SaveErrorHandler>(ExecutionOrderConstants.Initializable.ErrorHandler);
        }

        private void BindKeyGenerator()
        {
            Container.Bind<IKeyGenerator>()
                     .To<SimpleKeyGenerator>()
                     .AsSingle();
        }

        private void BindEncryptor()
        {
            Container.Bind<ISymmetricEncryptor>()
                     .To<SymmetricEncryptor>()
                     .AsSingle();
        }

        private void BindAudio()
        {
            Container.BindInstance(_mixer)
                     .AsSingle();

            BindOST();
            BindSounds();

            void BindSounds()
            {
                Container.BindInterfacesAndSelfTo<SoundsPlayer>()
                         .AsSingle()
                         .NonLazy();

                Container.Bind<SoundsAudioSourceMonoWrapper>()
                         .FromComponentInNewPrefab(_soundsAudioSourceMonoWrapper)
                         .AsSingle()
                         .WhenInjectedInto<SoundsPlayer>();
            }

            void BindOST()
            {
                Container.BindInterfacesAndSelfTo<OSTPlayer>()
                         .AsSingle()
                         .NonLazy();

                Container.Bind<OSTAudioSourceMonoWrapper>()
                         .FromComponentInNewPrefab(_ostAudioSourceMonoWrapper)
                         .AsSingle()
                         .WhenInjectedInto<OSTPlayer>();
            }
        }

        private void BindGameJoltAPI()
        {
            BindFixators();

            Container.Bind<GameJoltAPI>()
                     .FromComponentInNewPrefab(_gameJoltAPIPrefab)
                     .AsSingle();

            Container.BindInterfacesAndSelfTo<GameJoltAPIProvider>()
                     .AsSingle();

            Container.BindInterfacesAndSelfTo<ExternalServices.GameJolt.Sessions>()
                     .AsSingle();

            Container.BindInterfacesAndSelfTo<ExternalServices.GameJolt.Scores>()
                     .AsSingle();

            void BindFixators()
            {
                Container.BindInterfacesTo<TotalScoresFixator>()
                         .AsSingle()
                         .NonLazy();
            }
        }

        private void BindConfidentialData()
        {
            Container.Bind<ConfidentialValues>()
                     .AsSingle()
                     .WithArguments(_realConfidentialValues != null ? _realConfidentialValues : _placeholderConfidentialValues);
        }

        private void BindStats()
        {
            Container.BindInterfacesTo<StatsCenter>()
                     .AsSingle();

            BindStatsDataFactories();
            BindStatsModules();

            void BindStatsDataFactories()
            {
                Container.BindFactory<ApplicationRunningTimeStatsData, ApplicationRunningTimeStatsData.Factory>()
                         .FromFactory<PrimitiveDIFactory<ApplicationRunningTimeStatsData>>();

                Container.BindFactory<MovesCountStatsData, MovesCountStatsData.Factory>()
                         .FromFactory<PrimitiveDIFactory<MovesCountStatsData>>();

                Container.BindFactory<TotalEarnedCoinsStatsData, TotalEarnedCoinsStatsData.Factory>()
                         .FromFactory<PrimitiveDIFactory<TotalEarnedCoinsStatsData>>();

                Container.BindFactory<TotalReceivedItemsCountStatsData, TotalReceivedItemsCountStatsData.Factory>()
                         .FromFactory<PrimitiveDIFactory<TotalReceivedItemsCountStatsData>>();

                Container.BindFactory<FirstLaunchedVersionStatsData, FirstLaunchedVersionStatsData.Factory>()
                         .FromFactory<PrimitiveDIFactory<FirstLaunchedVersionStatsData>>();
            }

            void BindStatsModules()
            {
                Container.BindInterfacesTo<ApplicationRunningTimeStatsModule>()
                         .AsSingle();

                Container.BindInterfacesTo<TotalEarnedCoinsStatsModule>()
                         .AsSingle();

                Container.BindInterfacesTo<TotalReceivedItemsCountStatsModule>()
                         .AsSingle();

                Container.BindInterfacesTo<FirstLaunchedVersionStatsModule>()
                         .AsSingle();
            }
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

                    Container.BindFactory<SimplifyLevelBoost, SimplifyLevelBoost.Factory>()
                             .FromFactory<PrimitiveDIFactory<SimplifyLevelBoost>>();
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
            Container.BindInterfacesTo<PlayerStorage>()
                     .AsSingle();
        }

        private void BindWallet()
        {
            Container.BindInterfacesTo<PlayerWallet>()
                     .AsSingle();
        }

        private void BindLevelsPassDataProvider()
        {
            Container.BindInterfacesTo<LevelsPassDataProvider>()
                     .AsSingle();
        }

        private void BindSave()
        {
            Container.BindInterfacesAndSelfTo<GameSaveProvider>()
                     .AsSingle();

            Container.BindInitializableExecutionOrder<GameSaveProvider>(ExecutionOrderConstants.Initializable.GameSaveProvider);

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

                Container.BindFactory<TextElement, TextKey, DefaultLocalizedTextElementView, DefaultLocalizedTextElementView.Factory>()
                         .FromFactory<PrimitiveDIFactory<TextElement, TextKey, DefaultLocalizedTextElementView>>();

                Container.BindFactory<Button, Action, TextKey, DefaultLocalizedButtonView, DefaultLocalizedButtonView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Button, Action, TextKey, DefaultLocalizedButtonView>>();

                Container.BindFactory<Label, CoinsView, CoinsView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, CoinsView>>();

                Container.BindFactory<Button, Action, ClickableCoinsView, ClickableCoinsView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Button, Action, ClickableCoinsView>>();

                Container.BindFactory<Label, GameJoltUsernameView, GameJoltUsernameView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, GameJoltUsernameView>>();

                Container.BindFactory<TextElement, TextKey, string, DefaultUniversalTextElementView, DefaultUniversalTextElementView.Factory>()
                         .FromFactory<PrimitiveDIFactory<TextElement, TextKey, string, DefaultUniversalTextElementView>>();

                Container.BindFactory<VisualElement, Sprite, DefaultSpriteView, DefaultSpriteView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, Sprite, DefaultSpriteView>>();

                Container.BindFactory<ButtonToggle, TextKey, TextKey, TextKey, DefaultLocalizedButtonToggleView, DefaultLocalizedButtonToggleView.Factory>()
                         .FromFactory<PrimitiveDIFactory<ButtonToggle, TextKey, TextKey, TextKey, DefaultLocalizedButtonToggleView>>();
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

                Container.BindFactory<LoadingDialogBoxViewCreationData, LoadingDialogBoxView, LoadingDialogBoxView.Factory>()
                         .FromFactory<PrimitiveDIFactory<LoadingDialogBoxViewCreationData, LoadingDialogBoxView>>();
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

                Container.BindInstance(_loadingDialogBoxAsset)
                         .AsCached()
                         .WhenInjectedInto<LoadingDialogBoxView>();
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
            BindAudioConfig();

            void BindAudioConfig()
            {
                Container.Bind<AudioConfig>()
                         .AsSingle()
                         .WithArguments(_audioConfig);
            }

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