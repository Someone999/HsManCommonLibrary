namespace CommonLibrary.BindableValues;

public interface IBindableValue
{
    object? DefaultValue { get; }
    object? Value { get; }
    TVal GetValueAs<TVal>();
    bool IsInitialized();
}

public interface IBindableValue<T> : IBindableValue
{
    event Action<ValueChangedEventArgs<T>> OnValueChanged;  
    new T? DefaultValue { get; }
    new T? Value { get; }
    void BindValue(T value);
}