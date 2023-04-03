using ConnectIt.Config;
using ConnectIt.Config.ScriptableObjects;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Localization;
using ConnectIt.Time;
using ConnectIt.Utilities.Formatters;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ConnectIt.DI.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameplayLogicConfigSO _gameplayLogicConfig;
        [SerializeField] private GameplayViewConfigSO _gameplayViewConfig;
        [SerializeField] private FormatterConfig _formatterConfig;

        public override void InstallBindings()
        {
            BindRenderCameraProvider();
            BindConfig();
            BindFormatter();
            BindTime();
            BindLocalization();
        }

        private void BindLocalization()
        {
            Container.BindInterfacesTo<LocalizationProvider>()
                     .AsSingle();

            Container.BindFactory<string, IEnumerable<object>, TextKey, TextKey.Factory>()
                     .FromFactory<PrimitiveDIFactory<string, IEnumerable<object>, TextKey>>();
        }

        private void BindTime()
        {
            Container.BindInterfacesTo<TimeProvider>()
                     .AsSingle();
        }

        private void BindFormatter()
        {
            Container.Bind<IFormatter>()
                     .To<Formatter>()
                     .AsSingle()
                     .WithArguments(_formatterConfig);
        }

        private void BindConfig()
        {
            BindGameplayConfig();
        }

        private void BindGameplayConfig()
        {
            Container.Bind<GameplayLogicConfig>()
                     .AsSingle()
                     .WithArguments(_gameplayLogicConfig);

            Container.Bind<GameplayViewConfig>()
                     .AsSingle()
                     .WithArguments(_gameplayViewConfig);
        }

        private void BindRenderCameraProvider()
        {
            Container.Bind<RenderCameraProvider>()
                     .AsSingle();
        }
    }
}