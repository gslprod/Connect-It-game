using ConnectIt.Config;
using UnityEngine;
using Zenject;

namespace ConnectIt.DI.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameplayLogicConfig _gameplayLogicConfig;
        [SerializeField] private GameplayViewConfig _gameplayViewConfig;

        public override void InstallBindings()
        {
            BindRenderCameraProvider();
            BindConfig();
        }

        private void BindConfig()
        {
            BindGameplayConfig();
        }

        private void BindGameplayConfig()
        {
            Container.Bind<GameplayLogicConfig>()
                     .FromInstance(_gameplayLogicConfig)
                     .AsSingle();

            Container.Bind<GameplayViewConfig>()
                     .FromInstance(_gameplayViewConfig)
                     .AsSingle();
        }

        private void BindRenderCameraProvider()
        {
            Container.Bind<RenderCameraProvider>()
                     .AsSingle();
        }
    }
}