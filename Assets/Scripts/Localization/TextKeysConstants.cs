﻿namespace ConnectIt.Localization
{
    public static class TextKeysConstants
    {
        public static class Gameplay
        {
            public const string LevelLabel_Text = GameplayBaseString + ".LevelLabel.Text";
            public const string PauseMenu_Continue = GameplayBaseString + ".PauseMenu.Continue";
            public const string PauseMenu_Exit = GameplayBaseString + ".PauseMenu.Exit";
            public const string WinMenu_Title = GameplayBaseString + ".WinMenu.Title";
            public const string WinMenu_Title_BoostsUsed = GameplayBaseString + ".WinMenu.Title.BoostsUsed";
            public const string WinMenu_Content = GameplayBaseString + ".WinMenu.Content";
            public const string UseBoostButton_Text = GameplayBaseString + ".UseBoostButton.Text";
            public const string UseBoostMenu_Title = GameplayBaseString + ".UseBoostMenu.Title";
            public const string UseBoostMenu_InfoLabel_Text = GameplayBaseString + ".UseBoostMenu.InfoLabel.Text";
            public const string UseBoostMenu_UseBoostElement_AmountLabel_Text = GameplayBaseString + ".UseBoostMenu.UseBoostElement.AmountLabel.Text";
            public const string SkipMenu_Title = GameplayBaseString + ".SkipMenu.Title";
            public const string SkipMenu_Content = GameplayBaseString + ".SkipMenu.Content";

            private const string GameplayBaseString = "Gameplay";

            public class Tutorial
            {
                public const string Common_NextButton_Text = TutorialBaseString + ".Common.NextButton.Text";
                public const string Common_SkipButton_Text = TutorialBaseString + ".Common.SkipButton.Text";
                public const string Common_СompleteButton_Text = TutorialBaseString + ".Common.СompleteButton.Text";

                public const string ConfirmSkipDialogBox_Title = TutorialBaseString + ".ConfirmSkipDialogBox.Title";
                public const string ConfirmSkipDialogBox_Message = TutorialBaseString + ".ConfirmSkipDialogBox.Message";

                public const string Introductory_WelcomeDialogBox_Title = TutorialBaseString + ".Introductory.WelcomeDialogBox.Title";
                public const string Introductory_WelcomeDialogBox_Message = TutorialBaseString + ".Introductory.WelcomeDialogBox.Message";
                public const string Introductory_ConectionsDialogBox_Title = TutorialBaseString + ".Introductory.ConectionsDialogBox.Title";
                public const string Introductory_ConectionsDialogBox_Message = TutorialBaseString + ".Introductory.ConectionsDialogBox.Message";

                public const string RemoveConnections_MainDialogBox_Title = TutorialBaseString + ".RemoveConnections.MainDialogBox.Title";
                public const string RemoveConnections_MainDialogBox_Message = TutorialBaseString + ".RemoveConnections.MainDialogBox.Message";

                private const string TutorialBaseString = GameplayBaseString + ".Tutorial";
            }
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

            public const string LoadingBox_GJLogin_Title = DialogBoxBaseString + ".LoadingBox.GJLogin.Title";
            public const string LoadingBox_GJLogin_Message = DialogBoxBaseString + ".LoadingBox.GJLogin.Message";

            public const string InvalidLoginInputData_Title = DialogBoxBaseString + ".InvalidLoginInputData.Title";
            public const string InvalidLoginInputData_Message = DialogBoxBaseString + ".InvalidLoginInputData.Message";

            public const string LoginFailed_Title = DialogBoxBaseString + ".LoginFailed.Title";
            public const string LoginFailed_Message = DialogBoxBaseString + ".LoginFailed.Message";

            public const string ConfirmLogout_Title = DialogBoxBaseString + ".ConfirmLogout.Title";
            public const string ConfirmLogout_Message = DialogBoxBaseString + ".ConfirmLogout.Message";

            public const string AutoLoginInfo_Title = DialogBoxBaseString + ".AutoLoginInfo.Title";
            public const string AutoLoginInfo_Message = DialogBoxBaseString + ".AutoLoginInfo.Message";

            public const string ConfirmGameExit_Title = DialogBoxBaseString + ".ConfirmGameExit.Title";
            public const string ConfirmGameExit_Message = DialogBoxBaseString + ".ConfirmGameExit.Message";

            public const string GJScoreboardChange_Title = DialogBoxBaseString + ".GJScoreboardChange.Title";
            public const string GJScoreboardChange_Message = DialogBoxBaseString + ".GJScoreboardChange.Message";

            public const string SaveLoadError_Title = DialogBoxBaseString + ".SaveLoadError.Title";
            public const string SaveLoadError_Message = DialogBoxBaseString + ".SaveLoadError.Message";
            public const string SaveLoadError_DoNothingButton_Text = DialogBoxBaseString + ".SaveLoadError.DoNothingButton.Text";
            public const string SaveLoadError_EraseDataButton_Text = DialogBoxBaseString + ".SaveLoadError.EraseDataButton.Text";

            public const string ConfirmSaveErasing_Title = DialogBoxBaseString + ".ConfirmSaveErasing.Title";
            public const string ConfirmSaveErasing_Message = DialogBoxBaseString + ".ConfirmSaveErasing.Message";

            public const string DataSaveError_Title = DialogBoxBaseString + ".DataSaveError.Title";
            public const string DataSaveError_Message = DialogBoxBaseString + ".DataSaveError.Message";

            public const string Credits_Title = DialogBoxBaseString + ".Credits.Title";
            public const string Credits_Message = DialogBoxBaseString + ".Credits.Message";

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
            public const string Loading = CommonBaseString + ".Loading";
            public const string PleaseWait = CommonBaseString + ".PleaseWait";
            public const string Guest = CommonBaseString + ".Guest";
            public const string On = CommonBaseString + ".On";
            public const string Off = CommonBaseString + ".Off";

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

            public static class MainMenu
            {
                public const string CreditsButton_Text = MainMenuBaseString + ".CreditsButton.Text";

                private const string MainMenuBaseString = MenuBaseString + ".MainMenu";
            }

            public static class SelectLevelMenu
            {
                public const string Title = SelectLevelMenuBaseString + ".Title";

                public const string ChoosenLevelInfo_Title = SelectLevelMenuBaseString + ".ChoosenLevelInfo.Title";
                public const string ChoosenLevelInfo_Content_Completed = SelectLevelMenuBaseString + ".ChoosenLevelInfo.Content.Completed";
                public const string ChoosenLevelInfo_Content_Completed_WithBoosts = SelectLevelMenuBaseString + ".ChoosenLevelInfo.Content.Completed.WithBoosts";
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
                public const string MusicSlider_Text = SettingsMenuBaseString + ".MusicSlider.Text";
                public const string SoundsSlider_Text = SettingsMenuBaseString + ".SoundsSlider.Text";

                private const string SettingsMenuBaseString = MenuBaseString + ".SettingsMenu";
            }

            public static class StatsMenu
            {
                public const string Title = StatsMenuBaseString + ".Title";

                private const string StatsMenuBaseString = MenuBaseString + ".StatsMenu";
            }

            public static class GJMenu
            {
                private const string GJMenuBaseString = MenuBaseString + ".GJMenu";

                public static class GJLoginMenu
                {
                    public const string Title = GJLoginMenuBaseString + ".Title";
                    public const string GJInfoLabel_Text = GJLoginMenuBaseString + ".GJInfoLabel.Text";
                    public const string UsernameLabel_Text = GJLoginMenuBaseString + ".UsernameLabel.Text";
                    public const string TokenLabel_Text = GJLoginMenuBaseString + ".TokenLabel.Text";
                    public const string LoginButton_Text = GJLoginMenuBaseString + ".LoginButton.Text";
                    public const string AutoLoginToggle_Text = GJLoginMenuBaseString + ".AutoLoginToggle.Text";
                    public const string RegisterLabel_Text = GJLoginMenuBaseString + ".RegisterLabel.Text";

                    private const string GJLoginMenuBaseString = GJMenuBaseString + ".GJLoginMenu";
                }

                public static class GJProfileMenu
                {
                    public const string Title = GJProfileMenuBaseString + ".Title";
                    public const string ScoresTitleLabel_Text = GJProfileMenuBaseString + ".ScoresTitleLabel.Text";
                    public const string PlayerScoreDetailedInfo = GJProfileMenuBaseString + ".PlayerScoreDetailedInfo";
                    public const string PlayerScoreDetailedInfo_Title = GJProfileMenuBaseString + ".PlayerScoreDetailedInfo.Title";
                    public const string Scoreboard_Unknown = GJProfileMenuBaseString + ".Scoreboard.Unknown";
                    public const string ScoreboardNamePattern = GJProfileMenuBaseString + ".Scoreboard.{0}";
                    public const string TopPositionLabel_Text_HasScore = GJProfileMenuBaseString + ".TopPositionLabel.Text.HasScore";
                    public const string TopPositionLabel_Text_HasNoScore = GJProfileMenuBaseString + ".TopPositionLabel.Text.HasNoScore";

                    private const string GJProfileMenuBaseString = GJMenuBaseString + ".GJProfileMenu";
                }
            }

        }

        public static class StatsData
        {
            public const string ApplicationRunningTime_Name = StatsDataBaseString + ".ApplicationRunningTime.Name";
            public const string ApplicationRunningTime_Description = StatsDataBaseString + ".ApplicationRunningTime.Description";
            public const string ApplicationRunningTime_Value = StatsDataBaseString + ".ApplicationRunningTime.Value";

            public const string MovesCount_Name = StatsDataBaseString + ".MovesCount.Name";
            public const string MovesCount_Description = StatsDataBaseString + ".MovesCount.Description";
            public const string MovesCount_Value = StatsDataBaseString + ".MovesCount.Value";

            public const string TotalEarnedCoins_Name = StatsDataBaseString + ".TotalEarnedCoins.Name";
            public const string TotalEarnedCoins_Description = StatsDataBaseString + ".TotalEarnedCoins.Description";
            public const string TotalEarnedCoins_Value = StatsDataBaseString + ".TotalEarnedCoins.Value";

            public const string TotalReceivedItemsCount_Name = StatsDataBaseString + ".TotalReceivedItemsCount.Name";
            public const string TotalReceivedItemsCount_Description = StatsDataBaseString + ".TotalReceivedItemsCount.Description";
            public const string TotalReceivedItemsCount_Value = StatsDataBaseString + ".TotalReceivedItemsCount.Value";

            public const string FirstLaunchedVersion_Name = StatsDataBaseString + ".FirstLaunchedVersion.Name";
            public const string FirstLaunchedVersion_Description = StatsDataBaseString + ".FirstLaunchedVersion.Description";
            public const string FirstLaunchedVersion_Value = StatsDataBaseString + ".FirstLaunchedVersion.Value";

            public const string BoostsUsageCount_Name = StatsDataBaseString + ".BoostsUsageCount.Name";
            public const string BoostsUsageCount_Description = StatsDataBaseString + ".BoostsUsageCount.Description";
            public const string BoostsUsageCount_Value = StatsDataBaseString + ".BoostsUsageCount.Value";

            public const string PassedLevelsScoreSum_Name = StatsDataBaseString + ".PassedLevelsScoreSum.Name";
            public const string PassedLevelsScoreSum_Description = StatsDataBaseString + ".PassedLevelsScoreSum.Description";
            public const string PassedLevelsScoreSum_Value = StatsDataBaseString + ".PassedLevelsScoreSum.Value";

            public const string PassedWithoutBoostsLevelsScoreSum_Name = StatsDataBaseString + ".PassedWithoutBoostsLevelsScoreSum.Name";
            public const string PassedWithoutBoostsLevelsScoreSum_Description = StatsDataBaseString + ".PassedWithoutBoostsLevelsScoreSum.Description";
            public const string PassedWithoutBoostsLevelsScoreSum_Value = StatsDataBaseString + ".PassedWithoutBoostsLevelsScoreSum.Value";

            public const string PassedLevelsTimeSum_Name = StatsDataBaseString + ".PassedLevelsTimeSum.Name";
            public const string PassedLevelsTimeSum_Description = StatsDataBaseString + ".PassedLevelsTimeSum.Description";
            public const string PassedLevelsTimeSum_Value = StatsDataBaseString + ".PassedLevelsTimeSum.Value";

            public const string PassedWithoutBoostsLevelsTimeSum_Name = StatsDataBaseString + ".PassedWithoutBoostsLevelsTimeSum.Name";
            public const string PassedWithoutBoostsLevelsTimeSum_Description = StatsDataBaseString + ".PassedWithoutBoostsLevelsTimeSum.Description";
            public const string PassedWithoutBoostsLevelsTimeSum_Value = StatsDataBaseString + ".PassedWithoutBoostsLevelsTimeSum.Value";

            private const string StatsDataBaseString = "StatsData";
        }

        public static class Items
        {
            public const string Product_Boost_SkipLevel_Name = ItemsBaseString + ".Product.Boost.SkipLevel.Name";
            public const string Product_Boost_SkipLevel_Description = ItemsBaseString + ".Product.Boost.SkipLevel.Description";

            public const string Product_Boost_SimplifyLevel_Name = ItemsBaseString + ".Product.Boost.SimplifyLevel.Name";
            public const string Product_Boost_SimplifyLevel_Description = ItemsBaseString + ".Product.Boost.SimplifyLevel.Description";

            public const string Product_Boost_AllowIncompatibleConnections_Name = ItemsBaseString + ".Product.Boost.AllowIncompatibleConnections.Name";
            public const string Product_Boost_AllowIncompatibleConnections_Description = ItemsBaseString + ".Product.Boost.AllowIncompatibleConnections.Description";

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
