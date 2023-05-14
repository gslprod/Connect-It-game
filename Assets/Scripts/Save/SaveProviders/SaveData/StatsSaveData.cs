using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    [Serializable]
    public class StatsSaveData : IEquatable<StatsSaveData>
    {
        public const string SaveKey = "Stats";

        [SerializeField] internal StatsElementSaveData<double>[] DoubleStats;
        [SerializeField] internal StatsElementSaveData<long>[] LongStats;
        [SerializeField] internal StatsElementSaveData<string>[] StringStats;

        public StatsSaveData()
        {
            DoubleStats = null;
            LongStats = null;
            StringStats = null;
        }

        public StatsSaveData Clone()
        {
            StatsSaveData clonedObject = (StatsSaveData)MemberwiseClone();

            if (DoubleStats != null)
            {
                clonedObject.DoubleStats = new StatsElementSaveData<double>[DoubleStats.Length];
                for (int i = 0; i < DoubleStats.Length; i++)
                    clonedObject.DoubleStats[i] = DoubleStats[i].Clone();
            }

            if (LongStats != null)
            {
                clonedObject.LongStats = new StatsElementSaveData<long>[LongStats.Length];
                for (int i = 0; i < LongStats.Length; i++)
                    clonedObject.LongStats[i] = LongStats[i].Clone();
            }

            if (StringStats != null)
            {
                clonedObject.StringStats = new StatsElementSaveData<string>[StringStats.Length];
                for (int i = 0; i < StringStats.Length; i++)
                    clonedObject.StringStats[i] = StringStats[i].Clone();
            }

            return clonedObject;
        }

        public bool Equals(StatsSaveData other)
        {
            if (DoubleStats == null != (other.DoubleStats == null))
                return false;

            if (DoubleStats != null)
            {
                if (DoubleStats.Length != other.DoubleStats.Length)
                    return false;

                for (int i = 0; i < DoubleStats.Length; i++)
                {
                    if (!DoubleStats[i].Equals(other.DoubleStats[i]))
                        return false;
                }
            }

            if (LongStats == null != (other.LongStats == null))
                return false;

            if (LongStats != null)
            {
                if (LongStats.Length != other.LongStats.Length)
                    return false;

                for (int i = 0; i < LongStats.Length; i++)
                {
                    if (!LongStats[i].Equals(other.LongStats[i]))
                        return false;
                }
            }

            if (StringStats == null != (other.StringStats == null))
                return false;

            if (StringStats != null)
            {
                if (StringStats.Length != other.StringStats.Length)
                    return false;

                for (int i = 0; i < StringStats.Length; i++)
                {
                    if (!StringStats[i].Equals(other.StringStats[i]))
                        return false;
                }
            }

            return true;
        }

        [Serializable]
        public class StatsElementSaveData<T> : IEquatable<StatsElementSaveData<T>>
        {
            [SerializeField] internal string StatsElementSaveName;
            [SerializeField] internal T Value;

            public bool Equals(StatsElementSaveData<T> other)
                =>
                StatsElementSaveName == other.StatsElementSaveName &&
                EqualityComparer<T>.Default.Equals(Value, other.Value);

            public StatsElementSaveData<T> Clone()
                => (StatsElementSaveData<T>)MemberwiseClone();
        }
    }
}
