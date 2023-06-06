using ConnectIt.DI.Installers.Custom;
using ConnectIt.Gameplay.GameStateHandlers.GameEnd;
using ConnectIt.Gameplay.GameStateHandlers.Shop;
using ConnectIt.Gameplay.LevelLoading;
using ConnectIt.Gameplay.Model;
using ConnectIt.Gameplay.MonoWrappers;
using ConnectIt.Gameplay.Observers;
using ConnectIt.Gameplay.Observers.Internal;
using ConnectIt.Gameplay.Pause;
using ConnectIt.Gameplay.Time;
using ConnectIt.Gameplay.Tools.Calculators;
using ConnectIt.Gameplay.TutorialsSystem;
using ConnectIt.Gameplay.TutorialsSystem.Tutorials;
using ConnectIt.Gameplay.View;
using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.Dispose;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Infrastructure.Factories.Concrete;
using ConnectIt.Infrastructure.Registrators;
using ConnectIt.Infrastructure.Spawners;
using ConnectIt.Input;
using ConnectIt.Input.GameplayInputRouterStates;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
using ConnectIt.Stats.Modules;
using ConnectIt.UI.Gameplay.MonoWrappers;
using ConnectIt.UI.Gameplay.Views;
using ConnectIt.UI.Gameplay.Views.UseBoostMenu;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using CustomControls = ConnectIt.UI.CustomControls;

namespace ConnectIt.DI.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private ConnectionLineView _connectionLinePrefab;
        [SerializeField] private Transform _connectionLineParent;
        [SerializeField] private TilemapsMonoWrapper[] _tilemapsMonoWrapperPrefabs;
        [SerializeField] private GameplayUIDocumentMonoWrapper _gameplayUIDocumentMonoWrapper;
        [SerializeField] private VisualTreeAsset _useBoostMenuAsset;
        [SerializeField] private VisualTreeAsset _useBoostElementAsset;

        public override void InstallBindings()
        {
            BindTilemaps();
            BindGameplayInput();
            BindConnectionLine();
            BindPort();
            BindGameStateObservers();
            BindUIViews();
            BindTime();
            BindLevelLoading();
            BindPauseService();
            BindUIDocumentMonoWrapper();
            BindGameStateHandlers();
            BindBoostUsageContexts();
            BindTools();
            BindStats();
            BindTutorialsSystem();
        }

        private void BindTutorialsSystem()
        {
            Container.Bind<ITutorialsStarter>()
                     .To<TutorialsStarter>()
                     .AsSingle();

            BindTutorials();

            void BindTutorials()
            {
                Container.BindInterfacesTo<IntroductoryTutorial>()
                         .AsSingle();

                Container.BindInterfacesTo<RemoveConnectionsTutorial>()
                         .AsSingle();
            }
        }

        private void BindStats()
        {
            BindStatsModules();

            void BindStatsModules()
            {
                Container.BindInterfacesTo<MovesCountStatsModule>()
                         .AsSingle();

                Container.BindInterfacesTo<BoostsUsageCountStatsModule>()
                         .AsSingle();
            }
        }

        private void BindTools()
        {
            BindCalculators();

            void BindCalculators()
            {
                Container.BindInterfacesTo<ScoresCalculator>()
                         .AsSingle();

                Container.BindInterfacesTo<CoinsCalculator>()
                         .AsSingle();
            }
        }

        private void BindBoostUsageContexts()
        {
            Container.BindInterfacesTo<CreatedObjectNotifier<BoostUsageContext>>()
                     .AsSingle();

            Container.BindFactory<CommonUsageData, BoostUsageContext, BoostUsageContext.Factory>()
                     .FromFactory<BoostUsageContextFactory>()
                     .WhenNotInjectedInto<BoostUsageContextFactory>();

            Container.BindFactory<CommonUsageData, BoostUsageContext, BoostUsageContext.Factory>()
                     .FromFactory<PrimitiveDIFactory<CommonUsageData, BoostUsageContext>>()
                     .WhenInjectedInto<BoostUsageContextFactory>();

            Container.BindFactory<CommonUsageData, SkipLevelBoostUsageContext, SkipLevelBoostUsageContext.Factory>()
                     .FromFactory<PrimitiveDIFactory<CommonUsageData, SkipLevelBoostUsageContext>>()
                     .WhenInjectedInto<BoostUsageContextFactory>();

            Container.BindFactory<CommonUsageData, SimplifyLevelBoostUsageContext, SimplifyLevelBoostUsageContext.Factory>()
                     .FromFactory<PrimitiveDIFactory<CommonUsageData, SimplifyLevelBoostUsageContext>>()
                     .WhenInjectedInto<BoostUsageContextFactory>();

            Container.BindFactory<CommonUsageData, AllowIncompatibleConnectionsBoostUsageContext, AllowIncompatibleConnectionsBoostUsageContext.Factory>()
                     .FromFactory<PrimitiveDIFactory<CommonUsageData, AllowIncompatibleConnectionsBoostUsageContext>>()
                     .WhenInjectedInto<BoostUsageContextFactory>();
        }

        private void BindGameStateHandlers()
        {
            BindGameEndHandlers();
            BindShopHandlers();

            void BindGameEndHandlers()
            {
                Container.BindInterfacesTo<LevelEndHandler>()
                                     .AsSingle();

                BindFactories();

                void BindFactories()
                {
                    Container.BindFactory<IWinHandler, IWinHandler.Factory>()
                             .FromFactory<PrimitiveDIFactory<WinHandler>>()
                             .WhenInjectedInto<ILevelEndHandler>();

                    Container.BindFactory<IRestartHandler, IRestartHandler.Factory>()
                             .FromFactory<PrimitiveDIFactory<RestartHandler>>()
                             .WhenInjectedInto<ILevelEndHandler>();

                    Container.BindFactory<ISkipHandler, ISkipHandler.Factory>()
                             .FromFactory<PrimitiveDIFactory<SkipHandler>>()
                             .WhenInjectedInto<ILevelEndHandler>();

                    Container.BindFactory<IExitToMainMenuHandler, IExitToMainMenuHandler.Factory>()
                             .FromFactory<PrimitiveDIFactory<ExitToMainMenuHandler>>()
                             .WhenInjectedInto<ILevelEndHandler>();

                    Container.BindFactory<IGoToNextLevelHandler, IGoToNextLevelHandler.Factory>()
                             .FromFactory<PrimitiveDIFactory<GoToNextLevelHandler>>()
                             .WhenInjectedInto<ILevelEndHandler>();
                }
            }

            void BindShopHandlers()
            {
                Container.BindInterfacesTo<ReversibleDisposableItemsHandler<Boost>>()
                         .AsSingle();
            }
        }

        private void BindUIDocumentMonoWrapper()
        {
            Container.Bind<GameplayUIDocumentMonoWrapper>()
                     .FromInstance(_gameplayUIDocumentMonoWrapper)
                     .AsSingle();
        }

        private void BindPauseService()
        {
            Container.BindInterfacesTo<PauseService>()
                     .AsSingle();
        }

        private void BindLevelLoading()
        {
            Container.Bind<LevelContentLoader>()
                     .AsSingle()
                     .WithArguments(_tilemapsMonoWrapperPrefabs);

            Container.BindInterfacesTo<CameraSetup>()
                     .AsSingle();

            Container.BindInitializableExecutionOrder<CameraSetup>(ExecutionOrderConstants.Initializable.CameraSetup);
        }

        private void BindTime()
        {
            Container.Bind<GameplayTimeProvider>()
                     .AsSingle();

            Container.Bind<IGameplayTimeProvider>()
                     .To<GameplayTimeProvider>()
                     .FromResolve()
                     .AsCached();

            Container.Bind<IInitializable>()
                     .To<GameplayTimeProvider>()
                     .FromResolve()
                     .AsCached();

            Container.Bind<ITickable>()
                     .To<GameplayTimeProvider>()
                     .FromResolve()
                     .AsCached();

            Container.Bind<IDisposable>()
                     .To<GameplayTimeProvider>()
                     .FromResolve()
                     .AsCached();
        }

        private void BindUIViews()
        {
            BindUIViewsFactories();
            BindAssets();

            void BindUIViewsFactories()
            {
                Container.BindFactory<CustomControls.ProgressBar, LevelProgressView, LevelProgressView.Factory>()
                         .FromFactory<PrimitiveDIFactory<CustomControls.ProgressBar, LevelProgressView>>();

                Container.BindFactory<Label, TimeView, TimeView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, TimeView>>();

                Container.BindFactory<Label, LevelView, LevelView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, LevelView>>();

                Container.BindFactory<VisualElement, VisualElement, UseBoostMenuView, UseBoostMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, UseBoostMenuView>>();

                Container.BindFactory<Type, VisualElement, VisualElement, UseBoostElementView, UseBoostElementView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Type, VisualElement, VisualElement, UseBoostElementView>>();
            }

            void BindAssets()
            {
                Container.BindInstance(_useBoostMenuAsset)
                         .AsCached()
                         .WhenInjectedInto<GameplayUIDocumentMonoWrapper>();

                Container.BindInstance(_useBoostElementAsset)
                         .AsCached()
                         .WhenInjectedInto<UseBoostElementView>();
            }
        }

        private void BindGameStateObservers()
        {
            Container.BindInterfacesTo<GameStateObserver>()
                     .AsSingle();

            Container.BindInterfacesAndSelfTo<GameProgressObserver>()
                     .AsSingle();

            Container.BindInterfacesAndSelfTo<MovesObserver>()
                     .AsSingle();

            Container.BindInterfacesAndSelfTo<UsedBoostsObserver>()
                     .AsSingle();
        }

        private void BindPort()
        {
            Container.Bind<ICreatedObjectNotifier<Port>>()
                     .To<CreatedObjectNotifier<Port>>()
                     .AsSingle();

            BindFactories();

            BindViewFromModelSpawner();

            BindRegistrator();

            Container.BindInterfacesTo<Disposer<Port>>()
                     .AsSingle();

            void BindViewFromModelSpawner()
            {
                Container.BindInterfacesTo<ViewFromModelSpawner<Port, PortView, PortView.Factory>>()
                         .AsSingle();

                Container.BindInitializableExecutionOrder<ViewFromModelSpawner<Port, PortView, PortView.Factory>>(ExecutionOrderConstants.Initializable.PortViewFromModelSpawner);
            }

            void BindRegistrator()
            {
                Container.BindInterfacesTo<CreatedObjectsRegistrator<Port>>()
                         .AsSingle();

                Container.BindInitializableExecutionOrder<CreatedObjectsRegistrator<Port>>(ExecutionOrderConstants.Initializable.CreatedPortsRegistrator);
            }

            void BindFactories()
            {
                Container.BindFactory<Tile, int, Port, Port.Factory>()
                         .FromFactory<PrimitiveDIFactory<Tile, int, Port>>();

                Container.BindFactory<Port, PortView, PortView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Port, PortView>>();
            }
        }

        private void BindConnectionLine()
        {
            Container.BindInstance(_connectionLinePrefab)
                     .AsSingle();

            Container.Bind<ICreatedObjectNotifier<ConnectionLine>>()
                     .To<CreatedObjectNotifier<ConnectionLine>>()
                     .AsSingle();

            BindFactories();

            BindViewFromModelSpawner();

            void BindViewFromModelSpawner()
            {
                Container.BindInterfacesTo<ViewFromModelSpawner<ConnectionLine, ConnectionLineView, ConnectionLineView.Factory>>()
                         .AsSingle();

                Container.BindInitializableExecutionOrder<ViewFromModelSpawner<ConnectionLine, ConnectionLineView, ConnectionLineView.Factory>>(
                         ExecutionOrderConstants.Initializable.ConnectionLineViewFromModelSpawner);
            }

            void BindFactories()
            {
                Container.BindFactory<Port, ConnectionLine, ConnectionLine.Factory>()
                         .FromFactory<PrimitiveDIFactory<Port, ConnectionLine>>();

                Container.BindFactory<ConnectionLine, ConnectionLineView, ConnectionLineView.Factory>()
                         .FromIFactory(binder =>
                         binder.To<ParentingMonoBehaviourDIFactory<ConnectionLine, ConnectionLineView>>()
                               .AsCached()
                               .WithArguments(_connectionLineParent));
            }
        }

        private void BindGameplayInput()
        {
            Container.Bind<GameplayInput>()
                     .AsSingle();

            Container.BindInterfacesAndSelfTo<GameplayInputRouter>()
                     .AsSingle();

            BindGameplayInputRouterStateFactories();
        }

        private void BindGameplayInputRouterStateFactories()
        {
            Container.BindFactory<ConnectionLine, CreatingConnectionLineState, CreatingConnectionLineState.Factory>()
                     .AsCached();

            Container.BindFactory<ConnectionLine, RemovingConnectionLineState, RemovingConnectionLineState.Factory>()
                     .AsCached();

            Container.BindFactory<IdleTilemapsInteractionState, IdleTilemapsInteractionState.Factory>()
                     .AsCached();
        }

        private void BindTilemaps()
        {
            Container.BindInterfacesAndSelfTo<Tilemaps>()
                     .FromSubContainerResolve()
                     .ByInstaller<TilemapsInstaller>()
                     .AsSingle()
                     .NonLazy();

            Container.BindFactory<TilemapsMonoWrapper, TilemapsMonoWrapper, TilemapsMonoWrapper.Factory>()
                     .FromFactory<MonoBehaviourPrefabDIFactory<TilemapsMonoWrapper>>();
        }
    }
}