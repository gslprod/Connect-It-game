using ConnectIt.Coroutines;
using ConnectIt.ExternalServices.GameJolt;
using ConnectIt.UI.CommonViews;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views
{
    public class GameJoltAvatarView : DefaultSpriteView, IDisposable
    {
        private readonly GameJoltAPIProvider _gjAPIProvider;
        private readonly Sprite _defaultSprite;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;

        public GameJoltAvatarView(
            VisualElement element,
            GameJoltAPIProvider gjAPIProvider,
            Sprite defaultSprite,
            ICoroutinesGlobalContainer coroutinesGlobalContainer) : base(element, null)
        {
            _gjAPIProvider = gjAPIProvider;
            _defaultSprite = defaultSprite;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
        }

        public override void Initialize()
        {
            _gjAPIProvider.UserAvatarDownloadAttempt += OnUserAvatarDownloadAttempt;
            _gjAPIProvider.UserLogOut += UpdateAvatar;

            _coroutinesGlobalContainer.DelayedAction(UpdateAvatar);
        }

        private void OnUserAvatarDownloadAttempt(bool success)
        {
            if (!success)
                return;

            UpdateAvatar();
        }

        public void Dispose()
        {
            _gjAPIProvider.UserAvatarDownloadAttempt -= OnUserAvatarDownloadAttempt;
            _gjAPIProvider.UserLogOut -= UpdateAvatar;
        }

        private void UpdateAvatar()
        {
            sprite = _gjAPIProvider.UserExistsAndLoggedIn ?
                _gjAPIProvider.User.Avatar :
                _defaultSprite;

            float elementRadius = sprite != _defaultSprite ?
                Mathf.Min(element.resolvedStyle.width, element.resolvedStyle.height) / 2 :
                0;

            StyleLength styleRadius = new(elementRadius);
            IStyle style = element.style;

            style.borderBottomLeftRadius = styleRadius;
            style.borderTopRightRadius = styleRadius;
            style.borderBottomRightRadius = styleRadius;
            style.borderTopLeftRadius = styleRadius;

            UpdateElement();
        }

        public new class Factory : PlaceholderFactory<VisualElement, GameJoltAvatarView> { }
    }
}
