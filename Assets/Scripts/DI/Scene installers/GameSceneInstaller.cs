using ConnectIt.Model;
using Zenject;

namespace ConnectIt.DI.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindTilemaps();
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