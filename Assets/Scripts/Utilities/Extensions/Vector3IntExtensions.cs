using UnityEngine;

namespace ConnectIt.Utilities.Extensions
{
    public static class Vector3IntExtensions
    {
        public static bool IsNearTo(this Vector3Int source, Vector3Int target, int nearDistanceIncluding)
            => Mathf.Abs((source - target).AbsDimensionsSum()) <= nearDistanceIncluding;

        public static long AbsDimensionsSum(this Vector3Int source)
            => Mathf.Abs(source.x) + Mathf.Abs(source.y) + Mathf.Abs(source.z);
    }
}
