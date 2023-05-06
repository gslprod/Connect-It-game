using ConnectIt.Localization;
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Customer.Storage;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.UI.CommonViews;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Gameplay.Views.UseBoostMenu
{
    public class UseBoostMenuView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly DefaultLocalizedLabelView.Factory _defaultLabelViewFactory;
        private readonly TextKey.Factory _textKeyFactory;
        private readonly ICustomer _playerCustomer;
        private readonly UseBoostElementView.Factory _useBoostElementViewFactory;

        private VisualElement _useBoostElementsRoot;
        private DefaultLocalizedLabelView _infoLabel;
        private List<UseBoostElementView> _useBoostElementViews = new();

        public UseBoostMenuView(
            VisualElement viewRoot,
            VisualElement mainRoot,
            DefaultLocalizedLabelView.Factory defaultLabelViewFactory,
            TextKey.Factory textKeyFactory,
            ICustomer playerCustomer,
            UseBoostElementView.Factory useBoostElementViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _defaultLabelViewFactory = defaultLabelViewFactory;
            _textKeyFactory = textKeyFactory;
            _playerCustomer = playerCustomer;
            _useBoostElementViewFactory = useBoostElementViewFactory;
        }

        public void Initialize()
        {
            _useBoostElementsRoot = _viewRoot.Q<VisualElement>(TemplatesNameConstants.UseBoostMenu.BoostsContainer);

            CreateViews();
            CreateUseBoostElements();

            _playerCustomer.Storage.ItemsChanged += OnStorageItemsChanged;
        }

        public void Dispose()
        {
            DisposeDisposableViews();

            _playerCustomer.Storage.ItemsChanged -= OnStorageItemsChanged;
        }

        private void CreateViews()
        {
            _infoLabel = _defaultLabelViewFactory.Create(
                _viewRoot.Q<Label>(TemplatesNameConstants.UseBoostMenu.InfoLabel),
                _textKeyFactory.Create(TextKeysConstants.Gameplay.UseBoostMenu_InfoLabel_Text));
        }

        private void CreateUseBoostElements()
        {
            IEnumerable<ItemAmount> boostsAmounts = _playerCustomer.Storage.GetProductsAmountsOfType<Boost>();

            foreach (ItemAmount boostAmount in boostsAmounts)
                CreateUseBoostElement(boostAmount);
        }

        private void CreateUseBoostElement(ItemAmount boostItemAmount)
        {
            UseBoostElementView useBoostElementView = _useBoostElementViewFactory.Create(
                boostItemAmount.ItemType,
                _useBoostElementsRoot,
                _mainRoot);

            useBoostElementView.Disposing += OnUseBoostElementViewDisposing;
            _useBoostElementViews.Add(useBoostElementView);
        }

        private void OnUseBoostElementViewDisposing(UseBoostElementView useBoostElementView)
        {
            useBoostElementView.Disposing -= OnUseBoostElementViewDisposing;
            _useBoostElementViews.Remove(useBoostElementView);
        }

        private void DisposeDisposableViews()
        {
            _infoLabel.Dispose();

            for (int i = _useBoostElementViews.Count - 1; i >= 0; i--)
                _useBoostElementViews[i].Dispose();
        }

        private void OnStorageItemsChanged(IStorage obj)
        {
            IEnumerable<ItemAmount> boostsAmounts = _playerCustomer.Storage.GetProductsAmountsOfType<Boost>();

            if (boostsAmounts.Count() >= _useBoostElementViews.Count)
                return;

            ItemAmount last = boostsAmounts.Last();
            CreateUseBoostElement(last);
        }

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, UseBoostMenuView> { }
    }
}
