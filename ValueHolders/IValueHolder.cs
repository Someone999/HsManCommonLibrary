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
    void ResetValue();
    void Uninitialize();
    void Clear();
}

public interface IValueHolder<T> : IValueHolder
{
    event Action<ValueChangedEventArgs<T>> ValueChanged;  
    new T? DefaultValue { get; }
    new T? Value { get; }
    void SetValue(T value);
}