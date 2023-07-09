namespace CommonLibrary.BindableValues;

public class ValueChangedEventArgs<T>
{
    public ValueChangedEventArgs(T? oldValue, T value)
    {
        OldValue = oldValue;
        Value = value;
    }

    public T? OldValue { get; }
    public T Value { get; }
}