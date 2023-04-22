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
            => false;

        [Serializable]
        public class LevelPassSaveData : IEquatable<LevelPassSaveData>
        {
            [SerializeField] internal int Id;
            [SerializeField] internal bool Passed;
            [SerializeField] internal long Score;
            [SerializeField] internal float PassTimeSec;

            public bool Equals(LevelPassSaveData other)
                => Id == other.Id &&
                Passed == other.Passed &&
                Score == other.Score &&
                PassTimeSec == other.PassTimeSec;

            public LevelPassSaveData Clone()
                => (LevelPassSaveData)MemberwiseClone();
        }
    }
}
