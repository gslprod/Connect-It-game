using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Zenject;
using static ConnectIt.Localization.ILocalizationProvider;

namespace ConnectIt.Localization
{
    public class LocalizationProvider : ILocalizationProvider, IInitializable
    {
        public event Action LocalizationChanged;

        public IEnumerable<SupportedLanguages> AllSupporedLanguages => _allSupporedLanguages;
        public SupportedLanguages Language
        {
            get => _language;
            set
            {
                Assert.ThatArgIs(value != SupportedLanguages.None);

                if (_language == value)
                    return;

                _language = value;

                SaveLanguage();
                LocalizationChanged?.Invoke();
            }
        }

        private SupportedLanguages[] _allSupporedLanguages;
        private SupportedLanguages _language;
        private readonly Dictionary<SupportedLanguages, Dictionary<string, string>> _dictionary = new();
        private readonly ISettingsSaveProvider _settingsSaveProvider;

        public LocalizationProvider(
            ISettingsSaveProvider settingsSaveProvider)
        {
            _settingsSaveProvider = settingsSaveProvider;
        }

        public void Initialize()
        {
            Read();

            if (TryLoadLanguage())
                return;

            AutoLanguage();
        }

        public void AutoLanguage()
        {
            Language = Enum.TryParse(Application.systemLanguage.ToString(), out SupportedLanguages result) ? result : SupportedLanguages.English;
        }

        public void Read(string path = DefaultLocalizationPath)
        {
            if (_dictionary.Count > 0)
                return;

            var textAssets = Resources.LoadAll<TextAsset>(path);

            foreach (var textAsset in textAssets)
            {
                var text = ReplaceMarkers(textAsset.text).Replace("\"\"", "[quotes]");
                var matches = Regex.Matches(text, "\"[\\s\\S]+?\"");

                foreach (Match match in matches)
                {
                    text = text.Replace(match.Value, match.Value.Replace("\"", null).Replace(",", "[comma]").Replace("\n", "[newline]"));
                }

                var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                var languages = lines[0].Split(',').Select(i => i.Trim()).ToList();

                for (var i = 1; i < languages.Count; i++)
                {
                    var enumLanguage = StringToEnum(languages[i]);

                    if (!_dictionary.ContainsKey(enumLanguage))
                    {
                        _dictionary.Add(enumLanguage, new Dictionary<string, string>());
                    }
                }

                for (var i = 1; i < lines.Length; i++)
                {
                    var columns = lines[i].Split(',').Select(j => j.Trim()).Select(j => j.Replace("[comma]", ",").Replace("[newline]", "\n").Replace("[quotes]", "\"")).ToList();
                    var key = columns[0];

                    if (key == "")
                        continue;

                    for (var j = 1; j < languages.Count; j++)
                    {
                        var enumLanguage = StringToEnum(languages[j]);

                        _dictionary[enumLanguage].Add(key, columns[j]);
                    }
                }
            }

            UpdateSupportedLanguages();
        }

        public bool HasKey(string localizationKey)
            => HasKeyInLanguage(Language, localizationKey);

        public bool HasKeyInLanguage(SupportedLanguages language, string localizationKey)
        {
            return _dictionary[language].ContainsKey(localizationKey);
        }

        public string Localize(string localizationKey)
            => LocalizeToLanguage(Language, localizationKey);

        public string LocalizeToLanguage(SupportedLanguages language, string localizationKey)
        {
            if (_dictionary.Count == 0)
                throw new InvalidOperationException("Localization provider has not been initialized");

            if (!_dictionary.ContainsKey(Language))
                throw new KeyNotFoundException("Language not found: " + Language);

            //todo
            //if (string.IsNullOrEmpty(localizationKey))
            //    throw new ArgumentNullException(nameof(localizationKey));
            if (string.IsNullOrEmpty(localizationKey))
                return "#NullKey";

            //if (!Dictionary[Language].ContainsKey(localizationKey)) throw new KeyNotFoundException("Translation not found: " + localizationKey);
            if (!_dictionary[language].ContainsKey(localizationKey))
                return "#NoKeyFound";

            var missed = !_dictionary[language].ContainsKey(localizationKey) || string.IsNullOrEmpty(_dictionary[language][localizationKey]);

            if (missed)
            {
                Debug.LogWarningFormat("Translation not found: {0} ({1}).", localizationKey, language.ToString());

                return localizationKey;
            }

            return _dictionary[language][localizationKey];
        }

        public string LocalizeFormat(string localizationKey, params object[] args)
        {
            var pattern = Localize(localizationKey);

            return string.Format(pattern, args);
        }

        public string LocalizeToLanguageFormat(SupportedLanguages language, string localizationKey, params object[] args)
        {
            var pattern = LocalizeToLanguage(language, localizationKey);

            return string.Format(pattern, args);
        }

        private string ReplaceMarkers(string text)
        {
            return text.Replace("[Newline]", "\n");
        }

        private void UpdateSupportedLanguages()
        {
            _allSupporedLanguages = _dictionary.Keys.ToArray();
        }

        private SupportedLanguages StringToEnum(string stringLanguage)
        {
            return Enum.Parse<SupportedLanguages>(stringLanguage);
        }

        private bool TryLoadLanguage()
        {
            SettingsSaveData saveData = _settingsSaveProvider.LoadSettingsData();
            if (saveData.Language == SupportedLanguages.None)
                return false;

            _language = saveData.Language;
            return true;
        }

        private void SaveLanguage()
        {
            SettingsSaveData saveData = _settingsSaveProvider.LoadSettingsData();
            saveData.Language = Language;
            _settingsSaveProvider.SaveSettingsData(saveData);
        }
    }
}