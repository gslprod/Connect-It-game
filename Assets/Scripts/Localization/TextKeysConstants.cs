namespace ConnectIt.Localization
{
    public static class TextKeysConstants
    {
        public static class Gameplay
        {
            public const string Level = GameplayBaseString + ".LevelLabel.Title";
            public const string PauseMenu_Continue = GameplayBaseString + ".PauseMenu.Continue";
            public const string PauseMenu_Exit = GameplayBaseString + ".PauseMenu.Exit";
            public const string WinMenu_Title = GameplayBaseString + ".WinMenu.Title";
            public const string WinMenu_Content = GameplayBaseString + ".WinMenu.Content";
            public const string WinMenu_MainMenu = GameplayBaseString + ".WinMenu.MainMenu";
            public const string WinMenu_Restart = GameplayBaseString + ".WinMenu.Restart";
            public const string WinMenu_NextLevel = GameplayBaseString + ".WinMenu.NextLevel";

            private const string GameplayBaseString = "Gameplay";
        }

        public static class DialogBox
        {
            public const string QuitLevelConfirm_Title = DialogBoxBaseString + ".QuitLevelConfirm.Title";
            public const string QuitLevelConfirm_Message = DialogBoxBaseString + ".QuitLevelConfirm.Message";

            public const string RestartLevelConfirm_Title = DialogBoxBaseString + ".RestartLevelConfirm.Title";
            public const string RestartLevelConfirm_Message = DialogBoxBaseString + ".RestartLevelConfirm.Message";

            public const string LevelPaused_Title = DialogBoxBaseString + ".LevelPaused.Title";
            public const string LevelPaused_Message = DialogBoxBaseString + ".LevelPaused.Message";

            private const string DialogBoxBaseString = "DialogBox";
        }

        public static class Common
        {
            public const string Confirm = CommonBaseString + ".Confirm";
            public const string Cancel = CommonBaseString + ".Cancel";
            public const string Play = CommonBaseString + ".Play";

            private const string CommonBaseString = "Common";
        }

        public static class LoadingScreen
        {
            public const string SwitchingToGameScene_Title = LoadingScreenBaseString + ".SwitchingToGameScene.Title";
            public const string SwitchingToGameScene_Message = LoadingScreenBaseString + ".SwitchingToGameScene.Message";
            public const string SwitchingToMenuScene_Title = LoadingScreenBaseString + ".SwitchingToMenuScene.Title";
            public const string SwitchingToMenuScene_Message = LoadingScreenBaseString + ".SwitchingToMenuScene.Message";

            private const string LoadingScreenBaseString = "LoadingScreen";
        }

        public static class Menu
        {
            public const string CompletedLevelsLabel_Title = MenuBaseString + ".CompletedLevelsLabel.Title";
            public const string VersionLabel_Title = MenuBaseString + ".VersionLabel.Title";

            private const string MenuBaseString = "Menu";

            public static class SelectLevelMenu
            {
                public const string Title = SelectLevelMenuBaseString + ".Title";

                public const string ChoosenLevelInfo_Title = SelectLevelMenuBaseString + ".ChoosenLevelInfo.Title";
                public const string ChoosenLevelInfo_Content_Completed = SelectLevelMenuBaseString + ".ChoosenLevelInfo.Content.Completed";
                public const string ChoosenLevelInfo_Content_NotCompleted = SelectLevelMenuBaseString + ".ChoosenLevelInfo.Content.NotCompleted";
                public const string ChoosenLevelInfo_Content_NotAvailable = SelectLevelMenuBaseString + ".ChoosenLevelInfo.Content.NotAvailable";
                public const string ChoosenLevelInfo_Content_Skipped = SelectLevelMenuBaseString + ".ChoosenLevelInfo.Content.Skipped";

                private const string SelectLevelMenuBaseString = MenuBaseString + ".SelectLevelMenu";
            }

            public static class SettingsMenu
            {
                public const string Title = SettingsMenuBaseString + ".Title";

                private const string SettingsMenuBaseString = MenuBaseString + ".SettingsMenu";
            }

            public static class StatsMenu
            {
                public const string Title = StatsMenuBaseString + ".Title";

                private const string StatsMenuBaseString = MenuBaseString + ".StatsMenu";
            }

            public static class GJMenu
            {
                public const string Title = GJMenuBaseString + ".Title";

                private const string GJMenuBaseString = MenuBaseString + ".GJMenu";
            }

            public static class GJLoginMenu
            {
                public const string Title = GJLoginMenuBaseString + ".Title";

                private const string GJLoginMenuBaseString = MenuBaseString + ".GJLoginMenu";
            }
        }
    }
}
