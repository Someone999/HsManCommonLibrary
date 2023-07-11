namespace HsManCommonLibrary.BindableValues;

public interface IReadonlyBindableValue : IBindableValue
{
}

public interface IReadonlyBindableValue<T> : IReadonlyBindableValue
{
    new T? DefaultValue { get; }
    new T? Value { get; }
    void BindValue(T value);
}