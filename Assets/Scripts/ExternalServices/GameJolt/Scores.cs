using ConnectIt.ExternalServices.GameJolt.Objects;
using ConnectIt.Utilities.Extensions;
using GameJolt.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GJAPIScores = GameJolt.API.Scores;
using GJAPI = GameJolt.API.GameJoltAPI;

namespace ConnectIt.ExternalServices.GameJolt
{
    public class Scores
    {
        public const int ScorePerUpdateLimit = 20;

        public event Action TablesChanged;
        public event Action<TableInfo> TableScoresChanged;
        public event Action<TableInfo> TablePlayerScoresChanged;
        public event Action<TableInfo, Score, bool> PlayerScoreAppendAttempt;

        public IReadOnlyList<TableInfo> Tables => _tables;
        public IReadOnlyDictionary<int, IReadOnlyList<ScoreInfo>> ScoresInTables => _scoresInTables;
        public IReadOnlyDictionary<int, IReadOnlyList<ScoreInfo>> PlayerScoresInTables => _playerScoresInTables;

        private readonly List<TableInfo> _tables = new();
        private readonly Dictionary<int, IReadOnlyList<ScoreInfo>> _scoresInTables = new();
        private readonly Dictionary<int, IReadOnlyList<ScoreInfo>> _playerScoresInTables = new();
        private readonly Dictionary<int, List<Score>> _playerScoresInTablesWithoutRank = new();

        private bool _updatingRank;
        private bool _gettingScore;
        private bool _gettingTables;

        public void AppendPlayerScore(TableInfo table, Score toAppend, Action<bool> callback = null)
        {
            Action<bool> finalCallback = success => AppendPlayerScoreCallbackHandler(table, toAppend, success);
            if (finalCallback != null)
                finalCallback += callback;

            GJAPIScores.Add(toAppend, table.GJTable.ID, finalCallback);
        }

        public void UpdateScoresForTable(TableInfo table, bool onlyPlayerScores = false)
        {
            ClearScoresForTable(table, onlyPlayerScores);
            AddScoresForTable(table, onlyPlayerScores);
        }

        public void AddScoresForTable(TableInfo table, bool onlyPlayerScores = false)
        {
            if (_gettingScore)
                return;

            _gettingScore = true;

            GJAPIScores.Get(scores => GetPlayerScoresCallbackHandler(table, scores), table.GJTable.ID, ScorePerUpdateLimit, true);

            if (!onlyPlayerScores)
                GJAPIScores.Get(scores => GetScoresCallbackHandler(table, scores), table.GJTable.ID, ScorePerUpdateLimit);
        }

        public void ClearScoresForTable(TableInfo table, bool onlyPlayerScores = false)
        {
            ((List<ScoreInfo>)_playerScoresInTables[table.GJTable.ID]).Clear();

            TablePlayerScoresChanged?.Invoke(table);

            if (!onlyPlayerScores)
            {
                ((List<ScoreInfo>)_scoresInTables[table.GJTable.ID]).Clear();

                TableScoresChanged?.Invoke(table);
            }
        }

        public void LoadTables()
        {
            if (_gettingTables)
                return;

            _gettingTables = true;

            GJAPIScores.GetTables(GetTablesCallbackHandler);
        }

        private void UpdatePlayerScoresRanks()
        {
            if (_updatingRank)
                return;

            _updatingRank = true;

            UpdatePlayerScoresRanksInternal();
        }

        private void UpdatePlayerScoresRanksInternal()
        {
            if (_playerScoresInTablesWithoutRank.Count == 0)
            {
                _updatingRank = false;
                return;
            }

            int tableID = _playerScoresInTablesWithoutRank.ElementAt(0).Key;
            Score score = _playerScoresInTablesWithoutRank.ElementAt(0).Value[0];
            TableInfo table = GetTableWithID(tableID);

            int index = _scoresInTables[tableID].FindIndex(item => item.GJScore.UserID == GJAPI.Instance.CurrentUser.ID);
            if (index >= 0)
            {
                GetRankCallbackHandler(table, score, _scoresInTables[tableID][index].Rank);
                return;
            }

            GJAPIScores.GetRank(
                score.Value,
                table.GJTable.ID,
                (rank) => GetRankCallbackHandler(table, score, rank));
        }

        private TableInfo GetTableWithID(int id)
            => _tables.First(item => item.GJTable.ID == id);

        #region CallbackHandlers

        private void GetTablesCallbackHandler(Table[] tables)
        {
            _gettingTables = false;

            if (!GJAPI.Instance.HasSignedInUser)
                return;

            _tables.Clear();
            _scoresInTables.Clear();
            _playerScoresInTables.Clear();

            foreach (Table table in tables)
            {
                _tables.Add(new TableInfo(table));
                _scoresInTables.Add(table.ID, new List<ScoreInfo>());
                _playerScoresInTables.Add(table.ID, new List<ScoreInfo>());
            }

            TablesChanged?.Invoke();
        }

        private void GetScoresCallbackHandler(TableInfo table, Score[] scores)
        {
            _gettingScore = false;

            if (!GJAPI.Instance.HasSignedInUser)
                return;

            table.LastUpdated = DateTime.Now;

            for (int i = 0; i < scores.Length; i++)
                ((List<ScoreInfo>)_scoresInTables[table.GJTable.ID]).Add(new(scores[i], i + 1, table));

            TableScoresChanged?.Invoke(table);
        }

        private void GetPlayerScoresCallbackHandler(TableInfo table, Score[] scores)
        {
            _gettingScore = false;

            if (!GJAPI.Instance.HasSignedInUser)
                return;

            table.PlayerScoresLastUpdated = DateTime.Now;

            if (scores.Length == 0)
            {
                TablePlayerScoresChanged?.Invoke(table);
                return;
            }

            if (_playerScoresInTablesWithoutRank.ContainsKey(table.GJTable.ID))
                _playerScoresInTablesWithoutRank[table.GJTable.ID].AddRange(scores);
            else
                _playerScoresInTablesWithoutRank.Add(table.GJTable.ID, new List<Score>(scores));

            UpdatePlayerScoresRanks();
        }

        private void AppendPlayerScoreCallbackHandler(TableInfo table, Score toAppend, bool success)
        {
            if (!GJAPI.Instance.HasSignedInUser)
                return;

            if (success)
                UpdateScoresForTable(table, true);

            PlayerScoreAppendAttempt?.Invoke(table, toAppend, success);
        }

        private void GetRankCallbackHandler(TableInfo table, Score score, int rank)
        {
            if (!GJAPI.Instance.HasSignedInUser)
                return;

            _playerScoresInTablesWithoutRank[table.GJTable.ID].Remove(score);
            if (_playerScoresInTablesWithoutRank[table.GJTable.ID].Count == 0)
                _playerScoresInTablesWithoutRank.Remove(table.GJTable.ID);

            ((List<ScoreInfo>)_playerScoresInTables[table.GJTable.ID]).Add(new(score, rank, table));

            TablePlayerScoresChanged?.Invoke(table);
            UpdatePlayerScoresRanksInternal();
        }

        #endregion
    }
}
