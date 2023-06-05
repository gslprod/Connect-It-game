using ConnectIt.Config;
using ConnectIt.ExternalServices.GameJolt.Objects;
using ConnectIt.Gameplay.Data;
using ConnectIt.Localization;
using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Customer.Storage;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Stats;
using ConnectIt.Stats.Data;
using GameJolt.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ConnectIt.ExternalServices.GameJolt.Fixators.Scores
{
    public class TotalScoresFixator : IInitializable, IDisposable
    {
        private readonly GameJoltAPIProvider _gjApiProvider;
        private readonly ILevelsPassDataProvider _levelsPassDataProvider;
        private readonly IExternalServerSaveProvider _externalServerSaveProvider;
        private readonly GameVersion _version;
        private readonly ICustomer _playerCustomer;
        private readonly IStatsCenter _statsCenter;
        private readonly ISettingsSaveProvider _settingsSaveProvider;

        private long _totalScore;

        public TotalScoresFixator(
            GameJoltAPIProvider gjApiProvider,
            ILevelsPassDataProvider levelsPassDataProvider,
            IExternalServerSaveProvider externalServerSaveProvider,
            GameVersion version,
            ICustomer playerCustomer,
            IStatsCenter statsCenter,
            ISettingsSaveProvider settingsSaveProvider)
        {
            _gjApiProvider = gjApiProvider;
            _levelsPassDataProvider = levelsPassDataProvider;
            _externalServerSaveProvider = externalServerSaveProvider;
            _version = version;
            _playerCustomer = playerCustomer;
            _statsCenter = statsCenter;
            _settingsSaveProvider = settingsSaveProvider;
        }

        public void Initialize()
        {
            _levelsPassDataProvider.LevelDataChanged += OnLevelDataChanged;
            _gjApiProvider.UserLogInAttempt += OnUserLogInAttempt;
        }

        public void Dispose()
        {
            _levelsPassDataProvider.LevelDataChanged -= OnLevelDataChanged;
            _gjApiProvider.UserLogInAttempt -= OnUserLogInAttempt;
        }

        private void OnLevelDataChanged()
        {
            TryFixScore();
        }

        private void OnUserLogInAttempt(bool success)
        {
            TryFixScore();
        }

        private void TryFixScore()
        {
            if (!_gjApiProvider.UserExistsAndLoggedIn)
                return;

            _totalScore = _levelsPassDataProvider.LevelDataArray
                .Sum(item => item.FullyPassedWithoutBoosts ? item.Score : 0);

            ExternalServerSaveData saveData = _externalServerSaveProvider.LoadExternalServerData();
            int? index = saveData.FixedScores?.FindIndex(item => item.TableID == GJConstants.TableID.TotalScores);

            if (index.HasValue &&
                index.Value >= 0 &&
                saveData.FixedScores[index.Value].BestFixedScore >= _totalScore)

                return;

            TableInfo tableInfo = _gjApiProvider.Tables.First(item => item.GJTable.ID == GJConstants.TableID.TotalScores);
            if (tableInfo.PlayerScoresWasUpdatedAtLeastOnce)
            {
                TryFixScoreInTable(tableInfo);
            }
            else
            {
                _gjApiProvider.TablePlayerScoresChanged += ScoreboardCheckCallbackHandler;
                _gjApiProvider.UpdateScoresForTable(tableInfo, true);
            }
        }

        private void ScoreboardCheckCallbackHandler(TableInfo table)
        {
            if (table.GJTable.ID != GJConstants.TableID.TotalScores)
                return;

            _gjApiProvider.TablePlayerScoresChanged -= ScoreboardCheckCallbackHandler;

            TryFixScoreInTable(table);
        }

        private void TryFixScoreInTable(TableInfo table)
        {
            IReadOnlyList<ScoreInfo> playerScores = _gjApiProvider.PlayerScoresInTables[table.GJTable.ID];
            if (playerScores.Count == 0)
            {
                FixScoreInTable(table);
                return;
            }

            ScoreInfo bestPlayerScore = playerScores[0];
            if (bestPlayerScore.GJScore.Value >= _totalScore)
            {
                SaveFixedScore(bestPlayerScore.GJScore.Value);
                return;
            }

            FixScoreInTable(table);
        }

        private void FixScoreInTable(TableInfo table)
        {
            int intScore = (int)Mathf.Clamp(_totalScore, int.MinValue, int.MaxValue);
            if (intScore == 0)
                return;

            Score score = new(
                intScore,
                intScore.ToString(),
                "",
                Uri.EscapeDataString(CollectExtraData()));

            _gjApiProvider.AppendPlayerScore(table, score, success => OnScoreFixAttempt(success, intScore));
        }

        private string CollectExtraData()
        {
            string version = _version.GetVersion();

            IEnumerable<LevelData> levelDataArray = _levelsPassDataProvider.LevelDataArray;
            int totalLevels = levelDataArray.Count();
            int passedLevels = levelDataArray.Count(item => item.Passed);
            int passedWithBoosts = levelDataArray.Count(item => item.Passed && item.BoostsUsed);
            int fullyPassedWithoutBoostsLevels = levelDataArray.Count(item => item.FullyPassedWithoutBoosts);
            int skippedLevels = levelDataArray.Count(item => item.Skipped);
            int notCompletedLevels = levelDataArray.Count(item => item.NotCompleted);
            float passTimeSumSec = levelDataArray.Sum(item => item.PassTimeSec);
            TimeSpan passTimeSum = TimeSpan.FromSeconds(passTimeSumSec);
            long allScoresSum = levelDataArray.Sum(item => item.Score);

            ExternalServerSaveData externalServerSaveData = _externalServerSaveProvider.LoadExternalServerData();
            int bestSavedFixedScore = externalServerSaveData.FixedScores.First(item => item.TableID == GJConstants.TableID.TotalScores).BestFixedScore;

            long coins = _playerCustomer.Wallet.Coins;
            IStorage playerStorage = _playerCustomer.Storage;
            int storageItemsTotal = playerStorage.Items.Count();
            int skipLevelBoostsCount = playerStorage.GetProductCountOfType<SkipLevelBoost>();
            int simplifyLevelBoostsCount = playerStorage.GetProductCountOfType<SimplifyLevelBoost>();
            int allowIncompatibleConnectionsBoostsCount = playerStorage.GetProductCountOfType<AllowIncompatibleConnectionsBoost>();

            string firstVersion = _statsCenter.GetData<FirstLaunchedVersionStatsData>().RawValue;
            double runningTimeSec = _statsCenter.GetData<ApplicationRunningTimeStatsData>().RawValue;
            TimeSpan runningTime = TimeSpan.FromSeconds(runningTimeSec);
            long movesCount = _statsCenter.GetData<MovesCountStatsData>().RawValue;
            long totalEarnedCoins = _statsCenter.GetData<TotalEarnedCoinsStatsData>().RawValue;
            long totalReceivedItemsCount = _statsCenter.GetData<TotalReceivedItemsCountStatsData>().RawValue;
            long boostsUsageCount = _statsCenter.GetData<BoostsUsageCountStatsData>().RawValue;

            SettingsSaveData settingsSaveData = _settingsSaveProvider.LoadSettingsData();
            SupportedLanguages language = settingsSaveData.Language;
            float ostVolume = settingsSaveData.OSTVolumePercents;
            float soundsVolume = settingsSaveData.SoundsVolumePercents;

            return
                $"Version: {version}\n" +
                $"Best saved score: {bestSavedFixedScore}\n" +
                $"Levels:\n" +
                $"Total: {totalLevels} | Passed: {passedLevels} | Passed with boosts: {passedWithBoosts} | " +
                $"Fully passed without boosts: {fullyPassedWithoutBoostsLevels} | " +
                $"Skipped: {skippedLevels} | Not completed: {notCompletedLevels} | All scores sum: {allScoresSum} | " +
                $"Pass time sum: {passTimeSum:hh\\:mm\\:ss\\.fff}\n" +
                $"Coins: {coins}\n" +
                $"Storage items:\n" +
                $"Total: {storageItemsTotal}\n" +
                $"Boosts:\n" +
                $"Skip level: {skipLevelBoostsCount} | Simplify level: {simplifyLevelBoostsCount} | " +
                $"Allow incompatible connections: {allowIncompatibleConnectionsBoostsCount}\n" +
                $"Stats:\n" +
                $"First version: {firstVersion} | Running time: {runningTime:hh\\:mm\\:ss\\.fff} | Moves count: {movesCount} | " +
                $"Total earned coins: {totalEarnedCoins} | Total received items count: {totalReceivedItemsCount} | " +
                $"Boosts usage count: {boostsUsageCount}\n" +
                $"Settings:\n" +
                $"Language: {language} | OST: {ostVolume} | Sounds: {soundsVolume}";
        }
        
        private void OnScoreFixAttempt(bool success, int intScore)
        {
            if (!success)
                return;

            SaveFixedScore(intScore);
        }

        private void SaveFixedScore(int intScore)
        {
            ExternalServerSaveData saveData = _externalServerSaveProvider.LoadExternalServerData();
            saveData.FixedScores ??= new();
            int index = saveData.FixedScores.FindIndex(item => item.TableID == GJConstants.TableID.TotalScores);

            if (index >= 0)
                saveData.FixedScores[index].BestFixedScore = intScore;
            else
                saveData.FixedScores.Add(new()
                {
                    TableID = GJConstants.TableID.TotalScores,
                    BestFixedScore = intScore
                });

            _externalServerSaveProvider.SaveExternalServerData(saveData);
        }
    }
}
