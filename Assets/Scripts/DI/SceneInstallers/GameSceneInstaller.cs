using ConnectIt.DI.Installers.Custom;
using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.Dispose;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Infrastructure.Registrators;
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
        [SerializeField] private Transform _connectionLineParent;

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
            Container.Bind<ConnectionLineView>()
                     .FromInstance(_connectionLinePrefab)
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

            //todo
            Container.Bind<TilemapsMonoWrapper>()
                     .FromInstance(_tilemapsMonoWrapper)
                     .AsSingle()
                     .NonLazy();
        }
    }
}