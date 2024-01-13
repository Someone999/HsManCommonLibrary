using System.Collections;

namespace HsManCommonLibrary.Utils;

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

    public static bool SequenceEquals<T>(IEnumerable<T> a, IEnumerable<T> b)
    {
        var arrA = a.ToArray();
        var arrB = b.ToArray();
        if (arrA.Length != arrB.Length)
        {
            return false;
        }

        for (int i = 0; i < arrA.Length; i++)
        {
            if (!Equals(arrA[i], arrB[i]))
            {
                return false;
            }
        }

        return true;
    }
}