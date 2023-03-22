using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Input;
using ConnectIt.Input.GameplayInputRouterStates;
using ConnectIt.Model;
using ConnectIt.MonoWrappers;
using UnityEngine;
using Zenject;
using Factories = ConnectIt.Infrastructure.Factories;

namespace ConnectIt.DI.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private TilemapsMonoWrapper _tilemapsMonoWrapper;

        public override void InstallBindings()
        {
            BindTilemaps();
            BindGameplayInput();
            BindGameplayInputRouter();
            BindGameplayInputRouterStateFactories();
            BindCreatedConnectionLineNotifier();
        }

        private void BindCreatedConnectionLineNotifier()
        {
            Container.Bind<ICreatedObjectNotifier<ConnectionLine>>()
                     .To<CreatedObjectNotifier<ConnectionLine>>()
                     .FromNew()
                     .AsSingle();
        }

        private void BindGameplayInputRouterStateFactories()
        {
            Container.Bind<Factories.IFactory<CreatingConnectionLineState>>()
                     .To<Factories.DIFactory<CreatingConnectionLineState>>()
                     .FromNew()
                     .AsTransient();

            Container.Bind<Factories.IFactory<RemovingConnectionLineState>>()
                     .To<Factories.DIFactory<RemovingConnectionLineState>>()
                     .FromNew()
                     .AsTransient();

            Container.Bind<Factories.IFactory<IdleTilemapsInteractionState>>()
                     .To<Factories.DIFactory<IdleTilemapsInteractionState>>()
                     .FromNew()
                     .AsTransient();
        }

        private void BindGameplayInputRouter()
        {
            //todo
            Container.Bind<GameplayInputRouter>()
                     .FromNew()
                     .AsSingle()
                     .NonLazy();
        }

        private void BindGameplayInput()
        {
            Container.Bind<GameplayInput>()
                     .FromNew()
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