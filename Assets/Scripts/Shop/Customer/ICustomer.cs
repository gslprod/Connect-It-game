namespace ConnectIt.Shop.Customer
{
    public interface ICustomer
    {
        IStorage Storage { get; }
        IWallet Wallet { get; }
    }
}