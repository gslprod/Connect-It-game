using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Customer.Wallet;
using ConnectIt.Stats.Data;
using System;
using Zenject;

namespace ConnectIt.Stats.Modules
{
    public class TotalEarnedCoinsStatsModule : IStatsModule, IInitializable, IDisposable
    {
        private readonly IStatsCenter _statsCenter;
        private readonly ICustomer _playerCustomer;

        private TotalEarnedCoinsStatsData _data;

        public TotalEarnedCoinsStatsModule(
            IStatsCenter statsCenter,
            ICustomer playerCustomer)
        {
            _statsCenter = statsCenter;
            _playerCustomer = playerCustomer;
        }

        public void Initialize()
        {
            _data = _statsCenter.GetData<TotalEarnedCoinsStatsData>();
            _statsCenter.RegisterModule(this);

            _playerCustomer.Wallet.CoinsAdded += OnCoinsAdded;
        }

        public void Dispose()
        {
            _statsCenter.UnregisterModule(this);

            _playerCustomer.Wallet.CoinsAdded -= OnCoinsAdded;
        }

        private void OnCoinsAdded(IWallet wallet, long coins)
        {
            _data.InscreaseRawValue(coins);
        }
    }
}
