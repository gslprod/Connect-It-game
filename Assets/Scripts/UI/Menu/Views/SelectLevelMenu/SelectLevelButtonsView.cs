using ConnectIt.Config;
using ConnectIt.Gameplay.Data;
using ConnectIt.Localization;
using ConnectIt.Scenes.Switchers;
using ConnectIt.UI.DialogBox;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Formatters;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.SelectLevelMenu
{
    public class SelectLevelButtonsView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly GameplayLogicConfig _gameplayLogicConfig;
        private readonly DialogBoxView.Factory _dialogBoxFactory;
        private readonly ISceneSwitcher _sceneSwitcher;
        private readonly ILevelsPassDataProvider _levelsPassDataProvider;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly IFormatter _formatter;
        private readonly SelectLevelButtonView.Factory _selectLevelButtonViewFactory;

        private Dictionary<int, SelectLevelButtonView> _levelButtons = new();

        public SelectLevelButtonsView(VisualElement viewRoot,
            VisualElement mainRoot,
            GameplayLogicConfig gameplayLogicConfig,
            DialogBoxView.Factory dialogBoxViewFactory,
            ISceneSwitcher sceneSwitcher,
            ILevelsPassDataProvider levelsPassDataProvider,
            TextKey.Factory textKeyFactory,
            IFormatter formatter,
            SelectLevelButtonView.Factory selectLevelButtonViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _gameplayLogicConfig = gameplayLogicConfig;
            _dialogBoxFactory = dialogBoxViewFactory;
            _sceneSwitcher = sceneSwitcher;
            _levelsPassDataProvider = levelsPassDataProvider;
            _textKeyFactory = textKeyFactory;
            _formatter = formatter;
            _selectLevelButtonViewFactory = selectLevelButtonViewFactory;
        }

        public void Initialize()
        {
            CreateViews();
        }

        public void Dispose()
        {
            DisposeDisposableViews();
        }

        private void CreateViews()
        {
            int levelsCount = _gameplayLogicConfig.MaxAvailableLevel;

            for (int i = 1; i <= levelsCount; i++)
            {
                SelectLevelButtonView buttonView = _selectLevelButtonViewFactory.Create(
                    _viewRoot.Q<Button>(string.Format(NameConstants.SelectLevelMenu.LevelButtonFormat, i)),
                    i,
                    OnSelectLevelButtonClick);

                _levelButtons.Add(i, buttonView);
            }
        }

        private void DisposeDisposableViews()
        {
            foreach (KeyValuePair<int, SelectLevelButtonView> pair in _levelButtons)
                pair.Value.Dispose();
        }

        #region SelectLevelButton

        private void OnSelectLevelButtonClick(int level)
        {
            LevelData levelData = _levelsPassDataProvider.GetDataByLevelId(level);

            bool levelBlocked = false;
            if (_levelsPassDataProvider.TryGetDataByLevelId(level - 1, out LevelData previousLevelData))
                levelBlocked = previousLevelData.NotCompleted;

            CreateDialogBoxByLevelData(levelData, levelBlocked);
        }

        private void CreateDialogBoxByLevelData(LevelData levelData, bool levelBlocked)
        {
            DialogBoxButtonInfo cancelButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Common.Cancel, null),
                null,
                DialogBoxButtonType.Default,
                true);

            DialogBoxButtonInfo[] buttonsInfo;
            if (levelBlocked)
            {
                buttonsInfo = new DialogBoxButtonInfo[]
                {
                    cancelButtonInfo
                };
            }
            else
            {
                DialogBoxButtonInfo playButtonInfo = new(
                    _textKeyFactory.Create(TextKeysConstants.Common.Play, null),
                    () => OnStartLevelButtonClick(levelData.Id),
                    DialogBoxButtonType.Accept);

                buttonsInfo = new DialogBoxButtonInfo[]
                {
                    playButtonInfo, cancelButtonInfo
                };
            }

            DialogBoxCreationData creationData = new(
                _mainRoot,
                _textKeyFactory.Create(TextKeysConstants.Menu.SelectLevelMenu.ChoosenLevelInfo_Title, new object[] { levelData.Id }),
                GetMessageTextKeyByLevelData(levelData, levelBlocked),
                buttonsInfo);

            _dialogBoxFactory.Create(creationData);
        }

        private TextKey GetMessageTextKeyByLevelData(LevelData levelData, bool levelBlocked)
        {
            string stringTextKey;
            if (levelBlocked)
                stringTextKey = TextKeysConstants.Menu.SelectLevelMenu.ChoosenLevelInfo_Content_NotAvailable;
            else
                stringTextKey = levelData.PassState switch
                {
                    PassStates.NotCompleted => TextKeysConstants.Menu.SelectLevelMenu.ChoosenLevelInfo_Content_NotCompleted,
                    PassStates.Passed => TextKeysConstants.Menu.SelectLevelMenu.ChoosenLevelInfo_Content_Completed,
                    PassStates.Skipped => TextKeysConstants.Menu.SelectLevelMenu.ChoosenLevelInfo_Content_Skipped,

                    _ => throw Assert.GetFailException(),
                };

            object[] textKeyArgs;
            if (levelBlocked)
                textKeyArgs = null;
            else
                textKeyArgs = levelData.PassState switch
                {
                    PassStates.Passed => new object[]
                    {
                        _formatter.FormatDetailedGameplayElapsedTime(TimeSpan.FromSeconds(levelData.PassTimeSec)),
                        levelData.Score
                    },

                    _ => null,
                };

            return _textKeyFactory.Create(stringTextKey, textKeyArgs);
        }

        private void OnStartLevelButtonClick(int level)
        {
            _gameplayLogicConfig.SetCurrentLevel(level);
            _sceneSwitcher.TryGoToScene(Scenes.SceneType.GameScene);
        }

        #endregion

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, SelectLevelButtonsView> { }
    }
}
