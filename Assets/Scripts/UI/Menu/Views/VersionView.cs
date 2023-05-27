using ConnectIt.Config;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views
{
    public class VersionView : DefaultLocalizedTextElementView
    {
        private readonly GameVersion _gameVersion;
        private readonly TextKey.Factory _textKeyFactory;

        public VersionView(Label versionLabel,
            GameVersion gameVersion,
            ILocalizationProvider localizationProvider,
            TextKey.Factory textKeyFactory) : base(versionLabel, null, localizationProvider)
        {
            _gameVersion = gameVersion;
            _textKeyFactory = textKeyFactory;
        }

        public override void Initialize()
        {
            textKey = _textKeyFactory.Create(
                TextKeysConstants.Menu.VersionLabel_Text,
                new object[] { _gameVersion.GetVersion() });

            base.Initialize();
        }

        public new class Factory : PlaceholderFactory<Label, VersionView> { }
    }
}
