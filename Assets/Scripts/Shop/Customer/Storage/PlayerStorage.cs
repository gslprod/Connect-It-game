using ConnectIt.Save.Names;
using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Shop.Goods;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using static ConnectIt.Save.SaveProviders.SaveData.ShopSaveData;

namespace ConnectIt.Shop.Customer.Storage
{
    public class PlayerStorage : Storage, IInitializable, IDisposable
    {
        private readonly IShopSaveProvider _saveProvider;
        private readonly SkipLevelBoost.Factory _skipLevelBoostFactory;
        private readonly SimplifyLevelBoost.Factory _simplifyLevelBoostFactory;
        private readonly AllowIncompatibleConnectionsBoost.Factory _allowIncompatibleConnectionsBoostFactory;

        public PlayerStorage(
            IShopSaveProvider saveProvider,
            SkipLevelBoost.Factory skipLevelBoostFactory,
            SimplifyLevelBoost.Factory simplifyLevelBoostFactory,
            AllowIncompatibleConnectionsBoost.Factory allowIncompatibleConnectionsBoostFactory)
        {
            _saveProvider = saveProvider;
            _skipLevelBoostFactory = skipLevelBoostFactory;
            _simplifyLevelBoostFactory = simplifyLevelBoostFactory;
            _allowIncompatibleConnectionsBoostFactory = allowIncompatibleConnectionsBoostFactory;
        }

        public void Initialize()
        {
            LoadItems();

            ItemsChanged += OnItemsChanged;
        }

        public void Dispose()
        {
            ItemsChanged -= OnItemsChanged;
        }

        private void SaveItems()
        {
            StorageItemSaveData[] itemsSaveData = new StorageItemSaveData[itemsCounts.Count];

            for (int i = 0; i < itemsCounts.Count; i++)
            {
                KeyValuePair<Type, int> pair = itemsCounts.ElementAt(i);

                StorageItemSaveData saveData = new()
                {
                    ItemSaveName = SaveNames.GetSaveName(pair.Key),
                    Amount = pair.Value
                };

                itemsSaveData[i] = saveData;
            }

            ShopSaveData shopSaveData = _saveProvider.LoadShopData();
            shopSaveData.StorageItems = itemsSaveData;
            _saveProvider.SaveShopData(shopSaveData);
        }

        private void OnItemsChanged(IStorage storage)
        {
            SaveItems();
        }

        private void LoadItems()
        {
            ShopSaveData shopSaveData = _saveProvider.LoadShopData();

            if (shopSaveData.StorageItems == null)
                return;

            items.Clear();
            for (int i = 0; i < shopSaveData.StorageItems.Length; i++)
            {
                StorageItemSaveData levelSaveData = shopSaveData.StorageItems[i];

                for (int j = 0; j < levelSaveData.Amount; j++)
                    AddItemWithoutNotify(CreateProductByTypeName(levelSaveData.ItemSaveName));
            }

            InvokeItemsChangedEvent();
        }

        private IProduct CreateProductByTypeName(string typeName)
        {
            Type type = SaveNames.GetTypeBySaveName(typeName);
            Assert.That(typeof(IProduct).IsAssignableFrom(type));

            if (type == typeof(SkipLevelBoost))
                return _skipLevelBoostFactory.Create();

            if (type == typeof(SimplifyLevelBoost))
                return _simplifyLevelBoostFactory.Create();

            if (type == typeof(AllowIncompatibleConnectionsBoost))
                return _allowIncompatibleConnectionsBoostFactory.Create();

            throw Assert.GetFailException();
        }
    }
}
