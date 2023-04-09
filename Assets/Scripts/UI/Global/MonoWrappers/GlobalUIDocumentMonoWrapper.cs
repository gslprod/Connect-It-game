using UnityEngine;
using UnityEngine.UIElements;

namespace ConnectIt.UI.Global.MonoWrappers
{
    public class GlobalUIDocumentMonoWrapper : MonoBehaviour
    {
        public VisualElement Root => _rootVE;
        public VisualElement LoadingScreenCreationRoot => _loadingScreenCreationRootVE;

        private UIDocument _uiDocument;
        private VisualElement _documentRootVE => _uiDocument.rootVisualElement;
        private VisualElement _rootVE;
        private VisualElement _loadingScreenCreationRootVE;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();

            _rootVE = _documentRootVE.Q<VisualElement>(NameConstants.RootName);
            _loadingScreenCreationRootVE = _documentRootVE.Q<VisualElement>(NameConstants.LoadingScreenCreationRootName);
        }
    }
}