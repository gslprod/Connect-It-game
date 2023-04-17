using ConnectIt.Infrastructure.Factories;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Menu.Views;
using ConnectIt.UI.Menu.Views.GJLoginMenu;
using ConnectIt.UI.Menu.Views.GJMenu;
using ConnectIt.UI.Menu.Views.MainMenu;
using ConnectIt.UI.Menu.Views.SelectLevelMenu;
using ConnectIt.UI.Menu.Views.SettingsMenu;
using ConnectIt.UI.Menu.Views.StatsMenu;
using ConnectIt.UI.Tools;
using UnityEngine.UIElements;
using Zenject;

public class MenuSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindUIViews();
    }

    private void BindUIViews()
    {
        BindGlobalViews();
        BindMainMenuUIViews();
        BindSelectLevelMenuViews();
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
                Container.BindFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, SelectLevelMenuView, SelectLevelMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, SelectLevelMenuView>>();
            }
        }

        void BindSettingsMenuViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, SettingsMenuView, SettingsMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, SettingsMenuView>>();
            }
        }

        void BindStatsMenuViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, StatsMenuView, StatsMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, StatsMenuView>>();
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
