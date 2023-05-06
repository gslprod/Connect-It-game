using System;

namespace ConnectIt.Shop.Customer.Wallet
{
    public interface IWallet
    {
        event Action<IWallet> CoinsChanged;

        long Coins { get; }

        void Add(long coins);
        bool CanWithdraw(long coins);
        bool TryWithdraw(long coins);
        void Withdraw(long coins);
    }
}