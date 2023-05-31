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
        [SerializeField] internal float OSTVolumePercents;

        public SettingsSaveData()
        {
            Language = SupportedLanguages.None;
            OSTVolumePercents = 50f;
        }

        public SettingsSaveData Clone()
            => (SettingsSaveData)MemberwiseClone();

        public bool Equals(SettingsSaveData other)
            => Language == other.Language &&
            OSTVolumePercents == other.OSTVolumePercents;
    }
}
