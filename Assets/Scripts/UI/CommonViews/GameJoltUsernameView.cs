using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.Localization;
using ConnectIt.Utilities.Extensions;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class GameJoltUsernameView : DefaultUniversalTextElementView
    {
        private readonly GameJoltAPIProvider _gjApiProvider;
        private readonly TextKey.Factory _textKeyFactory;

        public GameJoltUsernameView(
            Label label,
            ILocalizationProvider localizationProvider,
            GameJoltAPIProvider gjApiProvider,
            TextKey.Factory textKeyFactory) : base(label, null, null, localizationProvider)
        {
            _gjApiProvider = gjApiProvider;
            _textKeyFactory = textKeyFactory;
        }

        public override void Initialize()
        {
            textKey = _textKeyFactory.Create(TextKeysConstants.Common.Guest);

            _gjApiProvider.UserInfoUpdateAttempt += OnUserInfoUpdateAttempt;
            _gjApiProvider.UserLogInAttempt += OnUserInfoUpdateAttempt;
            _gjApiProvider.UserLogOut += UpdateInfo;

            base.Initialize();

            UpdateInfo();
        }

        public override void Dispose()
        {
            _gjApiProvider.UserInfoUpdateAttempt -= OnUserInfoUpdateAttempt;
            _gjApiProvider.UserLogInAttempt -= OnUserInfoUpdateAttempt;
            _gjApiProvider.UserLogOut -= UpdateInfo;

            base.Dispose();
        }

        private void OnUserInfoUpdateAttempt(bool success)
        {
            if (!success)
                return;

            UpdateInfo();
        }

        private void UpdateInfo()
        {
            if (_gjApiProvider.UserExistsAndLoggedIn)
                text = _gjApiProvider.User.Name;

            SetLocalizationEnable(!_gjApiProvider.UserExistsAndLoggedIn);
            UpdateLabel();
        }

        public new class Factory : PlaceholderFactory<Label, GameJoltUsernameView> { }
    }
}
