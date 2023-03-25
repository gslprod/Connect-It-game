using Assets.Scripts.DI.CustomInstallers;
using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Infrastructure.Spawners;
using ConnectIt.Input;
using ConnectIt.Input.GameplayInputRouterStates;
using ConnectIt.Model;
using ConnectIt.MonoWrappers;
using ConnectIt.View;
using UnityEngine;
using Zenject;

namespace ConnectIt.DI.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private TilemapsMonoWrapper _tilemapsMonoWrapper;
        [SerializeField] private ConnectionLineView _connectionLinePrefab;

        public override void InstallBindings()
        {
            BindTilemaps();
            BindGameplayInput();
            BindConnectionLine();
            BindPort();
        }

        private void BindPort()
        {
            Container.Bind<ICreatedObjectNotifier<Port>>()
                     .To<CreatedObjectNotifier<Port>>()
                     .AsSingle();

            Container.BindFactory<Port, PortView, PortView.Factory>();

            Container.BindInterfacesTo<ViewFromModelSpawner<Port, PortView, PortView.Factory>>()
                     .AsSingle();

            Container.BindInitializableExecutionOrder<ViewFromModelSpawner<Port, PortView, PortView.Factory>>(-10);
        }

        private void BindConnectionLine()
        {
            Container.Bind<ConnectionLineView>()
                     .FromInstance(_connectionLinePrefab)
                     .AsSingle();

            Container.Bind<ICreatedObjectNotifier<ConnectionLine>>()
                     .To<CreatedObjectNotifier<ConnectionLine>>()
                     .AsSingle();

            Container.BindFactory<ConnectionLine, ConnectionLineView, ConnectionLineView.Factory>()
                     .FromFactory<PrimitiveMonoBehaviourDIFactory<ConnectionLine, ConnectionLineView>>();

            Container.BindInterfacesTo<ViewFromModelSpawner<ConnectionLine, ConnectionLineView, ConnectionLineView.Factory>>()
                     .AsSingle();

            Container.BindInitializableExecutionOrder<ViewFromModelSpawner<ConnectionLine, ConnectionLineView, ConnectionLineView.Factory>>(-10);
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

            //todo
            Container.Bind<TilemapsMonoWrapper>()
                     .FromInstance(_tilemapsMonoWrapper)
                     .AsSingle()
                     .NonLazy();
        }
    }
}