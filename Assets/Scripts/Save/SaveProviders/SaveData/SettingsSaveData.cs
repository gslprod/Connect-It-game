using ConnectIt.Localization;
using System;
using UnityEngine;

namespace ConnectIt.Save.SaveProviders.SaveData
{
    [Serializable]
    public class SettingsSaveData : IEquatable<SettingsSaveData>
    {
        public const string SaveKey = "Settings";

        [SerializeField] internal SupportedLanguages Language;

        public SettingsSaveData()
        {
            Language = SupportedLanguages.None;
        }

        public SettingsSaveData Clone()
            => (SettingsSaveData)MemberwiseClone();

        public bool Equals(SettingsSaveData other)
            => Language == other.Language;
    }
}
