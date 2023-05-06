using System;

namespace ConnectIt.Shop.Customer.Storage
{
    public readonly struct ItemAmount
    {
        public Type ItemType { get; }
        public int Amount { get; }

        public ItemAmount(Type itemType, int amount)
        {
            ItemType = itemType;
            Amount = amount;
        }
    }
}
