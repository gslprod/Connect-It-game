using ConnectIt.Gameplay.Data;
using System;
using UnityEngine;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    [Serializable]
    public class GameplaySaveData : IEquatable<GameplaySaveData>
    {
        public const string SaveKey = "Gameplay";

        [SerializeField] internal LevelPassSaveData[] LevelPasses;

        public GameplaySaveData()
        {
            LevelPasses = null;
        }

        public GameplaySaveData Clone()
        {
            GameplaySaveData clonedObject = (GameplaySaveData)MemberwiseClone();

            if (LevelPasses != null)
            {
                clonedObject.LevelPasses = new LevelPassSaveData[LevelPasses.Length];
                for (int i = 0; i < LevelPasses.Length; i++)
                    clonedObject.LevelPasses[i] = LevelPasses[i].Clone();
            }

            return clonedObject;
        }

        public bool Equals(GameplaySaveData other)
        {
            if (LevelPasses == null != (other.LevelPasses == null))
                return false;

            if (LevelPasses != null)
            {
                if (LevelPasses.Length != other.LevelPasses.Length)
                    return false;

                for (int i = 0; i < LevelPasses.Length; i++)
                {
                    if (!LevelPasses[i].Equals(other.LevelPasses[i]))
                        return false;
                }
            }

            return true;
        }

        [Serializable]
        public class LevelPassSaveData : IEquatable<LevelPassSaveData>
        {
            [SerializeField] internal int Id;
            [SerializeField] internal PassStates PassState;
            [SerializeField] internal long Score;
            [SerializeField] internal long TotalEarnedCoins;
            [SerializeField] internal float PassTimeSec;
            [SerializeField] internal float PassLevelProgress;

            public bool Equals(LevelPassSaveData other)
                => Id == other.Id &&
                PassState == other.PassState &&
                Score == other.Score &&
                TotalEarnedCoins == other.TotalEarnedCoins &&
                PassTimeSec == other.PassTimeSec &&
                PassLevelProgress == other.PassLevelProgress;

            public LevelPassSaveData Clone()
                => (LevelPassSaveData)MemberwiseClone();
        }
    }
}
