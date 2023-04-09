using ConnectIt.Gameplay.LevelLoading;
using ConnectIt.Gameplay.Model;
using ConnectIt.Gameplay.MonoWrappers;
using Zenject;

namespace ConnectIt.DI.Installers.Custom
{
    public class TilemapsInstaller : Installer<TilemapsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Tilemaps>().AsSingle();

            Container.Bind<TilemapsMonoWrapper>()
                     .FromResolveGetter<LevelContentLoader>(loader => loader.InstantiateTilemapsPrefab())
                     .AsSingle()
                     .NonLazy();

            Container.Bind<TilemapLayerSet[]>()
                     .FromResolveGetter<TilemapsMonoWrapper>(tilemapsMonoWrapper => tilemapsMonoWrapper.TilemapLayers)
                     .AsSingle();

            Container.Bind<TileBaseAndObjectInfoSet[]>()
                     .FromResolveGetter<TilemapsMonoWrapper>(tilemapsMonoWrapper => tilemapsMonoWrapper.ObjectsInfo)
                     .AsSingle();
        }
    }
}
