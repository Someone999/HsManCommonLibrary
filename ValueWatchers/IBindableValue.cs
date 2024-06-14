namespace HsManCommonLibrary.ValueWatchers;

public interface IBindableValue<TValue>
{
    void ChangeValue(TValue value);
    event ValueChangedEventHandler<ValueChangedEventData<TValue>>? OnValueChanged;
}