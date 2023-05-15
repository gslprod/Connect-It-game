namespace ConnectIt.Localization
{
    public static class TextKeysConstants
    {
        public static class Gameplay
        {
            public const string LevelLabel_Text = GameplayBaseString + ".LevelLabel.Text";
            public const string PauseMenu_Continue = GameplayBaseString + ".PauseMenu.Continue";
            public const string PauseMenu_Exit = GameplayBaseString + ".PauseMenu.Exit";
            public const string WinMenu_Title = GameplayBaseString + ".WinMenu.Title";
            public const string WinMenu_Content = GameplayBaseString + ".WinMenu.Content";
            public const string UseBoostButton_Text = GameplayBaseString + ".UseBoostButton.Text";
            public const string UseBoostMenu_Title = GameplayBaseString + ".UseBoostMenu.Title";
            public const string UseBoostMenu_InfoLabel_Text = GameplayBaseString + ".UseBoostMenu.InfoLabel.Text";
            public const string UseBoostMenu_UseBoostElement_AmountLabel_Text = GameplayBaseString + ".UseBoostMenu.UseBoostElement.AmountLabel.Text";
            public const string SkipMenu_Title = GameplayBaseString + ".SkipMenu.Title";
            public const string SkipMenu_Content = GameplayBaseString + ".SkipMenu.Content";

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

            public const string NotEnoughCoins_Title = DialogBoxBaseString + ".NotEnoughCoins.Title";
            public const string NotEnoughCoins_Message = DialogBoxBaseString + ".NotEnoughCoins.Message";

            public const string ConfirmBuy_Title = DialogBoxBaseString + ".ConfirmBuy.Title";
            public const string ConfirmBuy_Message = DialogBoxBaseString + ".ConfirmBuy.Message";

            public const string NotEnoughProductAmount_Title = DialogBoxBaseString + ".NotEnoughProductAmount.Title";
            public const string NotEnoughProductAmount_Message = DialogBoxBaseString + ".NotEnoughProductAmount.Message";

            public const string ConfirmBoostUse_Title = DialogBoxBaseString + ".ConfirmBoostUse.Title";
            public const string ConfirmBoostUse_Message = DialogBoxBaseString + ".ConfirmBoostUse.Message";

            public const string BoostUsed_Title = DialogBoxBaseString + ".BoostUsed.Title";
            public const string BoostUsed_Message = DialogBoxBaseString + ".BoostUsed.Message";

            public const string LanguageChange_Title = DialogBoxBaseString + ".LanguageChange.Title";
            public const string LanguageChange_Message = DialogBoxBaseString + ".LanguageChange.Message";

            private const string DialogBoxBaseString = "DialogBox";
        }

        public static class Common
        {
            public const string Confirm = CommonBaseString + ".Confirm";
            public const string Cancel = CommonBaseString + ".Cancel";
            public const string Play = CommonBaseString + ".Play";
            public const string Buy = CommonBaseString + ".Buy";
            public const string Close = CommonBaseString + ".Close";
            public const string Description = CommonBaseString + ".Description";
            public const string Use = CommonBaseString + ".Use";
            public const string NextLevel = CommonBaseString + ".NextLevel";
            public const string RestartLevel = CommonBaseString + ".RestartLevel";
            public const string MainMenu = CommonBaseString + ".MainMenu";

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
            public const string CompletedLevelsLabel_Text = MenuBaseString + ".CompletedLevelsLabel.Text";
            public const string VersionLabel_Text = MenuBaseString + ".VersionLabel.Text";
            public const string CoinsLabel_Text = MenuBaseString + ".CoinsLabel.Text";

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

            public static class ShopMenu
            {
                public const string Title = ShopMenuBaseString + ".Title";
                public const string CoinsInfoLabel_Text = ShopMenuBaseString + ".CoinsInfoLabel.Text";
                public const string GoodsTitleLabel_Text = ShopMenuBaseString + ".GoodsTitleLabel.Text";
                public const string Product_PriceLabel_Text = ShopMenuBaseString + ".Product.PriceLabel.Text";
                public const string Product_AmountLabel_Text = ShopMenuBaseString + ".Product.AmountLabel.Text";

                private const string ShopMenuBaseString = MenuBaseString + ".ShopMenu";
            }

            public static class SettingsMenu
            {
                public const string Title = SettingsMenuBaseString + ".Title";

                public const string LanguageButton_Text = SettingsMenuBaseString + ".LanguageButton.Text";

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

        public static class Items
        {
            public const string Boosts_SkipLevel_Name = ItemsBaseString + ".Product.Boost.SkipLevel.Name";
            public const string Boosts_SkipLevel_Description = ItemsBaseString + ".Product.Boost.SkipLevel.Description";

            public const string StatsData_ApplicationRunningTime_Name = ItemsBaseString + ".StatsData.ApplicationRunningTime.Name";
            public const string StatsData_ApplicationRunningTime_Description = ItemsBaseString + ".StatsData.ApplicationRunningTime.Description";
            public const string StatsData_ApplicationRunningTime_Value = ItemsBaseString + ".StatsData.ApplicationRunningTime.Value";

            public const string StatsData_MovesCount_Name = ItemsBaseString + ".StatsData.MovesCount.Name";
            public const string StatsData_MovesCount_Description = ItemsBaseString + ".StatsData.MovesCount.Description";
            public const string StatsData_MovesCount_Value = ItemsBaseString + ".StatsData.MovesCount.Value";

            private const string ItemsBaseString = "Items";
        }

        public static class Languages
        {
            public const string Pattern = LanguagesBaseString + ".{0}";

            public const string English = LanguagesBaseString + ".English";
            public const string Russian = LanguagesBaseString + ".Russian";

            private const string LanguagesBaseString = "Languages";
        }
    }
}
