namespace HsManCommonLibrary.ValueHolders;

public interface IValueHolder
{
    object? DefaultValue { get; }
    object? Value { get; }
    TVal GetValueAs<TVal>();
    bool IsInitialized();
}

public interface IValueHolder<T> : IValueHolder
{
    event Action<ValueChangedEventArgs<T>> OnValueChanged;  
    new T? DefaultValue { get; }
    new T? Value { get; }
    void BindValue(T value);
}