using ConnectIt.Utilities;

namespace ConnectIt.Shop.Customer
{
    public class Wallet : IWallet
    {
        public long Coins { get; private set; }

        public void Add(long coins)
        {
            Coins += coins;
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
        }
    }
}
