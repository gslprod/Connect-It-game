using ConnectIt.Gameplay.GameStateHandlers.GameEnd;
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Goods;
using System;
using System.Linq;
using Zenject;

namespace ConnectIt.Gameplay.GameStateHandlers.Shop
{
    public class ReversibleDisposableItemsHandler<T> : IInitializable, IDisposable
        where T : IReversibleDisposableItem
    {
        private readonly ICustomer _playerCustomer;
        private readonly ILevelEndHandler _levelEndHandler;

        public ReversibleDisposableItemsHandler(
            ICustomer playerCustomer,
            ILevelEndHandler levelEndHandler)
        {
            _playerCustomer = playerCustomer;
            _levelEndHandler = levelEndHandler;
        }

        public void Initialize()
        {
            _levelEndHandler.LevelEnded += OnLevelEnded;
        }

        public void Dispose()
        {
            _levelEndHandler.LevelEnded -= OnLevelEnded;
        }

        private void OnLevelEnded(LevelEndReason reason)
        {
            bool needToDestroy =
                reason == LevelEndReason.Win;

            int count = _playerCustomer.Storage.Items.Count();
            for (int i = count - 1; i >= 0; i--)
            {
                IProduct item = _playerCustomer.Storage.Items.ElementAt(i);

                if (item is not T reversibleDisposableItem || !reversibleDisposableItem.ReversibleDisposed)
                    continue;

                if (needToDestroy)
                    reversibleDisposableItem.Dispose();
                else
                    reversibleDisposableItem.CancelReversibleDispose();
            }
        }
    }
}
