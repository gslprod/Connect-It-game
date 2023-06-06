using ConnectIt.Localization;

namespace ConnectIt.Gameplay.TutorialsSystem.Wrappers
{
    public class DialogBoxTutorialData
    {
        public Frame[] Frames { get; }
        public bool Skippable { get; }

        public DialogBoxTutorialData(
            Frame[] frames,
            bool skippable = true)
        {
            Frames = frames;
            Skippable = skippable;
        }

        public class Frame
        {
            public TextKey Title { get; }
            public TextKey Message { get; }

            public Frame(TextKey title, TextKey message)
            {
                Title = title;
                Message = message;
            }
        }
    }
}
