using ConnectIt.Shop.Customer.Storage;
using ConnectIt.Shop.Customer.Wallet;

namespace ConnectIt.Shop.Customer
{
    public interface ICustomer
    {
        IStorage Storage { get; }
        IWallet Wallet { get; }
    }
}