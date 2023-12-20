namespace HsManCommonLibrary.ValueWatchers;

public interface IBindableValueWatcher<TData>
{
    IBindableValue<TData> BindableValue { get; }
    event ValueChangedEventHandler<ValueChangedEventData<TData>>? OnValueChanged;
}

class BindableValueWatcher<TData> : IBindableValueWatcher<TData>
{
    public BindableValueWatcher(IBindableValue<TData> bindableValue)
    {
        BindableValue = bindableValue;
        BindableValue.OnValueChanged += OnValueChanged;
    }

    public IBindableValue<TData> BindableValue { get; }
    public event ValueChangedEventHandler<ValueChangedEventData<TData>>? OnValueChanged;
}