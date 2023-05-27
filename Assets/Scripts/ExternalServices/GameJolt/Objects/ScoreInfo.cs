using GameJolt.API.Objects;

namespace ConnectIt.ExternalServices.GameJolt.Objects
{
    public class ScoreInfo
    {
        public Score GJScore { get; }
        public int Rank { get; }
        public TableInfo ParentTable { get; }

        public ScoreInfo(Score dataSource, int rank, TableInfo parentTable)
        {
            GJScore = dataSource;
            Rank = rank;
            ParentTable = parentTable;
        }
    }
}
