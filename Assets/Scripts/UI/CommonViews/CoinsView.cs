using ConnectIt.Localization;
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Customer.Wallet;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class CoinsView : DefaultLocalizedLabelView
    {
        private readonly ICustomer _userCustomer;
        private readonly TextKey.Factory _textKeyFactory;

        public CoinsView(Label versionLabel,
            ICustomer userCustomer,
            ILocalizationProvider localizationProvider,
            TextKey.Factory textKeyFactory) : base(versionLabel, null, localizationProvider)
        {
            _userCustomer = userCustomer;
            _textKeyFactory = textKeyFactory;
        }

        public override void Initialize()
        {
            textKey = _textKeyFactory.Create(
                TextKeysConstants.Menu.CoinsLabel_Text,
                new object[] { _userCustomer.Wallet.Coins });

            _userCustomer.Wallet.CoinsChanged += OnWalletCoinsChanged;

            base.Initialize();
        }

        public override void Dispose()
        {
            _userCustomer.Wallet.CoinsChanged -= OnWalletCoinsChanged;

            base.Dispose();
        }

        private void OnWalletCoinsChanged(IWallet wallet)
        {
            textKey.SetArgs(new object[] { _userCustomer.Wallet.Coins });

            UpdateLabel();
        }

        public new class Factory : PlaceholderFactory<Label, CoinsView> { }
    }
}
