namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class ComparisionResult
{
    public DictionaryWrapper<Type, bool> Results { get; } = DictionaryWrapper<Type, bool>.CreateCurrentUsing();

    public bool IsCompatible(Type t)
    {
        return Results.TryGetValue(t, out var r) && r;
    }

    public void CacheResult(Type t, bool isCompatible)
    {
        Results.TryAdd(t, isCompatible);
        Results[t] = isCompatible;
    }
}