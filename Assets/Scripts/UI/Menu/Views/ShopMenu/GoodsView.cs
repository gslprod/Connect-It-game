using ConnectIt.Shop;
using ConnectIt.UI.Menu.Views.StatsMenu;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.ShopMenu
{
    public class GoodsView : IInitializable, IDisposable
    {
        private readonly VisualElement _viewRoot;
        private readonly VisualElement _mainRoot;
        private readonly IShop _shop;
        private readonly ProductView.Factory _productViewFactory;

        private readonly List<ProductView> _productViews = new();

        public GoodsView(
            VisualElement viewRoot,
            VisualElement mainRoot,
            IShop shop,
            ProductView.Factory productViewFactory)
        {
            _viewRoot = viewRoot;
            _mainRoot = mainRoot;
            _shop = shop;
            _productViewFactory = productViewFactory;
        }

        public void Initialize()
        {
            CreateViews();

            _shop.GoodsChanged += OnGoodsChanged;
        }

        public void Dispose()
        {
            _shop.GoodsChanged -= OnGoodsChanged;

            DisposeDisposableViews();
        }

        private void CreateViews()
        {
            foreach (var product in _shop.Goods)
            {
                ProductView createdView =
                    _productViewFactory.Create(product, _viewRoot, _mainRoot);

                _viewRoot.GetLastChild().AddToClassList(ClassNamesConstants.Global.ScrollViewContainerChild);
                _productViews.Add(createdView);
            }

            if (_productViews.Count > 0)
                _viewRoot.GetLastChild().AddToClassList(ClassNamesConstants.Global.ScrollViewContainerChildLast);
        }

        private void DisposeDisposableViews()
        {
            for (int i = _productViews.Count - 1; i >= 0; i--)
            {
                ProductView productView = _productViews[i];

                productView.Dispose();
            }

            _productViews.Clear();
        }

        private void OnGoodsChanged(IShop shop)
        {
            DisposeDisposableViews();
            CreateViews();
        }

        public class Factory : PlaceholderFactory<VisualElement, VisualElement, GoodsView> { }
    }
}
