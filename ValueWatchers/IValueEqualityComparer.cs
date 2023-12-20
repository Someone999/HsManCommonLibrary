namespace HsManCommonLibrary.ValueWatchers;

public interface IValueEqualityComparer<in TValue>
{
    bool AreValuesEqual(TValue left, TValue right);
}
