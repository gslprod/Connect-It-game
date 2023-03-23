using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Input;
using ConnectIt.Input.GameplayInputRouterStates;
using ConnectIt.Model;
using ConnectIt.MonoWrappers;
using ConnectIt.View;
using UnityEngine;
using Zenject;
using Factories = ConnectIt.Infrastructure.Factories;

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
            BindViewFromModelFactories();
            BindPortViewPrefab();
        }

        private void BindPortViewPrefab()
        {
            Container.Bind<PortView>()
                     .FromInstance(_portViewPrefab)
                     .AsSingle();
        }

        private void BindViewFromModelFactories()
        {
            Container.Bind<IViewFromModelFactory<Port, PortView>>()
                     .To<MonoBehaviourViewFromModelDIAutoFactory<Port, PortView>>()
                     .AsSingle()
                     .NonLazy();
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
            //Container.Bind<CreatingConnectionLineState.IFactory>()
            //         .To<PrimitiveDIFactory<ConnectionLine, CreatingConnectionLineState>>()
            //         .AsCached();

            Container.Bind<Factories.IFactory<RemovingConnectionLineState>>()
                     .To<Factories.DIFactory<RemovingConnectionLineState>>()
                     .AsCached();

            Container.Bind<Factories.IFactory<IdleTilemapsInteractionState>>()
                     .To<Factories.DIFactory<IdleTilemapsInteractionState>>()
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