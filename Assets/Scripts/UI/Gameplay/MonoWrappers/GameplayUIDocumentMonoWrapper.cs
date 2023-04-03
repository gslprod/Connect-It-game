using ConnectIt.UI.Gameplay.Views;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Custom = ConnectIt.UI.CustomControls;

namespace ConnectIt.UI.Gameplay.MonoWrappers
{
    public class GameplayUIDocumentMonoWrapper : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _rootVE => _uiDocument.rootVisualElement;

        private LevelProgressView _levelProgressView;
        private LevelProgressView.Factory _levelProgressViewFactory;
        private TimeView _timeView;
        private TimeView.Factory _timeViewFactory;
        private LevelView _levelView;
        private LevelView.Factory _levelViewFactory;

        [Inject]
        public void Constructor(
            LevelProgressView.Factory levelProgressViewFactory,
            TimeView.Factory timeViewFactory,
            LevelView.Factory levelViewFactory)
        {
            _levelProgressViewFactory = levelProgressViewFactory;
            _timeViewFactory = timeViewFactory;
            _levelViewFactory = levelViewFactory;
        }

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
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
            _levelProgressView = _levelProgressViewFactory
                .Create(_rootVE.Q<Custom.ProgressBar>(NameConstants.LevelProgressBarName));

            _timeView = _timeViewFactory
                .Create(_rootVE.Q<Label>(NameConstants.TimeLabelName));

            _levelView = _levelViewFactory
                .Create(_rootVE.Q<Label>(NameConstants.LevelLabelName));
        }

        private void SendTickToTickableViews()
        {
            _timeView.Tick();
        }

        private void DestroyDisposableViews()
        {
            _levelProgressView.Dispose();
            _levelView.Dispose();
        }
    }
}