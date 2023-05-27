using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.GJMenu.GJProfileMenu
{
    public class GJProfileMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedTextElementView.Factory _defaultTextElementViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly GameJoltUsernameView.Factory _gameJoltUsernameViewFactory;
        private readonly GameJoltAPIProvider _gjAPIProvider;
        private readonly DialogBoxView.Factory _dialogBoxViewFactory;
        private readonly GJScoreboardsView.Factory _gjScoreboardsViewFactory;

        private DefaultLocalizedTextElementView _titleLabel;
        private GameJoltUsernameView _gameJoltUsernameLabel;
        private DefaultButtonView _logOutButton;
        private DefaultLocalizedTextElementView _scoresTitleLabel;
        private GJScoreboardsView _gjScoreboardsView;

        public GJProfileMenuView(
            VisualElement viewRoot,
            VisualElement mainRoot,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedTextElementView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            GameJoltUsernameView.Factory gameJoltUsernameViewFactory,
            GameJoltAPIProvider gjAPIProvider,
            DialogBoxView.Factory dialogBoxViewFactory,
            GJScoreboardsView.Factory gjScoreboardsViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultTextElementViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _gameJoltUsernameViewFactory = gameJoltUsernameViewFactory;
            _gjAPIProvider = gjAPIProvider;
            _dialogBoxViewFactory = dialogBoxViewFactory;
            _gjScoreboardsViewFactory = gjScoreboardsViewFactory;
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
            _titleLabel = _defaultTextElementViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJMenu.GJProfileMenu.TitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJProfileMenu.Title));

            _gameJoltUsernameLabel = _gameJoltUsernameViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJMenu.GJProfileMenu.UsernameLabel));

            _logOutButton = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(NameConstants.GJMenu.GJProfileMenu.LogOutButton),
                OnLogOutButtonClick);

            _scoresTitleLabel = _defaultTextElementViewFactory.Create(
                _viewRoot.Q<Label>(NameConstants.GJMenu.GJProfileMenu.ScoresTitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJProfileMenu.ScoresTitleLabel_Text));

            _gjScoreboardsView = _gjScoreboardsViewFactory.Create(
                _viewRoot.Q<VisualElement>(NameConstants.GJMenu.GJProfileMenu.GJScrollViewInternalContainer), _mainRoot);
        }

        private void DisposeDisposableViews()
        {
            _titleLabel.Dispose();
            _gameJoltUsernameLabel.Dispose();
            _logOutButton.Dispose();
            _scoresTitleLabel.Dispose();
            _gjScoreboardsView.Dispose();
        }

        #region LogOutButton

        private void OnLogOutButtonClick()
        {
            _dialogBoxViewFactory.CreateDefaultConfirmCancelDialogBox(
                _mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.ConfirmLogout_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.ConfirmLogout_Message),
                _textKeyFactory,
                OnLogOutConfirmButtonClick,
                true);
        }

        private void OnLogOutConfirmButtonClick()
        {
            _gjAPIProvider.LogOut();
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, GJProfileMenuView> { }
    }
}
