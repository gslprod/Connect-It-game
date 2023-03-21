using ConnectIt.Input;
using ConnectIt.Model;
using Zenject;

namespace ConnectIt.DI.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindTilemaps();
            BindGameplayInput();
            BindGameplayInputRouter();
        }

        private void BindGameplayInputRouter()
        {
            Container.Bind<GameplayInputRouter>()
                     .FromNew()
                     .AsSingle();
        }

        private void BindGameplayInput()
        {
            Container.Bind<GameplayInput>()
                     .FromNew()
                     .AsTransient();
        }

        private void BindTilemaps()
        {
            Container.Bind<Tilemaps>()
                     .FromInstance(null)
                     .AsSingle()
                     .NonLazy();
        }
    }
}