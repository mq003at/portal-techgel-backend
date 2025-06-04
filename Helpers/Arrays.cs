namespace portal.Helpers;

public static class ArrayHelper
{
    // Check if an array is null or empty
    public static bool IsNullOrEmpty<T>(this T[]? array)
    {
        return array == null || array.Length == 0;
    }

    // Check if an array is not null and not empty
    public static bool IsNotNullOrEmpty<T>(this T[]? array)
    {
        return !IsNullOrEmpty(array);
    }

    // Check if 2 arrays are equal (regardless of order)
    public static bool AreArraysEqual(List<int> a, List<int> b)
    {
        return a.Count == b.Count && !a.Except(b).Any() && !b.Except(a).Any();
    }
}