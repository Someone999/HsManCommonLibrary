namespace HsManCommonLibrary.ValueHolders;

public interface IReadonlyValueHolder : IValueHolder
{
}

public interface IReadonlyValueHolder<T> : IReadonlyValueHolder
{
    new T? DefaultValue { get; }
    new T? Value { get; }
    void BindValue(T value);
}