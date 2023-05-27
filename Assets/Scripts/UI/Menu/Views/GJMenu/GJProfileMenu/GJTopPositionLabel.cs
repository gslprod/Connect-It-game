using ConnectIt.Coroutines;
using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.ExternalServices.GameJolt.Objects;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.Tools;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.GJMenu.GJProfileMenu
{
    public class GJTopPositionLabel : DefaultLocalizedTextElementView
    {
        private readonly TextKey.Factory _textKeyFactory;
        private readonly GameJoltAPIProvider _gjApiProvider;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;

        private FramesSwitcher<GJScoreboardView> _framesSwitcher;
        private readonly TransitionsStopWaiter _transitionsStopWaiter = new();

        private TextKey _hasNoScoreTextKey;
        private TextKey _hasScoreTextKey;

        public GJTopPositionLabel(
            Label label,
            ILocalizationProvider localizationProvider,
            TextKey.Factory textKeyFactory,
            GameJoltAPIProvider gjAPIProvider,
            ICoroutinesGlobalContainer coroutinesGlobalContainer) : base(label, null, localizationProvider)
        {
            _textKeyFactory = textKeyFactory;
            _gjApiProvider = gjAPIProvider;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
        }

        public override void Initialize()
        {
            _hasNoScoreTextKey = _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJProfileMenu.TopPositionLabel_Text_HasNoScore);
            _hasScoreTextKey = _textKeyFactory.Create(TextKeysConstants.Menu.GJMenu.GJProfileMenu.TopPositionLabel_Text_HasScore);

            textKey = _hasNoScoreTextKey;

            _gjApiProvider.TablePlayerScoresChanged += OnTablePlayerScoresChanged;

            base.Initialize();
        }

        public override void Dispose()
        {
            _gjApiProvider.TablePlayerScoresChanged -= OnTablePlayerScoresChanged;
            if (_framesSwitcher != null )
                UnsubscribeFromFramesSwitcher();

            base.Dispose();
        }

        public void SetFramesSwitcher(FramesSwitcher<GJScoreboardView> framesSwitcher)
        {
            Assert.ArgIsNotNull(framesSwitcher);

            if (_framesSwitcher != null)
                UnsubscribeFromFramesSwitcher();

            _framesSwitcher = framesSwitcher;
            SubscribeToFramesSwitcher();
        }

        private void SubscribeToFramesSwitcher()
        {
            _framesSwitcher.FrameSwitched += OnFrameSwitched;
        }

        private void UnsubscribeFromFramesSwitcher()
        {
            _framesSwitcher.FrameSwitched -= OnFrameSwitched;
        }

        private void OnTablePlayerScoresChanged(TableInfo table)
        {
            if (_framesSwitcher?.Current == null ||
                !_framesSwitcher.Current.TargetTable.Equals(table) ||
                textElement.ClassListContains(ClassNamesConstants.MenuView.GJAccountInfoTopPositionLabelHidden))

                return;

            UpdateInfo();
        }

        private void OnFrameSwitched(GJScoreboardView obj)
        {
            textElement.AddToClassList(ClassNamesConstants.MenuView.GJAccountInfoTopPositionLabelHidden);
            _coroutinesGlobalContainer.DelayedAction(() => _transitionsStopWaiter.AbortCurrentAndWait(() =>
            {
                UpdateInfo();

                textElement.RemoveFromClassList(ClassNamesConstants.MenuView.GJAccountInfoTopPositionLabelHidden);
            }, textElement));
        }

        private void UpdateInfo()
        {
            IReadOnlyList<ScoreInfo> playerScores = _gjApiProvider.PlayerScoresInTables[_framesSwitcher.Current.TargetTable.GJTable.ID];
            if (playerScores.Count() == 0)
            {
                SetTextKey(_hasNoScoreTextKey);
                return;
            }

            SetTextKey(_hasScoreTextKey);

            ScoreInfo playerBestScore = playerScores[0];
            SetArgs(new object[]
            {
                playerBestScore.Rank,
                playerBestScore.GJScore.Value
            });
        }

        public new class Factory : PlaceholderFactory<Label, GJTopPositionLabel> { }
    }
}
