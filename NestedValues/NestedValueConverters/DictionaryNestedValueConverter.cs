namespace HsManCommonLibrary.NestedValues.NestedValueConverters;

public class DictionaryNestedValueConverter : INestedValueStoreConverter<Dictionary<string, object?>>
{
    public Dictionary<string, object?> Convert(INestedValueStore nestedValueStore)
    {
        return ExpendInternal(nestedValueStore.GetValue() as Dictionary<string, INestedValueStore> ??
                                  throw new InvalidCastException());
    }

    private Dictionary<string, object?> ExpendInternal(Dictionary<string, INestedValueStore> dict)
    {
        Dictionary<string, object?> result = new Dictionary<string, object?>();
        foreach (var pair in dict)
        {
            var nestedVal = pair.Value;
            
            if (nestedVal.GetValue() is Dictionary<string, INestedValueStore> d)
            {
                result.Add(pair.Key, ExpendInternal(d));
            }
            else
            {
                var val = Equals(nestedVal.GetValue(), NullObject.Value) ? null : nestedVal.GetValue();
                result.Add(pair.Key, val);
            }
        }

        return result;
    }

    object? INestedValueStoreConverter.Convert(INestedValueStore nestValueStore)
    {
        return Convert(nestValueStore);
    }
}