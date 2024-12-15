namespace HsManCommonLibrary.Common;

public class Optional<T> : IOptional<T>
{
    public Optional(T? value, bool hasValue)
    {
        Value = value;
        HasValue = hasValue;
    }
    
    public Optional(T? value)
    {
        Value = value;
        HasValue = value != null;
    }
    
    public Optional(T? value, Func<T?, bool> valuePredicate)
    {
        Value = value;
        HasValue = valuePredicate(Value);
    }
    
    public bool HasValue { get; }
    public T? Value { get; }
    public T? GetValueOrDefault(T? defaultValue)
    {
        return !HasValue ? defaultValue : Value;
    }
}