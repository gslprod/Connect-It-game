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
            Container.Bind<Factories.IFactory<ConnectionsGameplayInputRouterState>>()
                     .To<Factories.DIFactory<ConnectionsGameplayInputRouterState>>()
                     .FromNew()
                     .AsTransient();
        }

        private void BindGameplayInputRouter()
        {
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
            Container.Bind<Tilemaps>()
                     .FromInstance(_tilemapsMonoWrapper.Model)
                     .AsSingle()
                     .NonLazy();
        }
    }
}