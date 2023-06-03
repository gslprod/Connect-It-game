using ConnectIt.Config.ScriptableObjects;
using ConnectIt.Config.Wrappers;
using ConnectIt.Utilities;

namespace ConnectIt.Config
{
    public class GameplayLogicConfig
    {
        public float RemoveConnectionLineHoldDurationSec => _configSO.RemoveConnectionLineHoldDurationSec;
        public float UsedTilesVsConnectedPortsGameCompleteFactor => _configSO.UsedTilesVsConnectedPortsGameCompleteFactor;
        public int MaxAvailableLevel => _configSO.MaxAvailableLevel;
        public int CurrentLevel => _currentLevel;
        public LevelRewardData[] LevelsRewardData => _configSO.LevelRewardData;

        //todo
        private int _currentLevel = 10;

        private readonly GameplayLogicConfigSO _configSO;

        public GameplayLogicConfig(GameplayLogicConfigSO configSO)
        {
            _configSO = configSO;
        }

        public void SetCurrentLevel(int levelNumber)
        {
            Assert.ThatArgIs(levelNumber >= 1,
                levelNumber <= MaxAvailableLevel);

            _currentLevel = levelNumber;
        }
    }
}
