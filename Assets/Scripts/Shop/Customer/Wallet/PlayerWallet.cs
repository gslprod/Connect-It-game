using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Shop.Customer.Storage;
using System;
using System.Collections.Generic;
using Zenject;
using static ConnectIt.Localization.TextKeysConstants;
using static ConnectIt.Save.SaveProviders.SaveData.ShopSaveData;

namespace ConnectIt.Shop.Customer.Wallet
{
    public class PlayerWallet : Wallet, IInitializable, IDisposable
    {
        private readonly IShopSaveProvider _saveProvider;

        public PlayerWallet(
            IShopSaveProvider saveProvider)
        {
            _saveProvider = saveProvider;
        }

        public void Initialize()
        {
            LoadCoins();

            CoinsChanged += OnCoinsChanged;
        }

        public void Dispose()
        {
            CoinsChanged -= OnCoinsChanged;
        }

        private void SaveCoins()
        {
            ShopSaveData shopSaveData = _saveProvider.LoadShopData();
            shopSaveData.Coins = Coins;
            _saveProvider.SaveShopData(shopSaveData);
        }

        private void LoadCoins()
        {
            ShopSaveData shopSaveData = _saveProvider.LoadShopData();
            Coins = shopSaveData.Coins;

            InvokeCoinsChangedEvent();
        }

        private void OnCoinsChanged(IWallet wallet)
        {
            SaveCoins();
        }
    }
}
