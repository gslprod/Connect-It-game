﻿using ConnectIt.Shop.Customer.Storage;
using ConnectIt.Shop.Customer.Wallet;

namespace ConnectIt.Shop.Customer
{
    public class Customer : ICustomer
    {
        public IWallet Wallet { get; }
        public IStorage Storage { get; }

        public Customer(IWallet wallet,
            IStorage storage)
        {
            Wallet = wallet;
            Storage = storage;
        }
    }
}
