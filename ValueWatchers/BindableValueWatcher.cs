namespace HsManCommonLibrary.ValueWatchers;

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