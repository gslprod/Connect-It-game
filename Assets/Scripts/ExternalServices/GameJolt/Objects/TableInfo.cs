using GameJolt.API.Objects;
using System;

namespace ConnectIt.ExternalServices.GameJolt.Objects
{
    public class TableInfo : IEquatable<TableInfo>
    {
        public Table GJTable { get; }
        public DateTime LastUpdated { get; set; }
        public bool WasUpdatedAtLeastOnce => LastUpdated != default;

        public TableInfo(Table table)
        {
            GJTable = table;
        }

        public bool Equals(TableInfo other)
            => GJTable.Equals(other.GJTable);
    }
}
