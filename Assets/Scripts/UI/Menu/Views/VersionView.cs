using ConnectIt.Config;
using ConnectIt.Localization;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views
{
    public class VersionView : IInitializable, IDisposable
    {
        private readonly Label _versionLabel;
        private readonly GameVersion _gameVersion;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly TextKey.Factory _textKeyFactory;

        private TextKey _versionTextKey;

        public VersionView(Label versionLabel,
            GameVersion gameVersion,
            ILocalizationProvider localizationProvider,
            TextKey.Factory textKeyFactory)
        {
            _versionLabel = versionLabel;
            _gameVersion = gameVersion;
            _localizationProvider = localizationProvider;
            _textKeyFactory = textKeyFactory;
        }

        public void Initialize()
        {
            _versionTextKey = _textKeyFactory.Create(
                TextKeysConstants.Menu.VersionLabel_Title,
                new object[] { _gameVersion.GetVersion() });

            _localizationProvider.LocalizationChanged += OnLocalizationChanged;

            UpdateView();
        }

        public void Dispose()
        {
            _localizationProvider.LocalizationChanged -= OnLocalizationChanged;
        }

        private void OnLocalizationChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _versionLabel.text = _versionTextKey.ToString();
        }

        public class Factory : PlaceholderFactory<Label, VersionView> { }
    }
}
