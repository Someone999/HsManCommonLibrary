using HsManCommonLibrary.Timing;

namespace HsManCommonLibrary.Initialization;

public interface IInitializationTimeManager
{
    IClock Clock { get; }
    IInitializationTimeRecord[] InitializationTimeRecords { get; }
    IInitializationTimeRecord? GetInitializationTimeRecord<T>();
    void RecordInitializationTime(IInitializationTimeRecord record);
    void RecordInitializationTime<T>(TimeSpan time);
    void RecordInitializationTimeWithCurrentTime<T>();
}