namespace HsManCommonLibrary.Common;

public interface IAtomic<T>
{
    T? Value { get; set; }
}