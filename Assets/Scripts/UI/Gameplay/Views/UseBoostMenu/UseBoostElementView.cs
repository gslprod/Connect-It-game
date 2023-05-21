using ConnectIt.Infrastructure.Dispose;
using ConnectIt.Localization;
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Customer.Storage;
using ConnectIt.Shop.Goods;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.DialogBox;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Gameplay.Views.UseBoostMenu
{
    public class UseBoostElementView : IInitializable, IDisposeNotifier<UseBoostElementView>
    {
        public event Action<UseBoostElementView> Disposing;

        private readonly Type _boostType;
        private readonly VisualElement _creationRoot;
        private readonly VisualElement _mainRoot;
        private readonly DefaultLocalizedButtonView.Factory _defaultLocalizedButtonViewFactory;
        private readonly DefaultButtonView.Factory _defaultButtonViewFactory;
        private readonly DefaultLocalizedLabelView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly VisualTreeAsset _asset;
        private readonly ICustomer _playerCustomer;
        private readonly DialogBoxView.Factory _dialogBoxFactory;
        private readonly BoostUsageContext.Factory _boostUsageContextFactory;

        private VisualElement _viewRoot;
        private DefaultLocalizedLabelView _nameLabel;
        private DefaultLocalizedLabelView _amountLabel;
        private DefaultButtonView _infoButton;
        private DefaultLocalizedButtonView _useButton;
        private List<Boost> _boosts = new();

        private Boost _firstBoost => _boosts[0];

        public UseBoostElementView(
            Type boostType,
            VisualElement creationRoot,
            VisualElement mainRoot,
            DefaultButtonView.Factory defaultButtonViewFactory,
            DefaultLocalizedLabelView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            VisualTreeAsset asset,
            ICustomer playerCustomer,
            DialogBoxView.Factory dialogBoxFactory,
            DefaultLocalizedButtonView.Factory defaultLocalizedButtonViewFactory,
            BoostUsageContext.Factory boostUsageContextFactory)
        {
            _boostType = boostType;
            _creationRoot = creationRoot;
            _mainRoot = mainRoot;
            _defaultButtonViewFactory = defaultButtonViewFactory;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _asset = asset;
            _playerCustomer = playerCustomer;
            _dialogBoxFactory = dialogBoxFactory;
            _defaultLocalizedButtonViewFactory = defaultLocalizedButtonViewFactory;
            _boostUsageContextFactory = boostUsageContextFactory;
        }

        public void Initialize()
        {
            foreach (IProduct item in _playerCustomer.Storage.Items)
            {
                if (!_boostType.IsAssignableFrom(item.GetType()))
                    continue;

                AddBoost((Boost)item);
            }

            CreateViews();
            UpdateView();

            _playerCustomer.Storage.ItemsChanged += OnStorageItemsChanged;
        }

        public void Dispose()
        {
            _playerCustomer.Storage.ItemsChanged -= OnStorageItemsChanged;

            DisposeDisposableViews();

            _viewRoot.RemoveFromHierarchy();

            Disposing?.Invoke(this);
        }

        private void CreateViews()
        {
            int index = _creationRoot.childCount;
            _asset.CloneTree(_creationRoot);

            _viewRoot = _creationRoot[index];

            _nameLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.UseBoostElement.NameLabel),
                _firstBoost.Name);

            _amountLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.UseBoostElement.AmountLabel),
                _textKeyFactory.Create(
                    TextKeysConstants.Gameplay.UseBoostMenu_UseBoostElement_AmountLabel_Text,
                    new object[] { GetProductAmountFromCustomer() }));

            _infoButton = _defaultButtonViewFactory.Create(
                _viewRoot.Q<Button>(TemplatesNameConstants.UseBoostElement.InfoButton),
                OnInfoButtonClick);

            _useButton = _defaultLocalizedButtonViewFactory.Create(
                _viewRoot.Q<Button>(TemplatesNameConstants.UseBoostElement.UseButton),
                OnUseButtonClick,
                _textKeyFactory.Create(TextKeysConstants.Common.Use));
        }

        private void DisposeDisposableViews()
        {
            _nameLabel.Dispose();
            _amountLabel.Dispose();
            _infoButton.Dispose();
            _useButton.Dispose();
        }

        private int GetProductAmountFromCustomer()
            => _playerCustomer.Storage.GetProductCountOfType(_firstBoost.GetType());

        private bool IsAnyBoostUsed()
            => _boosts.Any(boost => boost.ReversibleDisposed);

        private void OnStorageItemsChanged(IStorage storage)
        {
            int count = GetProductAmountFromCustomer();

            _amountLabel.SetArgs(count);
            if (count == 0)
            {
                Dispose();
                return;
            }

            if (count <= _boosts.Count)
                return;

            IProduct last = _playerCustomer.Storage.Items.Last();
            AddBoost((Boost)last);
        }

        private void AddBoost(Boost boost)
        {
            _boosts.Add(boost);

            SubscribeToBoostItem(boost);
        }

        private void RemoveBoost(Boost boost)
        {
            UnsubscribeFromBoostItem(boost);

            _boosts.Remove(boost);
        }

        private void SubscribeToBoostItem(Boost boost)
        {
            boost.ReversibleDisposeChanged += OnBoostReversibleDisposeChanged;
            boost.Disposing += OnBoostDisposing;
        }

        private void UnsubscribeFromBoostItem(Boost boost)
        {
            boost.ReversibleDisposeChanged -= OnBoostReversibleDisposeChanged;
            boost.Disposing -= OnBoostDisposing;
        }

        private void OnBoostReversibleDisposeChanged(Boost boost)
        {
            UpdateView();
        }

        private void OnBoostDisposing(Boost boost)
        {
            RemoveBoost(boost);
            UpdateView();
        }

        private void UpdateView()
        {
            bool blocked = IsAnyBoostUsed();

            if (blocked == _viewRoot.ClassListContains(ClassNamesConstants.GameplayView.BoostElementRootUsed))
                return;

            _viewRoot.ToggleInClassList(ClassNamesConstants.GameplayView.BoostElementRootUsed);
            if (blocked)
                _useButton.SetOnClick(OnUsedButtonClick);
            else
                _useButton.SetOnClick(OnUseButtonClick);
        }

        #region DescriptionButton

        private void OnInfoButtonClick()
        {
            _dialogBoxFactory.CreateDefaultOneButtonDialogBox(_mainRoot,
                _firstBoost.Name,
                _firstBoost.Description,
                _textKeyFactory.Create(TextKeysConstants.Common.Close),
                true);
        }

        #endregion

        #region UseButton

        private void OnUseButtonClick()
        {
            if (!IsEnoughAmount())
                return;

            if (CheckForAnyBoostUse())
                return;

            _dialogBoxFactory.CreateDefaultConfirmCancelDialogBox(_mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.ConfirmBoostUse_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.ConfirmBoostUse_Message),
                _textKeyFactory,
                OnConfirmUseButtonClick,
                true);
        }

        private bool IsEnoughAmount()
        {
            if (GetProductAmountFromCustomer() > 0)
                return true;

            _dialogBoxFactory.CreateDefaultOneButtonDialogBox(_mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.NotEnoughProductAmount_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.NotEnoughProductAmount_Message),
                _textKeyFactory.Create(TextKeysConstants.Common.Close),
                true);

            return false;
        }

        private bool CheckForAnyBoostUse()
        {
            if (!IsAnyBoostUsed())
                return false;

            CreateDialogBoxAboutUsedBoost();
            return true;
        }

        #endregion

        #region ConfirmUseButton

        private void OnConfirmUseButtonClick()
        {
            if (!IsEnoughAmount())
                return;

            if (CheckForAnyBoostUse())
                return;

            Boost toUse = _boosts.First();

            CommonUsageData commonUsageData = new()
            {
                UsedBoost = toUse
            };

            BoostUsageContext usageContext = _boostUsageContextFactory.Create(commonUsageData);
            toUse.Use(usageContext);
        }

        #endregion

        #region UsedButton

        private void OnUsedButtonClick()
        {
            CreateDialogBoxAboutUsedBoost();
        }

        private void CreateDialogBoxAboutUsedBoost()
        {
            _dialogBoxFactory.CreateDefaultOneButtonDialogBox(_mainRoot,
                _textKeyFactory.Create(TextKeysConstants.DialogBox.BoostUsed_Title),
                _textKeyFactory.Create(TextKeysConstants.DialogBox.BoostUsed_Message),
                _textKeyFactory.Create(TextKeysConstants.Common.Close),
                true);
        }

        #endregion

        public class Factory : PlaceholderFactory<Type, VisualElement, VisualElement, UseBoostElementView> { }
    }
}
