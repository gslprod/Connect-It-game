using ConnectIt.ExternalServices.GameJolt.Objects;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Shop.Goods;
using ConnectIt.Stats.Data;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Menu.Views;
using ConnectIt.UI.Menu.Views.GJMenu;
using ConnectIt.UI.Menu.Views.GJMenu.GJLoginMenu;
using ConnectIt.UI.Menu.Views.GJMenu.GJProfileMenu;
using ConnectIt.UI.Menu.Views.MainMenu;
using ConnectIt.UI.Menu.Views.SelectLevelMenu;
using ConnectIt.UI.Menu.Views.SettingsMenu;
using ConnectIt.UI.Menu.Views.ShopMenu;
using ConnectIt.UI.Menu.Views.StatsMenu;
using ConnectIt.UI.Tools;
using GameJolt.API.Objects;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class MenuSceneInstaller : MonoInstaller
{
    [SerializeField] private VisualTreeAsset _productAsset;
    [SerializeField] private VisualTreeAsset _statsElementAsset;
    [SerializeField] private Sprite _defaultGJAvatarSprite;
    [SerializeField] private VisualTreeAsset _gjScoreAsset;
    [SerializeField] private VisualTreeAsset _gjScoreboardAsset;

    public override void InstallBindings()
    {
        BindUIViews();
    }

    private void BindUIViews()
    {
        BindGlobalViews();
        BindMainMenuUIViews();
        BindSelectLevelMenuViews();
        BindShopMenuViews();
        BindSettingsMenuViews();
        BindStatsMenuViews();
        BindGJMenuViews();

        void BindGlobalViews()
        {
            BindUIViewsFactories();
            BindInstances();

            void BindUIViewsFactories()
            {
                Container.BindFactory<Label, VersionView, VersionView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, VersionView>>();

                Container.BindFactory<Label, CompletedLevelsView, CompletedLevelsView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, CompletedLevelsView>>();

                Container.BindFactory<VisualElement, GameJoltAvatarView, GameJoltAvatarView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, GameJoltAvatarView>>();
            }

            void BindInstances()
            {
                Container.BindInstance(_defaultGJAvatarSprite)
                         .AsSingle()
                         .WhenInjectedInto<GameJoltAvatarView>();
            }
        }

        void BindMainMenuUIViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, MainMenuView, MainMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, MainMenuView>>();
            }
        }

        void BindSelectLevelMenuViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, SelectLevelMenuView, SelectLevelMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, SelectLevelMenuView>>();

                Container.BindFactory<VisualElement, VisualElement, SelectLevelButtonsView, SelectLevelButtonsView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, SelectLevelButtonsView>>();

                Container.BindFactory<Button, int, Action<int>, SelectLevelButtonView, SelectLevelButtonView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Button, int, Action<int>, SelectLevelButtonView>>();
            }
        }

        void BindShopMenuViews()
        {
            BindUIViewsFactories();
            BindAssets();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, ShopMenuView, ShopMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, ShopMenuView>>();

                Container.BindFactory<VisualElement, VisualElement, GoodsView, GoodsView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, GoodsView>>();

                Container.BindFactory<ShowcaseProduct<IProduct>, VisualElement, VisualElement, ProductView, ProductView.Factory>()
                         .FromFactory<PrimitiveDIFactory<ShowcaseProduct<IProduct>, VisualElement, VisualElement, ProductView>>();

                Container.BindFactory<IStatsData, VisualElement, VisualElement, StatsElementView, StatsElementView.Factory>()
                         .FromFactory<PrimitiveDIFactory<IStatsData, VisualElement, VisualElement, StatsElementView>>();

                Container.BindFactory< VisualElement, VisualElement, StatsElementsListView, StatsElementsListView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, StatsElementsListView>>();
            }

            void BindAssets()
            {
                Container.BindInstance(_productAsset)
                         .AsCached()
                         .WhenInjectedInto<ProductView>();

                Container.BindInstance(_statsElementAsset)
                         .AsCached()
                         .WhenInjectedInto<StatsElementView>();
            }
        }

        void BindSettingsMenuViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, SettingsMenuView, SettingsMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, SettingsMenuView>>();
            }
        }

        void BindStatsMenuViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, StatsMenuView, StatsMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, StatsMenuView>>();
            }
        }

        void BindGJMenuViews()
        {
            BindGJProfileMenuViews();
            BindGJLoginMenuViews();

            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, GJMenuView, GJMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, GJMenuView>>();
            }

            void BindGJLoginMenuViews()
            {
                BindUIViewsFactories();

                void BindUIViewsFactories()
                {
                    Container.BindFactory<VisualElement, VisualElement, GJLoginMenuView, GJLoginMenuView.Factory>()
                             .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, GJLoginMenuView>>();
                }
            }

            void BindGJProfileMenuViews()
            {
                BindUIViewsFactories();
                BindAssets();

                void BindUIViewsFactories()
                {
                    Container.BindFactory<VisualElement, VisualElement, GJProfileMenuView, GJProfileMenuView.Factory>()
                             .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, GJProfileMenuView>>();

                    Container.BindFactory<TableInfo, VisualElement, VisualElement, GJScoreboardView, GJScoreboardView.Factory>()
                             .FromFactory<PrimitiveDIFactory<TableInfo, VisualElement, VisualElement, GJScoreboardView>>();

                    Container.BindFactory<ScoreInfo, VisualElement, VisualElement, GJScoreView, GJScoreView.Factory>()
                             .FromFactory<PrimitiveDIFactory<ScoreInfo, VisualElement, VisualElement, GJScoreView>>();

                    Container.BindFactory<VisualElement, VisualElement, GJScoreboardsView, GJScoreboardsView.Factory>()
                             .FromFactory<PrimitiveDIFactory<VisualElement, VisualElement, GJScoreboardsView>>();

                    Container.BindFactory<Label, GJTopPositionLabel, GJTopPositionLabel.Factory>()
                             .FromFactory<PrimitiveDIFactory<Label, GJTopPositionLabel>>();
                }

                void BindAssets()
                {
                    Container.BindInstance(_gjScoreAsset)
                             .AsCached()
                             .WhenInjectedInto<GJScoreView>();

                    Container.BindInstance(_gjScoreboardAsset)
                             .AsCached()
                             .WhenInjectedInto<GJScoreboardView>();
                }
            }
        }
    }
}
