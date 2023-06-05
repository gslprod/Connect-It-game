namespace ConnectIt.UI
{
    public static class ClassNamesConstants
    {
        public static class Global
        {
            public const string ScrollViewContainerChild = "scroll-view-container__child";
            public const string ScrollViewContainerChildLast = "scroll-view-container__child--last";

            #region DialogBox

            public const string DialogBoxRoot = "dialog-box__root";
            public const string DialogBoxRootClosed = "dialog-box__root--closed";
            public const string DialogBoxContainerClosed = "dialog-box__container--closed";
            public const string DialogBoxButton = "dialog-box__button";
            public const string DialogBoxButtonAdditional = "dialog-box__button--additional";
            public const string DialogBoxButtonNotLastInLayout = "dialog-box__button--not-last-in-layout";
            public const string DialogBoxButtonDismiss = "dialog-box__button--dismiss";
            public const string DialogBoxButtonAccept = "dialog-box__button--accept";

            public const string LoadingBoxRoot = "loading-box__root";
            public const string LoadingBoxRootClosed = "loading-box__root--closed";
            public const string LoadingBoxContainer = "loading-box__container";
            public const string LoadingBoxContainerClosed = "loading-box__container--closed";
            public const string LoadingBoxSpinningIcon360Rotated = "loading-box__spinning-icon--360-rotated";

            #endregion

            #region LoadingScreen

            public const string LoadingScreenRoot = "loading-screen__root";
            public const string LoadingScreenRootClosed = "loading-screen__root--closed";
            public const string LoadingScreenContainerClosed = "loading-screen__container--closed";
            public const string LoadingScreenProgressBarLabel = "loading-screen__loading-progress-bar-label";

            #endregion
        }

        public static class GlobalView
        {
            public const string BlockPanelDisabled = "block-panel--disabled";
        }

        public static class MenuView
        {
            public const string ContentContainerFrameClosed = "content-container__frame--closed";

            #region SelectLevel

            public const string LevelButtonCompleted = "level-button--completed";
            public const string LevelButtonCompletedWithBoosts = "level-button--completed-with-boosts";
            public const string LevelButtonCurrent = "level-button--current";
            public const string LevelButtonSkipped = "level-button--skipped";

            #endregion

            #region GJ

            public const string GJAccountInfoTopPositionLabelHidden = "gj-account-info-container__top-position-label--hidden";

            public const string GJScoreTopPositionPattern = "gj-score--top-{0}-position";

            #endregion
        }

        public static class GameplayView
        {
            #region UseBoostElement

            public const string BoostElementRootUsed = "boost-element__root--used";

            public const string BoostsContainerChild = "boosts-container__child";
            public const string BoostsContainerChildLast = "boosts-container__child--last";

            #endregion
        }
    }
}
