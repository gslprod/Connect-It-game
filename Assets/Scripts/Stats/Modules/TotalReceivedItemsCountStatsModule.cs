using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Customer.Storage;
using ConnectIt.Shop.Goods;
using ConnectIt.Stats.Data;
using System;
using Zenject;

namespace ConnectIt.Stats.Modules
{
    public class TotalReceivedItemsCountStatsModule : IStatsModule, IInitializable, IDisposable
    {
        private readonly IStatsCenter _statsCenter;
        private readonly ICustomer _playerCustomer;

        private TotalReceivedItemsCountStatsData _data;

        public TotalReceivedItemsCountStatsModule(
            IStatsCenter statsCenter,
            ICustomer playerCustomer)
        {
            _statsCenter = statsCenter;
            _playerCustomer = playerCustomer;
        }

        public void Initialize()
        {
            _data = _statsCenter.GetData<TotalReceivedItemsCountStatsData>();
            _statsCenter.RegisterModule(this);

            _playerCustomer.Storage.ItemAdded += OnItemAdded;
        }

        public void Dispose()
        {
            _statsCenter.UnregisterModule(this);

            _playerCustomer.Storage.ItemAdded -= OnItemAdded;
        }

        private void OnItemAdded(IStorage storage, IProduct product)
        {
            _data.InscreaseRawValue(1);
        }
    }
}
