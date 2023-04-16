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

            if (isBlock)
                _blockVE.RemoveFromClassList(ClassNamesConstants.GlobalView.BlockPanelDisabled);
            else
                _blockVE.AddToClassList(ClassNamesConstants.GlobalView.BlockPanelDisabled);
        }

        private bool IsBlockedInternal()
            => !_blockVE.ClassListContains(ClassNamesConstants.GlobalView.BlockPanelDisabled);
    }
}