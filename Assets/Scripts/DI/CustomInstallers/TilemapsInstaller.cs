﻿using ConnectIt.Model;
using ConnectIt.MonoWrappers;
using Zenject;

namespace Assets.Scripts.DI.CustomInstallers
{
    public class TilemapsInstaller : Installer<TilemapsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Tilemaps>().AsSingle();

            Container.Bind<TilemapLayerSet[]>()
                        .FromResolveGetter<TilemapsMonoWrapper>(tilemapsMonoWrapper => tilemapsMonoWrapper.TilemapLayers)
                        .AsSingle();

            Container.Bind<TileBaseAndObjectInfoSet[]>()
                        .FromResolveGetter<TilemapsMonoWrapper>(tilemapsMonoWrapper => tilemapsMonoWrapper.ObjectsInfo)
                        .AsSingle();
        }
    }
}