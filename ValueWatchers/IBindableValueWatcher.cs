namespace HsManCommonLibrary.ValueWatchers;

public interface IBindableValueWatcher<TData>
{
    IBindableValue<TData> BindableValue { get; }
    event ValueChangedEventHandler<ValueChangedEventData<TData>>? ValueChanged;
}