namespace ConnectIt.Gameplay.Data
{
    public enum PassStates
    {
        NotCompleted = 0,
        Passed,
        Skipped
    }

    public struct LevelData
    {
        public bool Passed => PassState == PassStates.Passed;
        public bool NotCompleted => PassState == PassStates.NotCompleted;

        public int Id;
        public PassStates PassState;
        public long Score;
        public long TotalEarnedCoins;
        public float PassTimeSec;

        public LevelData(int id)
        {
            Id = id;
            PassState = PassStates.NotCompleted;
            Score = 0;
            TotalEarnedCoins = 0;
            PassTimeSec = 0f;
        }
    }
}
