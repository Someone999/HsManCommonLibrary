namespace HsManCommonLibrary.ValueWatchers;

public delegate void ValueChangedEventHandler<in TData>(object sender, TData data);

public class ValueChangedEventData<TValue>
{
    public ValueChangedEventData(TValue? currentValue, TValue? lastValue)
    {
        CurrentValue = currentValue;
        LastValue = lastValue;
    }

    public TValue? CurrentValue { get; }
    public TValue? LastValue { get; }
}