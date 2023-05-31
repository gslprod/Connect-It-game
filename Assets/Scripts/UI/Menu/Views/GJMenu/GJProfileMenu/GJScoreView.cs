using ConnectIt.ExternalServices.GameJolt.Objects;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.GJMenu.GJProfileMenu
{
    public class GJScoreView : IInitializable, IDisposable
    {
        private readonly ScoreInfo _scoreToShow;
        private readonly VisualElement _creationRoot;
        private readonly VisualElement _mainRoot;
        private readonly DefaultLocalizedTextElementView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly VisualTreeAsset _asset;
        private readonly DialogBoxView.Factory _dialogBoxFactory;
        private readonly DefaultUniversalTextElementView.Factory _defaultUniversalTextViewFactory;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;

        private VisualElement _viewRoot;
        private DefaultLocalizedTextElementView _positionLabel;
        private DefaultLocalizedTextElementView _usernameLabel;
        private DefaultLocalizedTextElementView _valueLabel;
        private DefaultButtonView _moreInfoButton;

        public GJScoreView(
            ScoreInfo score,
            VisualElement creationRoot,
            VisualElement mainRoot,
            DefaultLocalizedTextElementView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            VisualTreeAsset asset,
            DialogBoxView.Factory dialogBoxFactory,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultUniversalTextElementView.Factory defaultUniversalTextViewFactory)
        {
            _scoreToShow = score;
            _creationRoot = creationRoot;
            _mainRoot = mainRoot;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _asset = asset;
            _dialogBoxFactory = dialogBoxFactory;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultUniversalTextViewFactory = defaultUniversalTextViewFactory;
        }

        public void Initialize()
        {
            CreateViews();
        }

        public void Dispose()
        {
            DisposeDisposableViews();

            _viewRoot.RemoveFromHierarchy();
        }

        private void CreateViews()
        {
            _asset.CloneTree(_creationRoot);
            _viewRoot = _creationRoot.GetLastChild();

            SetupAppearance();

            _positionLabel = _defaultUniversalTextViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.GJScore.PositionLabel),
                null,
                _scoreToShow.Rank.ToString());

            _usernameLabel = _defaultUniversalTextViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.GJScore.UsernameLabel),
                null,
                _scoreToShow.GJScore.PlayerName);

            _valueLabel = _defaultUniversalTextViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.GJScore.ValueLabel),
                null,
                _scoreToShow.GJScore.Value.ToString());

            _moreInfoButton = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(TemplatesNameConstants.GJScore.MoreInfoButton),
                OnMoreInfoButtonClick);
        }

        private void SetupAppearance()
        {
            if (_scoreToShow.Rank > 3)
                return;

            _viewRoot.AddToClassList(
                string.Format(ClassNamesConstants.MenuView.GJScoreTopPositionPattern, _scoreToShow.Rank));
        }

        private void DisposeDisposableViews()
        {
            _positionLabel.Dispose();
            _usernameLabel.Dispose();
            _valueLabel.Dispose();
            _moreInfoButton.Dispose();
        }

        #region DescriptionButton

        private void OnMoreInfoButtonClick()
        {
            _dialogBoxFactory.CreateDefaultOneButtonDialogBox(_mainRoot,
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJProfileMenu.PlayerScoreDetailedInfo_Title,
                new object[]
                {
                    _scoreToShow.GJScore.PlayerName,
                }),
                _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJProfileMenu.PlayerScoreDetailedInfo,
                new object[]
                {
                    _scoreToShow.Rank,
                    _scoreToShow.GJScore.Value
                }),
                _textKeyFactory,
                true);
        }

        #endregion

        public class Factory : PlaceholderFactory<ScoreInfo, VisualElement, VisualElement, GJScoreView> { }
    }
}
