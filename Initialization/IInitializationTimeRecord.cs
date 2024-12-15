using HsManCommonLibrary.Timing;

namespace HsManCommonLibrary.Initialization;

public interface IInitializationTimeRecord
{
    Type ComponentType { get; }
    TimeSpan InitTime { get; }
}