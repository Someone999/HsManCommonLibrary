namespace HsManCommonLibrary.ValueWatchers;

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