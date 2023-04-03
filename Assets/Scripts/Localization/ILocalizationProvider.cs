using System;

namespace ConnectIt.Localization
{
    public interface ILocalizationProvider
    {
        public enum SupporedLanguages
        {
            English = 1,
            Russian
        }

        const string DefaultLocalizationPath = "Localization";

        SupporedLanguages[] AllSupporedLanguages { get; }
        SupporedLanguages Language { get; set; }

        event Action LocalizationChanged;

        void AutoLanguage();
        bool HasKey(string localizationKey);
        bool HasKeyInLanguage(SupporedLanguages language, string localizationKey);
        string Localize(string localizationKey);
        string LocalizeFormat(string localizationKey, params object[] args);
        string LocalizeToLanguage(SupporedLanguages language, string localizationKey);
        string LocalizeToLanguageFormat(SupporedLanguages language, string localizationKey, params object[] args);
        void Read(string path = DefaultLocalizationPath);
    }
}