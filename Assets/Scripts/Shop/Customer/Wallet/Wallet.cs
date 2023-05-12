using ConnectIt.Utilities;
using System;

namespace ConnectIt.Shop.Customer.Wallet
{
    public class Wallet : IWallet
    {
        public event Action<IWallet> CoinsChanged;

        public long Coins { get; protected set; }

        public void Add(long coins)
        {
            Coins += coins;
            CoinsChanged?.Invoke(this);
        }

        public void Withdraw(long coins)
        {
            Assert.That(CanWithdraw(coins));

            Remove(coins);
        }

        public bool TryWithdraw(long coins)
        {
            if (!CanWithdraw(coins))
                return false;

            Remove(coins);
            return true;
        }

        public bool CanWithdraw(long coins)
            => coins <= Coins;

        private void Remove(long coins)
        {
            Coins -= coins;
            CoinsChanged?.Invoke(this);
        }

        protected void InvokeCoinsChangedEvent()
        {
            CoinsChanged?.Invoke(this);
        }
    }
}
