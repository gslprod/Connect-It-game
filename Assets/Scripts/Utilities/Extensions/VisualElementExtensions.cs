using UnityEngine.UIElements;

namespace ConnectIt.Utilities.Extensions
{
    public static class VisualElementExtensions
    {
        public static VisualElement GetLastChild(this VisualElement source)
            => source[source.childCount - 1];
    }
}
