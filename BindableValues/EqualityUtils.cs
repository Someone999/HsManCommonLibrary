using System.Collections;

namespace HsManCommonLibrary.BindableValues;

public static class EqualityUtils
{
    public static bool Equals<T>(T a, T b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a == null || b == null)
        {
            return false;
        }

        switch (a)
        {
            case IEquatable<T> equatable:
                return equatable.Equals(b);
            case IEqualityComparer<T> genericEqualityComparer:
                return genericEqualityComparer.Equals(b);
            case IEqualityComparer equalityComparer:
                return equalityComparer.Equals(b);
            case IComparable comparable:
                return comparable.CompareTo(b) == 0;
            case IComparable<T> genericComparable:
                return genericComparable.CompareTo(b) == 0;
            default:
                return a.Equals(b);
        }
    }
}