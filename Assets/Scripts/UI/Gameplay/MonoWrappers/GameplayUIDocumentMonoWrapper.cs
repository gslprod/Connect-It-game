using ConnectIt.Gameplay.Pause;
using ConnectIt.Input;
using ConnectIt.Localization;
using ConnectIt.Scenes;
using ConnectIt.Scenes.Switchers;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.UI.Gameplay.Views;
using ConnectIt.Utilities.Extensions;
using ConnectIt.Utilities.Extensions.GameplayInputRouter;
using ConnectIt.Utilities.Extensions.IPauseService;
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
        private TextKey.Factory _textKeyFactory;
        private GameplayInputRouter _gameplayInputRouter;
        private IPauseService _pauseService;
        private ISceneSwitcher _sceneSwitcher;

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
            ISceneSwitcher sceneSwitcher)
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
        }

        #region RestartButton

        private void RestartButtonClick()
        {
            DialogBoxView dialogBox = _dialogBoxFactory.CreateDefaultRestartLevelDialogBox(_textKeyFactory, _rootVE, _sceneSwitcher);

            dialogBox.Showing += OnRestartDialogBoxShowing;
            dialogBox.Closing += OnRestartDialogBoxClosing;

            dialogBox.Show();
        }

        private void OnRestartDialogBoxShowing(DialogBoxView dialogBox)
        {
            dialogBox.Showing -= OnRestartDialogBoxShowing;

            _gameplayInputRouter.SetEnable(false, GameplayInputRouterEnablePriority.DialogBox, dialogBox);
        }

        private void OnRestartDialogBoxClosing(DialogBoxView dialogBox)
        {
            dialogBox.Closing -= OnRestartDialogBoxClosing;

            _gameplayInputRouter.ResetEnable(dialogBox);
        }

        #endregion

        #region ExitButton

        private void OnPauseButtonClick()
        {
            DialogBoxButtonInfo continueButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Gameplay.PauseMenu_Continue, null),
                null,
                DialogBoxButtonType.Default,
                true);

            DialogBoxButtonInfo exitButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Gameplay.PauseMenu_Exit, null),
                OnExitButtonClick,
                DialogBoxButtonType.Dismiss);

            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[]
            {
                continueButtonInfo, exitButtonInfo
            };

            DialogBoxCreationData creationData = new(
                _rootVE,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LevelPaused_Title, null),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.LevelPaused_Message, null),
                buttonsInfo,
                null,
                false);

            DialogBoxView dialogBox = _dialogBoxFactory.Create(creationData);

            dialogBox.Showing += OnPauseDialogBoxShowing;
            dialogBox.Closing += OnPauseDialogBoxClosing;

            dialogBox.Show();
        }

        private void OnPauseDialogBoxShowing(DialogBoxView dialogBox)
        {
            dialogBox.Showing -= OnRestartDialogBoxShowing;

            _pauseService.SetPause(true, PauseEnablePriority.PauseMenu, dialogBox);
        }

        private void OnPauseDialogBoxClosing(DialogBoxView dialogBox)
        {
            dialogBox.Closing -= OnRestartDialogBoxClosing;

            _pauseService.ResetPause(dialogBox);
        }

        private void OnExitButtonClick()
        {
            DialogBoxButtonInfo cancelButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Common.Cancel, null),
                null,
                DialogBoxButtonType.Default,
                true);

            DialogBoxButtonInfo exitButtonInfo = new(
                _textKeyFactory.Create(TextKeysConstants.Common.Confirm, null),
                OnConfirmExitButtonClick,
                DialogBoxButtonType.Dismiss);

            DialogBoxButtonInfo[] buttonsInfo = new DialogBoxButtonInfo[]
            {
                cancelButtonInfo, exitButtonInfo
            };

            DialogBoxCreationData creationData = new(
                _rootVE,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.QuitLevelConfirm_Title, null),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.QuitLevelConfirm_Message, null),
                buttonsInfo);

            _dialogBoxFactory.Create(creationData);
        }

        private void OnConfirmExitButtonClick()
        {
            _sceneSwitcher.TryGoToScene(SceneType.MenuScene);
        }

        #endregion
    }
}