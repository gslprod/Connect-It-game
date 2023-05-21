using ConnectIt.Infrastructure.Setters;
using ConnectIt.UI.Tools;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConnectIt.UI.Global.MonoWrappers
{
    public class GlobalUIDocumentMonoWrapper : MonoBehaviour, IUIBlocker
    {
        public VisualElement Root => _rootVE;
        public VisualElement LoadingScreenCreationRoot => _loadingScreenCreationRootVE;
        public bool Blocked => IsBlockedInternal();

        private UIDocument _uiDocument;
        private VisualElement _documentRootVE => _uiDocument.rootVisualElement;
        private VisualElement _rootVE;
        private VisualElement _loadingScreenCreationRootVE;
        private VisualElement _blockVE;

        private PriorityAwareSetter<bool> _blockSetter;

        private void Awake()
        {
            //todo
            //for some reason on android Zenject doesnt marks this object as DontDestroyOnLoad, whether this object is creating from ProjectContext
            //in the editor playmode this problem doesnt appear
            DontDestroyOnLoad(gameObject);

            _uiDocument = GetComponent<UIDocument>();

            _rootVE = _documentRootVE.Q<VisualElement>(NameConstants.RootName);
            _loadingScreenCreationRootVE = _documentRootVE.Q<VisualElement>(NameConstants.LoadingScreenCreationRootName);

            _blockVE = _documentRootVE.Q<VisualElement>(NameConstants.BlockPanelName);
            _blockSetter = new(SetBlockInternal, IsBlockedInternal, false);
        }

        public void SetBlock(bool isPause, int priority, object source)
        {
            _blockSetter.SetValue(isPause, priority, source);
        }

        public void ResetBlock(object source)
        {
            _blockSetter.ResetValue(source);
        }

        private void SetBlockInternal(bool isBlock)
        {
            if (IsBlockedInternal() == isBlock)
                return;

            _blockVE.ToggleInClassList(ClassNamesConstants.GlobalView.BlockPanelDisabled);
        }

        private bool IsBlockedInternal()
            => !_blockVE.ClassListContains(ClassNamesConstants.GlobalView.BlockPanelDisabled);
    }
}