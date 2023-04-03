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

        public SupporedLanguages[] AllSupporedLanguages { get; private set; }
        public SupporedLanguages Language
        {
            get => _language;
            set
            {
                if (_language == value)
                    return;

                _language = value;
                LocalizationChanged?.Invoke();
            }
        }

        private SupporedLanguages _language;
        private readonly Dictionary<SupporedLanguages, Dictionary<string, string>> _dictionary = new();

        public void Initialize()
        {
            Read();
            AutoLanguage();
        }

        public void AutoLanguage()
        {
            Language = Application.systemLanguage switch
            {
                SystemLanguage.English => SupporedLanguages.English,
                SystemLanguage.Russian => SupporedLanguages.Russian,

                _ => SupporedLanguages.English,
            };
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

        public bool HasKeyInLanguage(SupporedLanguages language, string localizationKey)
        {
            return _dictionary[language].ContainsKey(localizationKey);
        }

        public string Localize(string localizationKey)
            => LocalizeToLanguage(Language, localizationKey);

        public string LocalizeToLanguage(SupporedLanguages language, string localizationKey)
        {
            if (_dictionary.Count == 0)
                throw new InvalidOperationException("Localization provider has not been initialized");

            if (!_dictionary.ContainsKey(Language))
                throw new KeyNotFoundException("Language not found: " + Language);

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

        public string LocalizeToLanguageFormat(SupporedLanguages language, string localizationKey, params object[] args)
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
            AllSupporedLanguages = _dictionary.Keys.ToArray();
        }

        private SupporedLanguages StringToEnum(string stringLanguage)
        {
            return Enum.Parse<SupporedLanguages>(stringLanguage);
        }
    }
}