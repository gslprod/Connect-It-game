using ConnectIt.Config;
using ConnectIt.Gameplay.Data;
using ConnectIt.Gameplay.TutorialsSystem.Wrappers;
using ConnectIt.Localization;
using ConnectIt.Utilities.Extensions;
using Zenject;

namespace ConnectIt.Gameplay.TutorialsSystem.Tutorials
{
    public class IntroductoryTutorial : IInitializable
    {
        private readonly GameplayLogicConfig _gameplayLogicConfig;
        private readonly ILevelsPassDataProvider _levelsPassDataProvider;
        private readonly ITutorialsStarter _tutorialsStarter;
        private readonly TextKey.Factory _textKeyFactory;

        public IntroductoryTutorial(
            GameplayLogicConfig gameplayLogicConfig,
            ILevelsPassDataProvider levelsPassDataProvider,
            ITutorialsStarter tutorialsStarter,
            TextKey.Factory textKeyFactory)
        {
            _gameplayLogicConfig = gameplayLogicConfig;
            _levelsPassDataProvider = levelsPassDataProvider;
            _tutorialsStarter = tutorialsStarter;
            _textKeyFactory = textKeyFactory;
        }

        public void Initialize()
        {
            if (ShouldBeStarted())
                Start();
        }

        private bool ShouldBeStarted()
        {
            return
                _gameplayLogicConfig.CurrentLevel == 1 &&
                _levelsPassDataProvider.GetDataByLevelId(1).NotCompleted;
        }

        private void Start()
        {
            DialogBoxTutorialData.Frame[] tutorialFrames = new DialogBoxTutorialData.Frame[]
            {
                new(_textKeyFactory.Create(TextKeysConstants.Gameplay.Tutorial.Introductory_WelcomeDialogBox_Title),
                    _textKeyFactory.Create(TextKeysConstants.Gameplay.Tutorial.Introductory_WelcomeDialogBox_Message)),
                new(_textKeyFactory.Create(TextKeysConstants.Gameplay.Tutorial.Introductory_ConectionsDialogBox_Title),
                    _textKeyFactory.Create(TextKeysConstants.Gameplay.Tutorial.Introductory_ConectionsDialogBox_Message))
            };

            DialogBoxTutorialData tutorialData = new(tutorialFrames);

            _tutorialsStarter.StartDialogBoxTutorial(tutorialData);
        }
    }
}
