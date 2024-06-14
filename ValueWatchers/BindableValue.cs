using HsManCommonLibrary.Utils;

namespace HsManCommonLibrary.ValueWatchers;

public class BindableValue<TValue> : IBindableValue<TValue>
{
    private TValue? _innerValue;

    public BindableValue(TValue? innerValue = default)
    {
        _innerValue = innerValue;
    }

    public TValue? Value => _innerValue;

    public void ChangeValue(TValue? value)
    {
        if (EqualityUtils.Equals(_innerValue, value))
        {
            return;
        }

        ValueChangedEventData<TValue> data = new ValueChangedEventData<TValue>(value, _innerValue);
        _innerValue = value;
        OnValueChanged?.Invoke(this, data);
    }

    public event ValueChangedEventHandler<ValueChangedEventData<TValue>>? OnValueChanged;
    
}