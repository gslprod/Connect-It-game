using ConnectIt.Gameplay.Pause;
using ConnectIt.Input;
using ConnectIt.Localization;
using ConnectIt.Scenes;
using ConnectIt.Scenes.Switchers;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Gameplay.Views;
using ConnectIt.UI.Gameplay.Views.UseBoostMenu;
using ConnectIt.Utilities.Extensions;
using ConnectIt.Utilities.Extensions.GameplayInputRouter;
using ConnectIt.Utilities.Extensions.IPauseService;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Custom = ConnectIt.UI.CustomControls;

namespace ConnectIt.UI.Gameplay.MonoWrappers
{
    public class GameplayUIDocumentMonoWrapper : MonoBehaviour
    {
        public VisualElement Root => _rootVE;

        private UIDocument _uiDocument;
        private VisualElement _documentRootVE => _uiDocument.rootVisualElement;
        private VisualElement _rootVE;

        private DialogBoxView.Factory _dialogBoxFactory;
        private CustomDialogBoxView.Factory _customDialogBoxFactory;
        private TextKey.Factory _textKeyFactory;
        private GameplayInputRouter _gameplayInputRouter;
        private IPauseService _pauseService;
        private ISceneSwitcher _sceneSwitcher;
        private VisualTreeAsset _useBoostMenuAsset;

        private LevelProgressView.Factory _levelProgressViewFactory;
        private LevelProgressView _levelProgressView;

        private TimeView.Factory _timeViewFactory;
        private TimeView _timeView;

        private LevelView.Factory _levelViewFactory;
        private LevelView _levelView;

        private DefaultButtonView.Factory _defaultButtonViewFactory;
        private DefaultButtonView _pauseButtonView;
        private DefaultButtonView _restartButtonView;

        private CoinsView.Factory _coinsViewFactory;
        private CoinsView _coinsView;

        private DefaultLocalizedButtonView.Factory _defaultLocalizedButtonViewFactory;
        private DefaultLocalizedButtonView _useBoostButtonView;

        private UseBoostMenuView.Factory _useBoostMenuViewFactory;
        private Dictionary<IDialogBoxView, UseBoostMenuView> _useBoostMenuViews = new();

        [Inject]
        public void Constructor(
            LevelProgressView.Factory levelProgressViewFactory,
            TimeView.Factory timeViewFactory,
            LevelView.Factory levelViewFactory,
            DefaultButtonView.Factory defaultButtonViewFactory,
            CoinsView.Factory coinsViewFactory,
            DialogBoxView.Factory dialogBoxFactory,
            TextKey.Factory textKeyFactory,
            GameplayInputRouter gameplayInputRouter,
            IPauseService pauseService,
            ISceneSwitcher sceneSwitcher,
            DefaultLocalizedButtonView.Factory defaultLocalizedButtonViewFactory,
            CustomDialogBoxView.Factory customDialogBoxFactory,
            VisualTreeAsset useBoostMenuAsset,
            UseBoostMenuView.Factory useBoostMenuViewFactory)
        {
            _levelProgressViewFactory = levelProgressViewFactory;
            _timeViewFactory = timeViewFactory;
            _levelViewFactory = levelViewFactory;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _coinsViewFactory = coinsViewFactory;
            _dialogBoxFactory = dialogBoxFactory;
            _textKeyFactory = textKeyFactory;
            _gameplayInputRouter = gameplayInputRouter;
            _pauseService = pauseService;
            _sceneSwitcher = sceneSwitcher;
            _defaultLocalizedButtonViewFactory = defaultLocalizedButtonViewFactory;
            _customDialogBoxFactory = customDialogBoxFactory;
            _useBoostMenuAsset = useBoostMenuAsset;
            _useBoostMenuViewFactory = useBoostMenuViewFactory;
        }

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _rootVE = _documentRootVE.Q<VisualElement>(NameConstants.RootName);
        }

        private void Start()
        {
            CreateViews();
        }

        private void Update()
        {
            SendTickToTickableViews();
        }

        private void OnDestroy()
        {
            DestroyDisposableViews();
        }

        private void CreateViews()
        {
            _levelProgressView = _levelProgressViewFactory.Create(
                _documentRootVE.Q<Custom.ProgressBar>(NameConstants.LevelProgressBarName));

            _timeView = _timeViewFactory.Create(
                _documentRootVE.Q<Label>(NameConstants.TimeLabelName));

            _levelView = _levelViewFactory.Create(
                _documentRootVE.Q<Label>(NameConstants.LevelLabelName));

            _coinsView = _coinsViewFactory.Create(
                _documentRootVE.Q<Label>(NameConstants.CoinsLabelName));

            _pauseButtonView = _defaultButtonViewFactory.Create(
                _documentRootVE.Q<Button>(NameConstants.PauseButtonName),
                OnPauseButtonClick);

            _restartButtonView = _defaultButtonViewFactory.Create(
                _documentRootVE.Q<Button>(NameConstants.RestartButtonName),
                RestartButtonClick);

            _useBoostButtonView = _defaultLocalizedButtonViewFactory.Create(
                _documentRootVE.Q<Button>(NameConstants.UseBoostButtonName),
                OnUseBoostButtonClick,
                _textKeyFactory.Create(TextKeysConstants.Gameplay.UseBoostButton_Text));
        }

        private void SendTickToTickableViews()
        {
            _timeView.Tick();
        }

        private void DestroyDisposableViews()
        {
            _levelProgressView.Dispose();
            _levelView.Dispose();
            _pauseButtonView.Dispose();
            _restartButtonView.Dispose();
            _coinsView.Dispose();
            _useBoostButtonView.Dispose();
        }

        #region RestartButton

        private void RestartButtonClick()
        {
            DialogBoxView dialogBox = _dialogBoxFactory.CreateDefaultRestartLevelDialogBox(_textKeyFactory, _rootVE, _sceneSwitcher);

            dialogBox.Showing += OnRestartDialogBoxShowing;
            dialogBox.Closing += OnRestartDialogBoxClosing;

            dialogBox.Show();
        }

        private void OnRestartDialogBoxShowing(IDialogBoxView dialogBox)
        {
            dialogBox.Showing -= OnRestartDialogBoxShowing;

            _gameplayInputRouter.SetEnable(false, GameplayInputRouterEnablePriority.DialogBox, dialogBox);
        }

        private void OnRestartDialogBoxClosing(IDialogBoxView dialogBox)
        {
            dialogBox.Closing -= OnRestartDialogBoxClosing;

            _gameplayInputRouter.ResetEnable(dialogBox);
        }

        #endregion

        #region ExitButton

        private void OnPauseButtonClick()
        {
            DialogBoxButtonInfo continueButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Gameplay.PauseMenu_Continue),
                null,
                DialogBoxButtonType.Default,
                true);

            DialogBoxButtonInfo exitButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Gameplay.PauseMenu_Exit),
                OnExitButtonClick,
                DialogBoxButtonType.Dismiss);

            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[]
            {
                continueButtonInfo, exitButtonInfo
            };

            DialogBoxCreationData creationData = new(
                _rootVE,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LevelPaused_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LevelPaused_Message),
                buttonsInfo,
                null,
                false);

            DialogBoxView dialogBox = _dialogBoxFactory.Create(creationData);

            dialogBox.Showing += OnPauseDialogBoxShowing;
            dialogBox.Closing += OnPauseDialogBoxClosing;

            dialogBox.Show();
        }

        private void OnPauseDialogBoxShowing(IDialogBoxView dialogBox)
        {
            dialogBox.Showing -= OnRestartDialogBoxShowing;

            _pauseService.SetPause(true, PauseEnablePriority.PauseMenu, dialogBox);
        }

        private void OnPauseDialogBoxClosing(IDialogBoxView dialogBox)
        {
            dialogBox.Closing -= OnRestartDialogBoxClosing;

            _pauseService.ResetPause(dialogBox);
        }

        private void OnExitButtonClick()
        {
            DialogBoxButtonInfo cancelButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Common.Cancel),
                null,
                DialogBoxButtonType.Default,
                true);

            DialogBoxButtonInfo exitButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Common.Confirm),
                OnConfirmExitButtonClick,
                DialogBoxButtonType.Dismiss);

            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[]
            {
                cancelButtonInfo, exitButtonInfo
            };

            DialogBoxCreationData creationData = new(
                _rootVE,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.QuitLevelConfirm_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.QuitLevelConfirm_Message),
                buttonsInfo);

            _dialogBoxFactory.Create(creationData);
        }

        private void OnConfirmExitButtonClick()
        {
            _sceneSwitcher.TryGoToScene(SceneType.MenuScene);
        }

        #endregion

        #region UseBoostButton

        private void OnUseBoostButtonClick()
        {
            CustomDialogBoxView useBoostDialogBox = _customDialogBoxFactory.CreateDefaultCancelButtonDialogBox(
                _rootVE,
                _useBoostMenuAsset,
                _textKeyFactory.Create(TextKeysConstants.Gameplay.UseBoostMenu_Title),
                _textKeyFactory.Create(TextKeysConstants.Common.Cancel),
                false);

            useBoostDialogBox.Showing += OnUseBoostDialogBoxShowing;
            useBoostDialogBox.Closing += OnUseBoostDialogBoxClosing;
            useBoostDialogBox.Disposing += OnUseBoostDialogBoxDisposing;

            useBoostDialogBox.Show();
        }

        private void OnUseBoostDialogBoxShowing(IDialogBoxView dialogBox)
        {
            dialogBox.Showing -= OnUseBoostDialogBoxShowing;

            _gameplayInputRouter.SetEnable(false, GameplayInputRouterEnablePriority.DialogBox, dialogBox);

            CustomDialogBoxView customDialogBox = (CustomDialogBoxView)dialogBox;

            UseBoostMenuView useBoostMenu = _useBoostMenuViewFactory.Create(customDialogBox.ContentRoot, _rootVE);
            _useBoostMenuViews.Add(dialogBox, useBoostMenu);
        }

        private void OnUseBoostDialogBoxClosing(IDialogBoxView dialogBox)
        {
            dialogBox.Closing -= OnUseBoostDialogBoxClosing;

            _gameplayInputRouter.ResetEnable(dialogBox);
        }

        private void OnUseBoostDialogBoxDisposing(IDialogBoxView dialogBox)
        {
            dialogBox.Disposing -= OnUseBoostDialogBoxDisposing;

            _useBoostMenuViews[dialogBox].Dispose();
            _useBoostMenuViews.Remove(dialogBox);
        }

        #endregion
    }
}