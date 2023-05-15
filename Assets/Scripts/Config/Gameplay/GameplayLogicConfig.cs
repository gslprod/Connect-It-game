using ConnectIt.Config.ScriptableObjects;
using ConnectIt.Config.Wrappers;
using ConnectIt.Utilities;

namespace ConnectIt.Config
{
    public class GameplayLogicConfig
    {
        public float RemoveConnectionLineHoldDurationSec => _removeConnectionLineHoldDurationSec;
        public float UsedTilesVsConnectedPortsGameCompleteFactor => _usedTilesVsConnectedPortsGameCompleteFactor;
        public int MaxAvailableLevel => _maxAvailableLevel;
        public int CurrentLevel => _currentLevel;
        public LevelRewardData[] LevelsRewardData => _levelsRewardData;

        private float _removeConnectionLineHoldDurationSec;
        private float _usedTilesVsConnectedPortsGameCompleteFactor;
        private int _maxAvailableLevel;
        private LevelRewardData[] _levelsRewardData;

        //todo
        private int _currentLevel = 2;

        private readonly GameplayLogicConfigSO _configSO;

        public GameplayLogicConfig(GameplayLogicConfigSO configSO)
        {
            _configSO = configSO;

            SetValuesFromSO();
        }

        public void SetCurrentLevel(int levelNumber)
        {
            Assert.ThatArgIs(levelNumber >= 1,
                levelNumber <= _maxAvailableLevel);

            _currentLevel = levelNumber;
        }

        private void SetValuesFromSO()
        {
            _removeConnectionLineHoldDurationSec = _configSO.RemoveConnectionLineHoldDurationSec;
            _usedTilesVsConnectedPortsGameCompleteFactor = _configSO.UsedTilesVsConnectedPortsGameCompleteFactor;
            _maxAvailableLevel = _configSO.MaxAvailableLevel;
            _levelsRewardData = _configSO.LevelRewardData;
        }
    }
}
