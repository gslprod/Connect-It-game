using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultSpriteView : IInitializable
    {
        protected VisualElement element;
        protected Sprite sprite;

        public DefaultSpriteView(
            VisualElement element,
            Sprite sprite)
        {
            this.element = element;
            this.sprite = sprite;
        }

        public virtual void Initialize()
        {
            UpdateElement();
        }

        protected void UpdateElement()
        {
            element.style.backgroundImage = new(sprite);
        }

        public class Factory : PlaceholderFactory<VisualElement, Sprite, DefaultSpriteView> { }
    }
}
