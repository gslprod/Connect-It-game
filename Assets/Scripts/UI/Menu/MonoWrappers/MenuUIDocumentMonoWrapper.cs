using UnityEngine;
using UnityEngine.UIElements;

namespace ConnectIt.UI.Menu.MonoWrappers
{
    public class MenuUIDocumentMonoWrapper : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _documentRootVE => _uiDocument.rootVisualElement;
        private VisualElement _rootVE;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();

            _rootVE = _documentRootVE.Q<VisualElement>(NameConstants.RootName);
        }
    }
}