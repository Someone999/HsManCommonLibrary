namespace HsManCommonLibrary.ValueWatchers;

public delegate void ValueChangedEventHandler<in TData>(object sender, TData data);