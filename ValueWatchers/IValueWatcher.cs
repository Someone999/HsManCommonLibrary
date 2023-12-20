namespace HsManCommonLibrary.ValueWatchers;

public interface IValueWatcher<TValue>
{
    bool IsChanged(TValue obj, IValueEqualityComparer<TValue> comparer);
}