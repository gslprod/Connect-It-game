namespace ConnectIt.Localization
{
    public static class TextKeysConstants
    {
        public static class Gameplay
        {
            public const string Level = GameplayBaseString + ".LevelLabel.Title";
            public const string PauseMenu_Continue = GameplayBaseString + ".PauseMenu.Continue";
            public const string PauseMenu_Exit = GameplayBaseString + ".PauseMenu.Exit";

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
            public const string VersionLabel_Title = MenuBaseString + ".VersionLabel.Title";

            private const string MenuBaseString = "Menu";
        }
    }
}
