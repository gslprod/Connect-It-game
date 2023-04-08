using ConnectIt.DI.Installers.Custom;
using ConnectIt.Gameplay.LevelLoading;
using ConnectIt.Gameplay.Model;
using ConnectIt.Gameplay.MonoWrappers;
using ConnectIt.Gameplay.Observers;
using ConnectIt.Gameplay.Pause;
using ConnectIt.Gameplay.Time;
using ConnectIt.Gameplay.View;
using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.Dispose;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Infrastructure.Registrators;
using ConnectIt.Infrastructure.Spawners;
using ConnectIt.Input;
using ConnectIt.Input.GameplayInputRouterStates;
using ConnectIt.UI.Gameplay.Views;
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

        public override void InstallBindings()
        {
            BindTilemaps();
            BindGameplayInput();
            BindConnectionLine();
            BindPort();
            BindGameStateObserver();
            BindUIViews();
            BindTime();
            BindLevelLoader();
            BindPauseService();
        }

        private void BindPauseService()
        {
            Container.BindInterfacesTo<PauseService>()
                     .AsSingle();
        }

        private void BindLevelLoader()
        {
            Container.Bind<LevelLoader>()
                     .AsSingle()
                     .WithArguments(_tilemapsMonoWrapperPrefabs);
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

            void BindUIViewsFactories()
            {
                Container.BindFactory<CustomControls.ProgressBar, LevelProgressView, LevelProgressView.Factory>()
                         .FromFactory<PrimitiveDIFactory<CustomControls.ProgressBar, LevelProgressView>>();

                Container.BindFactory<Label, TimeView, TimeView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, TimeView>>();

                Container.BindFactory<Label, LevelView, LevelView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, LevelView>>();

                Container.BindFactory<Button, Action, DefaultButtonView, DefaultButtonView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Button, Action, DefaultButtonView>>();
            }
        }

        private void BindGameStateObserver()
        {
            Container.BindInterfacesTo<GameStateObserver>()
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

                Container.BindInitializableExecutionOrder<ViewFromModelSpawner<Port, PortView, PortView.Factory>>(-10);
            }

            void BindRegistrator()
            {
                Container.BindInterfacesTo<CreatedObjectsRegistrator<Port>>()
                         .AsSingle();

                Container.BindInitializableExecutionOrder<CreatedObjectsRegistrator<Port>>(-10);
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

                Container.BindInitializableExecutionOrder<ViewFromModelSpawner<ConnectionLine, ConnectionLineView, ConnectionLineView.Factory>>(-10);
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