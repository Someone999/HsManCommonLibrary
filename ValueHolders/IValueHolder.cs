using System.Diagnostics.CodeAnalysis;

namespace HsManCommonLibrary.ValueHolders;

public interface IValueHolder
{
    bool TryGetValueAs<TVal>(out TVal? value);
    object? DefaultValue { get; }
    object? Value { get; }
    TVal GetValueAs<TVal>();
    bool IsInitialized();
    bool HasValue { get; }
}

public interface IValueHolder<T> : IValueHolder
{
    event Action<ValueChangedEventArgs<T>> ValueChanged;  
    new T? DefaultValue { get; }
    new T? Value { get; }
    void BindValue(T value);
}