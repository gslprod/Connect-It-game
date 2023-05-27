namespace ConnectIt.UI.Menu
{
    public static class NameConstants
    {
        public const string RootName = "root";

        public const string TopContainer = "top-container";
        public const string ContentContainer = "content-container";
        public const string BottomContainer = "bottom-container";

        public const string MainMenuContainer = "main-menu-container";
        public const string SelectLevelContainer = "select-level-container";
        public const string SettingsContainer = "settings-container";
        public const string StatsContainer = "stats-container";
        public const string ShopContainer = "shop-container";
        public const string GJContainer = "gj-container";

        public const string CompletedLevelsLabel = "account-level";
        public const string VersionLabel = "version-label";
        public const string CoinsLabel = "coins-label";
        public const string UsernameLabel = "account-name";
        public const string Avatar = "account-avatar";

        public static class MainMenu
        {
            public const string PlayButton = "play-button";
            public const string GjApiButton = "gj-api-button";
            public const string ShopButton = "shop-button";
            public const string SettingsButton = "settings-button";
            public const string StatsButton = "stats-button";
            public const string ExitButton = "exit-button";
        }

        public static class SelectLevelMenu
        {
            public const string TitleLabel = "title-label";
            public const string BackButton = "back-button";

            public const string LevelViewContainer = "level-view-container";
            public const string LevelButtonFormat = "level-{0}-button";
        }

        public static class ShopMenu
        {
            public const string TitleLabel = "title-label";
            public const string BackButton = "back-button";

            public const string CoinsLabel = "shop-coins-label";
            public const string CoinsInfoLabel = "shop-coins-info-label";
            public const string GoodsTitleLabel = "goods-container-title";
            public const string GoodsScrollViewContainer = "scroll-view-container";

        }

        public static class SettingsMenu
        {
            public const string TitleLabel = "title-label";
            public const string BackButton = "back-button";

            public const string SoundSlider = "sound-slider";
            public const string MusicSlider = "music-slider";
            public const string HQEffectsButton = "hq-effects-button";
            public const string LanguageButton = "language-button";

        }

        public static class StatsMenu
        {
            public const string TitleLabel = "title-label";
            public const string BackButton = "back-button";

            public const string StatsListScrollViewContainer = "stats-scroll-view-internal-container";
        }

        public static class GJMenu
        {
            public const string BackButton = "back-button";

            public const string GJLoginContainer = "gj-login-container";
            public const string GJProfileContainer = "gj-profile-container";

            public static class GJLoginMenu
            {
                public const string TitleLabel = "title-label";

                public const string GJInfoLabel = "gj-info-label";
                public const string UsernameLabel = "username-title";
                public const string UsernameInputField = "username-input-field";
                public const string TokenLabel = "token-title";
                public const string TokenInputField = "token-input-field";
                public const string LoginButton = "login-button";
                public const string AutoLoginToggle = "auto-login-toggle";
                public const string AutoLoginInfoButton = "auto-login-info-button";
            }

            public static class GJProfileMenu
            {
                public const string TitleLabel = "title-label";

                public const string UsernameLabel = "username-label";
                public const string LogOutButton = "logout-button";
                public const string TopUserPositionLabel = "top-position-label";
                public const string ScoresTitleLabel = "scores-title";
                public const string ChangeTableButton = "scores-change-table-button";
                public const string ScoreboardsContainer = "scoreboards-container";
                public const string ScoresContainer = "scores-container";
                public const string GJScrollViewInternalContainer = "gj-scroll-view-internal-container";
            }
        }
    }
}
