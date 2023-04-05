using ConnectIt.Localization;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.DialogBox
{
    public class DialogBoxView : IInitializable
    {
        public const string DialogBoxAssetId = "DialogBoxAsset";
        public const string DialogBoxButtonAssetId = "DialogBoxButtonAsset";
        public const string DialogBoxButtonParentName = "buttons-group";
        public const string TitleLabelName = "title-label";
        public const string MessageLabelName = "message-label";

        private readonly ILocalizationProvider _localizationProvider;
        private readonly VisualTreeAsset _uiAsset;
        private readonly VisualTreeAsset _uiButtonAsset;
        private readonly DialogBoxButton.Factory _dialogBoxFactory;
        private DialogBoxCreationData _creationData;

        private VisualElement _parent;
        private TemplateContainer _root;
        private TextKey _titleKey;
        private TextKey _messageKey;
        private DialogBoxButtonInfo[] _buttonsInfo;

        private DialogBoxButton[] _createdButtons;
        private Label _titleLabel;
        private Label _messageLabel;

        public DialogBoxView(ILocalizationProvider localizationProvider,
            [Inject(Id = DialogBoxAssetId)] VisualTreeAsset uiAsset,
            [Inject(Id = DialogBoxButtonAssetId)] VisualTreeAsset uiButtonAsset,
            DialogBoxCreationData creationData,
            DialogBoxButton.Factory dialogBoxFactory)
        {
            _localizationProvider = localizationProvider;
            _uiAsset = uiAsset;
            _uiButtonAsset = uiButtonAsset;
            _creationData = creationData;
            _dialogBoxFactory = dialogBoxFactory;
        }

        public void Initialize()
        {
            _parent = _creationData.Parent;
            _titleKey = _creationData.TitleKey;
            _messageKey = _creationData.MessageKey;
            _buttonsInfo = _creationData.Buttons;

            if (_creationData.ShowImmediately)
                Show();

            _creationData = null;
        }

        public void Show()
        {
            _root = _uiAsset.CloneTree();
            _root.AddToClassList(ClassNamesConstants.DialogBoxRoot);
            _parent.Add(_root);

            _titleLabel = _root.Q<Label>(TitleLabelName);
            _messageLabel = _root.Q<Label>(MessageLabelName);

            CreateButtons();

            _titleKey.ArgsChanged += OnTitleKeyArgsChanged;
            _messageKey.ArgsChanged += OnMessageKeyArgsChanged;
            _localizationProvider.LocalizationChanged += UpdateLocalization;
        }

        public void Close()
        {


            _titleKey.ArgsChanged -= OnTitleKeyArgsChanged;
            _messageKey.ArgsChanged -= OnMessageKeyArgsChanged;
            _localizationProvider.LocalizationChanged -= UpdateLocalization;
        }

        private void UpdateLocalization()
        {
            UpdateTitleLocalization();
            UpdateMessageLocalization();
        }

        private void UpdateMessageLocalization()
        {
            _messageLabel.text = _messageKey.ToString();
        }

        private void UpdateTitleLocalization()
        {
            _titleLabel.text = _titleKey.ToString();
        }

        private void OnTitleKeyArgsChanged(TextKey obj)
        {
            UpdateTitleLocalization();
        }

        private void OnMessageKeyArgsChanged(TextKey obj)
        {
            UpdateMessageLocalization();
        }

        private void CreateButtons()
        {
            if (_buttonsInfo == null || _buttonsInfo.Length == 0)
                return;

            VisualElement buttonsParent = _parent.Q<VisualElement>(DialogBoxButtonParentName);
            _createdButtons = new DialogBoxButton[_buttonsInfo.Length];

            for (int i = 0; i < _buttonsInfo.Length; i++)
            {
                TemplateContainer createdButtonAsset = _uiButtonAsset.CloneTree();
                buttonsParent.Add(createdButtonAsset);

                Button button =  createdButtonAsset.Q<Button>();

                _createdButtons[i] = _dialogBoxFactory.Create(_buttonsInfo[i], button, this);
            }

            _buttonsInfo = null;
        }

        public class Factory : PlaceholderFactory<DialogBoxCreationData, DialogBoxView> { }
    }
}
