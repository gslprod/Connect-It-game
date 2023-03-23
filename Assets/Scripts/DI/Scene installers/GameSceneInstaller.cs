using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.Factories;
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
        [SerializeField] private PortView _portViewPrefab;

        public override void InstallBindings()
        {
            BindTilemaps();
            BindGameplayInput();
            BindGameplayInputRouter();
            BindGameplayInputRouterStateFactories();
            BindCreatedObjectsNotifiers();
            BindViewFactories();
            BindPortViewPrefab();
        }

        private void BindPortViewPrefab()
        {
            Container.Bind<PortView>()
                     .FromInstance(_portViewPrefab)
                     .AsSingle();
        }

        private void BindViewFactories()
        {
            Container.BindFactory<Port, PortView, PortView.Factory>()
                     .FromFactory<PrimitiveDIViewFromModelFactory<Port, PortView>>();
        }

        private void BindCreatedObjectsNotifiers()
        {
            Container.Bind<ICreatedObjectNotifier<ConnectionLine>>()
                     .To<CreatedObjectNotifier<ConnectionLine>>()
                     .AsSingle();

            Container.Bind<ICreatedObjectNotifier<Port>>()
                     .To<CreatedObjectNotifier<Port>>()
                     .AsSingle();
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

        private void BindGameplayInputRouter()
        {
            Container.BindInterfacesAndSelfTo<GameplayInputRouter>()
                     .AsSingle()
                     .NonLazy();
        }

        private void BindGameplayInput()
        {
            Container.Bind<GameplayInput>()
                     .AsSingle();
        }

        private void BindTilemaps()
        {
            //todo
            Container.Bind<Tilemaps>()
                     .FromInstance(_tilemapsMonoWrapper.Model)
                     .AsSingle()
                     .NonLazy();
        }
    }
}