﻿using ConnectIt.Audio.Sounds;
using ConnectIt.Config;
using ConnectIt.Gameplay.Data;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.Utilities;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.SelectLevelMenu
{
    public class SelectLevelButtonView : DefaultButtonView
    {
        private readonly int _level;
        private readonly ILevelsPassDataProvider _levelsPassDataProvider;

        private string _activeClass;

        public SelectLevelButtonView(Button button,
            int level,
            Action<int> onClick,
            ILevelsPassDataProvider levelsPassDataProvider,
            SoundsPlayer soundsPlayer,
            AudioConfig audioConfig) : base(button, () => onClick(level), soundsPlayer, audioConfig)
        {
            _level = level;
            _levelsPassDataProvider = levelsPassDataProvider;
        }

        public override void Initialize()
        {
            base.Initialize();

            UpdateColor();
            _levelsPassDataProvider.LevelDataChanged += UpdateColor;
        }

        public override void Dispose()
        {
            base.Dispose();

            _levelsPassDataProvider.LevelDataChanged -= UpdateColor;
        }

        private void UpdateColor()
        {
            LevelData levelData = _levelsPassDataProvider.GetDataByLevelId(_level);

            bool levelBlocked = false;
            if (_levelsPassDataProvider.TryGetDataByLevelId(_level - 1, out LevelData previousLevelData))
                levelBlocked = previousLevelData.NotCompleted;

            string newClass;
            if (levelBlocked)
                newClass = null;
            else
                newClass = levelData.PassState switch
                {
                    PassStates.Passed =>
                        levelData.BoostsUsed ?
                        ClassNamesConstants.MenuView.LevelButtonCompletedWithBoosts :
                        ClassNamesConstants.MenuView.LevelButtonCompleted,

                    PassStates.NotCompleted => ClassNamesConstants.MenuView.LevelButtonCurrent,
                    PassStates.Skipped => ClassNamesConstants.MenuView.LevelButtonSkipped,

                    _ => throw Assert.GetFailException(),
                };

            if (newClass == _activeClass)
                return;

            if (_activeClass != null)
                button.RemoveFromClassList(_activeClass);

            if (newClass != null)
                button.AddToClassList(newClass);
        }

        public new class Factory : PlaceholderFactory<Button, int, Action<int>, SelectLevelButtonView> { }
    }
}
