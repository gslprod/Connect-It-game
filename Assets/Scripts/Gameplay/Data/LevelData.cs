namespace ConnectIt.Gameplay.Data
{
    public struct LevelData
    {
        public int Id;
        public bool Passed;
        public long Score;
        public float PassTimeSec;

        public LevelData(int id)
        {
            Id = id;
            Passed = false;
            Score = 0;
            PassTimeSec = 0f;
        }
    }
}
