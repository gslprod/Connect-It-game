using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    [Serializable]
    public class ExternalServerSaveData : IEquatable<ExternalServerSaveData>
    {
        public const string SaveKey = "ExternalServer";

        [SerializeField] internal string Username;
        [SerializeField] internal string Token;
        [SerializeField] internal List<FixedScoreSaveData> FixedScores;

        public ExternalServerSaveData()
        {
            Username = null;
            Token = null;
            FixedScores = null;
        }

        public ExternalServerSaveData Clone()
        {
            ExternalServerSaveData clonedObject = (ExternalServerSaveData)MemberwiseClone();

            if (FixedScores != null)
            {
                clonedObject.FixedScores = new();
                clonedObject.FixedScores.AddRange(FixedScores);
            }

            return clonedObject;
        }

        public bool Equals(ExternalServerSaveData other)
        {
            if (FixedScores == null != (other.FixedScores == null))
                return false;

            if (FixedScores != null)
            {
                if (FixedScores.Count != other.FixedScores.Count)
                    return false;

                for (int i = 0; i < FixedScores.Count; i++)
                {
                    if (!FixedScores[i].Equals(other.FixedScores[i]))
                        return false;
                }
            }

            return
                Username == other.Username &&
                Token == other.Username;
        }

        [Serializable]
        public class FixedScoreSaveData : IEquatable<FixedScoreSaveData>
        {
            [SerializeField] internal int TableID;
            [SerializeField] internal int BestFixedScore;

            public bool Equals(FixedScoreSaveData other)
                => TableID == other.TableID &&
                BestFixedScore == other.BestFixedScore;

            public FixedScoreSaveData Clone()
                => (FixedScoreSaveData)MemberwiseClone();
        }
    }
}
