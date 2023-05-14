using ConnectIt.Infrastructure.Factories;
using ConnectIt.Shop.Goods;
using ConnectIt.Stats.Data;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Menu.Views;
using ConnectIt.UI.Menu.Views.GJLoginMenu;
using ConnectIt.UI.Menu.Views.GJMenu;
using ConnectIt.UI.Menu.Views.MainMenu;
using ConnectIt.UI.Menu.Views.SelectLevelMenu;
using ConnectIt.UI.Menu.Views.SettingsMenu;
using ConnectIt.UI.Menu.Views.ShopMenu;
using ConnectIt.UI.Menu.Views.StatsMenu;
using ConnectIt.UI.Tools;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class MenuSceneInstaller : MonoInstaller
{
    [SerializeField] private VisualTreeAsset _productAsset;
    [SerializeField] private VisualTreeAsset _statsElementAsset;

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
        BindGJLoginMenuViews();
        BindGJMenuViews();

        void BindGlobalViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<Label, VersionView, VersionView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, VersionView>>();

                Container.BindFactory<Label, CompletedLevelsView, CompletedLevelsView.Factory>()
                         .FromFactory<PrimitiveDIFactory<Label, CompletedLevelsView>>();
            }
        }

        void BindMainMenuUIViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, MainMenuView, MainMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, MainMenuView>>();
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

        void BindGJLoginMenuViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, GJLoginMenuView, GJLoginMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, GJLoginMenuView>>();
            }
        }

        void BindGJMenuViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, GJMenuView, GJMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, GJMenuView>>();
            }
        }
    }
}
