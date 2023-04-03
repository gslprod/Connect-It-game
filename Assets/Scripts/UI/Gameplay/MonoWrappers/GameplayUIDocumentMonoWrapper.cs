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

        [Inject]
        public void Constructor(
            LevelProgressView.Factory levelProgressViewFactory,
            TimeView.Factory timeViewFactory)
        {
            _levelProgressViewFactory = levelProgressViewFactory;
            _timeViewFactory = timeViewFactory;
        }

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            _levelProgressView = _levelProgressViewFactory
                .Create(_rootVE.Q<Custom.ProgressBar>(NameConstants.LevelProgressBarName));

            _timeView = _timeViewFactory
                .Create(_rootVE.Q<Label>(NameConstants.TimeLabelName));
        }

        private void Update()
        {
            _timeView.Tick();
        }

        private void OnDestroy()
        {
            _levelProgressView.Dispose();
        }
    }
}