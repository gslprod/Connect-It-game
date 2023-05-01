using ConnectIt.Localization;

namespace ConnectIt.Shop.Goods
{
    public interface IProduct
    {
        public TextKey Name { get; }
        public TextKey Description { get; }
    }
}
