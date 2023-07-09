using System.Collections;

namespace CommonLibrary.Utils;

public static class EqualityUtils
{
    public static bool Equals<T>(T? a, T? b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a == null || b == null)
        {
            return false;
        }

        return a switch
        {
            IEquatable<T> equatable => equatable.Equals(b),
            IEqualityComparer<T> genericEqualityComparer => genericEqualityComparer.Equals(b),
            IEqualityComparer equalityComparer => equalityComparer.Equals(b),
            IComparable comparable => comparable.CompareTo(b) == 0,
            IComparable<T> genericComparable => genericComparable.CompareTo(b) == 0,
            _ => a.Equals(b)
        };
    }
}