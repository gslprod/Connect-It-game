using ConnectIt.Config;
using ConnectIt.Model;
using Zenject;

namespace ConnectIt.DI.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindRenderCameraProvider();
            BindGameplayConfig();
        }

        private void BindGameplayConfig()
        {
            Container.Bind<GameplayConfig>()
                     .AsSingle();
        }

        private void BindRenderCameraProvider()
        {
            Container.Bind<RenderCameraProvider>()
                     .AsSingle();
        }
    }
}