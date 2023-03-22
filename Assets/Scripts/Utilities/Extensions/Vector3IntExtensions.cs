using UnityEngine;

namespace ConnectIt.Utilities.Extensions
{
    public static class Vector3IntExtensions
    {
        public static bool IsNearTo(this Vector3Int source, Vector3Int target, int nearDistanceIncluding)
            => Mathf.Abs((source - target).DimensionsSum()) <= nearDistanceIncluding;

        public static long DimensionsSum(this Vector3Int source)
            => source.x + source.y + source.z;
    }
}
