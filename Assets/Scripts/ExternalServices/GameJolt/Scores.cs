using GameJolt.API.Objects;
using System;
using System.Collections.Generic;
using GJAPIScores = GameJolt.API.Scores;

namespace ConnectIt.ExternalServices.GameJolt
{
    public class Scores
    {
        public const int ScorePerUpdateLimit = 20;

        public event Action TablesChanged;
        public event Action<Table> TableScoresChanged;
        public event Action<Table> TablePlayerScoresChanged;
        public event Action<Table, Score, bool> PlayerScoreAppendAttempt;

        public IReadOnlyDictionary<Table, IReadOnlyList<Score>> ScoresInTables => (IReadOnlyDictionary<Table, IReadOnlyList<Score>>)_scoresInTables;
        public IReadOnlyDictionary<Table, IReadOnlyList<Score>> PlayerScoresInTables => (IReadOnlyDictionary<Table, IReadOnlyList<Score>>)_playerScoresInTables;

        private readonly Dictionary<Table, List<Score>> _scoresInTables = new();
        private readonly Dictionary<Table, List<Score>> _playerScoresInTables = new();

        public void AppendPlayerScore(Table table, Score toAppend, Action<bool> callback = null)
        {
            Action<bool> finalCallback = success => AppendPlayerScoreCallbackHandler(table, toAppend, success);
            if (finalCallback != null)
                finalCallback += callback;

            GJAPIScores.Add(toAppend, table.ID, finalCallback);
        }

        public void UpdateScoresForTable(Table table, bool onlyPlayerScores = false)
        {
            ClearScoresForTable(table, onlyPlayerScores);
            AddScoresForTable(table, onlyPlayerScores);
        }

        public void AddScoresForTable(Table table, bool onlyPlayerScores = false)
        {
            GJAPIScores.Get(scores => GetPlayerScoresCallbackHandler(table, scores), table.ID, ScorePerUpdateLimit, true);

            if (!onlyPlayerScores)
                GJAPIScores.Get(scores => GetScoresCallbackHandler(table, scores), table.ID, ScorePerUpdateLimit);
        }

        public void ClearScoresForTable(Table table, bool onlyPlayerScores = false)
        {
            _playerScoresInTables.Clear();

            TablePlayerScoresChanged.Invoke(table);

            if (!onlyPlayerScores)
            {
                _scoresInTables[table].Clear();

                TableScoresChanged?.Invoke(table);
            }
        }

        public void LoadTables()
        {
            GJAPIScores.GetTables(GetTablesCallbackHandler);
        }

        #region CallbackHandlers

        private void GetTablesCallbackHandler(Table[] tables)
        {
            _scoresInTables.Clear();
            _playerScoresInTables.Clear();

            foreach (Table table in tables)
            {
                _scoresInTables.Add(table, new List<Score>());
                _playerScoresInTables.Add(table, new List<Score>());
            }

            TablesChanged?.Invoke();
        }

        private void GetScoresCallbackHandler(Table table, Score[] scores)
        {
            _scoresInTables[table].AddRange(scores);

            TableScoresChanged?.Invoke(table);
        }

        private void GetPlayerScoresCallbackHandler(Table table, Score[] scores)
        {
            _playerScoresInTables[table].AddRange(scores);

            TablePlayerScoresChanged?.Invoke(table);
        }

        private void AppendPlayerScoreCallbackHandler(Table table, Score toAppend, bool success)
        {
            PlayerScoreAppendAttempt?.Invoke(table, toAppend, success);
        }

        #endregion
    }
}
