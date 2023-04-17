using ConnectIt.Config.ScriptableObjects;
using ConnectIt.Utilities.Formatters;
using UnityEngine;

namespace ConnectIt.Config
{
    public class GameVersion
    {
        private string _additionalVersionInfo => _config.AdditionalVersionInfo;

        private readonly IFormatter _formatter;
        private readonly GameVersionSO _config;

        public GameVersion(IFormatter formatter, GameVersionSO gameVersionConfig)
        {
            _formatter = formatter;
            _config = gameVersionConfig;
        }

        public string GetVersion()
        {
            return _formatter.FormatVersion(Application.version, _additionalVersionInfo).Trim();
        }
    }
}
