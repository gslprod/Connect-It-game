using ConnectIt.ExternalServices.GameJolt.Objects;
using ConnectIt.Gameplay.Data;
using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
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

        private long _totalScore;

        public TotalScoresFixator(
            GameJoltAPIProvider gjApiProvider,
            ILevelsPassDataProvider levelsPassDataProvider,
            IExternalServerSaveProvider externalServerSaveProvider)
        {
            _gjApiProvider = gjApiProvider;
            _levelsPassDataProvider = levelsPassDataProvider;
            _externalServerSaveProvider = externalServerSaveProvider;
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

            _totalScore = _levelsPassDataProvider.LevelDataArray.Sum(data => data.Score);

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
                return;

            FixScoreInTable(table);
        }

        private void FixScoreInTable(TableInfo table)
        {
            int intScore = (int)Mathf.Clamp(_totalScore, int.MinValue, int.MaxValue);
            Score score = new(
                intScore,
                intScore.ToString(),
                "",
                "");

            _gjApiProvider.AppendPlayerScore(table, score, success => OnScoreFixAttempt(success, intScore));
        }

        private void OnScoreFixAttempt(bool success, int intScore)
        {
            if (!success)
                return;

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

            _externalServerSaveProvider.SaveExtrenalServerData(saveData);
        }
    }
}
