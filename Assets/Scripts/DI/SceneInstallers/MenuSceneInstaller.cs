using ConnectIt.Infrastructure.Factories;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Menu.Views.MainMenu;
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
        BindMainMenuUIViews();

        void BindMainMenuUIViews()
        {
            BindUIViewsFactories();

            void BindUIViewsFactories()
            {
                Container.BindFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, MainMenuView, MainMenuView.Factory>()
                         .FromFactory<PrimitiveDIFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, MainMenuView>>();
            }
        }
    }
}
