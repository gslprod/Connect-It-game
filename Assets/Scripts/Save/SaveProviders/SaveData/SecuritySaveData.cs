using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    public class SecuritySaveData : IEquatable<SecuritySaveData>
    {
        public const string SaveKey = "Security";

        [SerializeField] internal string GeneratedEncryptionKey;
        [SerializeField] internal List<SaveDataStateSaveData> SaveDataStates;

        public SecuritySaveData()
        {
            GeneratedEncryptionKey = null;
            SaveDataStates = null;
        }

        public SecuritySaveData Clone()
        {
            SecuritySaveData clonedObject = (SecuritySaveData)MemberwiseClone();

            if (SaveDataStates != null)
            {
                clonedObject.SaveDataStates = new();
                clonedObject.SaveDataStates.AddRange(SaveDataStates);
            }

            return clonedObject;
        }

        public bool Equals(SecuritySaveData other)
        {
            if (SaveDataStates == null != (other.SaveDataStates == null))
                return false;

            if (SaveDataStates != null)
            {
                if (SaveDataStates.Count != other.SaveDataStates.Count)
                    return false;

                for (int i = 0; i < SaveDataStates.Count; i++)
                {
                    if (!SaveDataStates[i].Equals(other.SaveDataStates[i]))
                        return false;
                }
            }

            return
                GeneratedEncryptionKey == other.GeneratedEncryptionKey;
        }

        [Serializable]
        public class SaveDataStateSaveData : IEquatable<SaveDataStateSaveData>
        {
            [SerializeField] internal string SaveDataSaveName;
            [SerializeField] internal bool WasCreated;

            public bool Equals(SaveDataStateSaveData other)
                => SaveDataSaveName == other.SaveDataSaveName &&
                WasCreated == other.WasCreated;

            public SaveDataStateSaveData Clone()
                => (SaveDataStateSaveData)MemberwiseClone();
        }
    }
}
