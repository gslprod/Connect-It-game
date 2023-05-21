using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Menu.MonoWrappers;
using ConnectIt.UI.Tools;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.MainMenu
{
    public class MainMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly FramesSwitcher<VisualElement> _framesSwitcher;
        private readonly MenuUIDocumentMonoWrapper _menuUIDocumentMonoWrapper;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;

        private DefaultButtonView _playButtonView;
        private DefaultButtonView _settingsButtonView;
        private DefaultButtonView _shopButtonView;
        private DefaultButtonView _gjApiButtonView;
        private DefaultButtonView _statsButtonView;

        public MainMenuView(VisualElement viewRoot,
            FramesSwitcher<VisualElement> switcher,
            MenuUIDocumentMonoWrapper menuUIDocumentMonoWrapper,
            DefaultButtonView.Factory defaultButtonViewFactory)
        {
            _viewRoot = viewRoot;
            _framesSwitcher = switcher;
            _menuUIDocumentMonoWrapper = menuUIDocumentMonoWrapper;
            _defaultButtonViewFactory = defaultButtonViewFactory;
        }

        public void Initialize()
        {
            CreateViews();
        }

        public void Dispose()
        {
            DisposeDisposableViews();
        }

        private void CreateViews()
        {
            _playButtonView = _defaultButtonViewFactory
                .Create(_viewRoot.Q<Button>(NameConstants.MainMenu.PlayButton), OnPlayButtonClick);

            _settingsButtonView = _defaultButtonViewFactory
                .Create(_viewRoot.Q<Button>(NameConstants.MainMenu.SettingsButton), OnSettingsButtonClick);

            _shopButtonView = _defaultButtonViewFactory
                .Create(_viewRoot.Q<Button>(NameConstants.MainMenu.ShopButton), OnShopButtonClick);

            _gjApiButtonView = _defaultButtonViewFactory
                .Create(_viewRoot.Q<Button>(NameConstants.MainMenu.GjApiButton), OnGjApiButtonClick);

            _statsButtonView = _defaultButtonViewFactory
                .Create(_viewRoot.Q<Button>(NameConstants.MainMenu.StatsButton), OnStatsButtonClick);
        }

        private void DisposeDisposableViews()
        {
            _playButtonView.Dispose();
            _settingsButtonView.Dispose();
            _shopButtonView.Dispose();
            _gjApiButtonView.Dispose();
            _statsButtonView.Dispose();
        }

        #region PlayButton

        private void OnPlayButtonClick()
        {
            _framesSwitcher.SwitchTo(_menuUIDocumentMonoWrapper.SelectLevelContainer);
        }

        #endregion

        #region SettingsButton

        private void OnSettingsButtonClick()
        {
            _framesSwitcher.SwitchTo(_menuUIDocumentMonoWrapper.SettingsContainer);
        }

        #endregion

        #region ShopButton

        private void OnShopButtonClick()
        {
            _framesSwitcher.SwitchTo(_menuUIDocumentMonoWrapper.ShopContainer);
        }

        #endregion

        #region GjApiButton

        private void OnGjApiButtonClick()
        {
            //todo
            _framesSwitcher.SwitchTo(_menuUIDocumentMonoWrapper.GJContainer);
        }

        #endregion

        #region StatsButton

        private void OnStatsButtonClick()
        {
            _framesSwitcher.SwitchTo(_menuUIDocumentMonoWrapper.StatsContainer);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, FramesSwitcher<VisualElement>, MenuUIDocumentMonoWrapper, MainMenuView> { }
    }
}