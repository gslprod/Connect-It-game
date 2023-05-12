using System;
using System.Collections.Generic;

namespace ConnectIt.Localization
{
    public interface ILocalizationProvider
    {
        const string DefaultLocalizationPath = "Localization";

        public IEnumerable<SupportedLanguages> AllSupporedLanguages { get; }
        SupportedLanguages Language { get; set; }

        event Action LocalizationChanged;

        void AutoLanguage();
        bool HasKey(string localizationKey);
        bool HasKeyInLanguage(SupportedLanguages language, string localizationKey);
        string Localize(string localizationKey);
        string LocalizeFormat(string localizationKey, params object[] args);
        string LocalizeToLanguage(SupportedLanguages language, string localizationKey);
        string LocalizeToLanguageFormat(SupportedLanguages language, string localizationKey, params object[] args);
        void Read(string path = DefaultLocalizationPath);
    }
}