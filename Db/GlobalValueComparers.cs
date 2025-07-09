using Microsoft.EntityFrameworkCore.ChangeTracking;

public static class GlobalValueComparers
{
    public static readonly ValueComparer<List<int>> IntListComparer = new ValueComparer<List<int>>(
        (c1, c2) => c1!.SequenceEqual(c2!),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c.ToList()
    );

    public static readonly ValueComparer<List<string>> StringListComparer = new ValueComparer<
        List<string>
    >(
        (c1, c2) => c1!.SequenceEqual(c2!),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c.ToList()
    );
}
