namespace HsManCommonLibrary.Common;

public interface IOptional<T>
{
    bool HasValue { get; }
    T? Value { get; }
    T? GetValueOrDefault(T? defaultValue);
}