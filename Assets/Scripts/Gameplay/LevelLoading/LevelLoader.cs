using ConnectIt.Config;
using ConnectIt.Gameplay.MonoWrappers;
using ConnectIt.Utilities;
using System.Linq;
using Zenject;

namespace ConnectIt.Gameplay.LevelLoading
{
    public class LevelLoader
    {
        private readonly TilemapsMonoWrapper[] _tilemapsMonoWrappersPrefabs;
        private readonly TilemapsMonoWrapper.Factory _tilemapsMonoWrapperFactory;
        private readonly GameplayLogicConfig _gameplayLogicConfig;

        public LevelLoader(
            TilemapsMonoWrapper[] tilemapsMonoWrappersPrefabs,
            TilemapsMonoWrapper.Factory tilemapsMonoWrapperFactory,
            GameplayLogicConfig gameplayLogicConfig)
        {
            _tilemapsMonoWrappersPrefabs = tilemapsMonoWrappersPrefabs;
            _tilemapsMonoWrapperFactory = tilemapsMonoWrapperFactory;
            _gameplayLogicConfig = gameplayLogicConfig;
        }

        public TilemapsMonoWrapper InstantiateTilemapsPrefab()
        {
            ValidateWrappersPrefabs();

            int currentLevel = _gameplayLogicConfig.CurrentLevel;

            TilemapsMonoWrapper toCreate = _tilemapsMonoWrappersPrefabs.First(wrapperPrefab => wrapperPrefab.TargetLevel == currentLevel);

            return _tilemapsMonoWrapperFactory.Create(toCreate);
        }

        private void ValidateWrappersPrefabs()
        {
            Assert.IsNotNull(_tilemapsMonoWrappersPrefabs);

            bool allTargetLevelsValid = _tilemapsMonoWrappersPrefabs.All(
                wrapperPrefab =>
                IsTagetLevelValid(wrapperPrefab.TargetLevel));

            Assert.That(allTargetLevelsValid);

            int groupsWithDuplicateLevelsCount =
                _tilemapsMonoWrappersPrefabs.GroupBy(wrapper => wrapper.TargetLevel)
                .Count(group => group.Count() > 1);

            Assert.That(groupsWithDuplicateLevelsCount == 0);
        }

        private bool IsTagetLevelValid(int level)
        {
            return
                level >= 1 &&
                level <= _gameplayLogicConfig.MaxAvailableLevel;
        }
    }
}
