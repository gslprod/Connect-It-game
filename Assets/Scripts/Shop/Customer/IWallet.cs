namespace ConnectIt.Shop.Customer
{
    public interface IWallet
    {
        long Coins { get; }

        void Add(long coins);
        bool CanWithdraw(long coins);
        bool TryWithdraw(long coins);
        void Withdraw(long coins);
    }
}