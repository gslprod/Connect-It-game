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

        private LevelProgressView.Factory _levelProgressViewFactory;

        private LevelProgressView _levelProgressView;

        [Inject]
        public void Constructor(
            LevelProgressView.Factory levelProgressViewFactory)
        {
            _levelProgressViewFactory = levelProgressViewFactory;
        }

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            _levelProgressView = _levelProgressViewFactory
                .Create(_rootVE.Q<Custom.ProgressBar>(NameConstants.LevelProgressBarName));
        }

        private void OnDestroy()
        {
            _levelProgressView.Dispose();
        }
    }
}