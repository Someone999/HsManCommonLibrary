using HsManCommonLibrary.ValueWatchers;

namespace HsManCommonLibrary.Versioning;

public delegate void VersionChangedEventHandler<T, TVersion>(ValueChangedEventData<T> objChange,
    ValueChangedEventData<TVersion> versionChange);