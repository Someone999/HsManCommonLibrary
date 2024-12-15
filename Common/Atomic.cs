namespace HsManCommonLibrary.Common;

public class Atomic<T> : IAtomic<T>
{
    private T? _value;
    private readonly object _locker = new object();

    public Atomic(T? value)
    {
        _value = value;
    }

    public T? Value
    {
        get => _value;
        set
        {
            lock (_locker)
            {
                _value = value;
            }
        }
    }
}