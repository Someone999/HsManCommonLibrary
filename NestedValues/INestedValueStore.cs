namespace HsManCommonLibrary.NestedValues;

public interface INestedValueStore
{
    object? GetValue();
    T? GetValueAs<T>();
    void SetValue(string key, INestedValueStore? val);
    INestedValueStore? this[string key] { get; set; }
    bool IsNull(string key);
}