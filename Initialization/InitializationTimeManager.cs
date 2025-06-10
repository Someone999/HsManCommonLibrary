using System.Collections.Specialized;
using HsManCommonLibrary.Exceptions;
using HsManCommonLibrary.Timing;

namespace HsManCommonLibrary.Initialization;

public class InitializationTimeManager : IInitializationTimeManager
{
    private readonly OrderedDictionary _records = new();

    public InitializationTimeManager(IClock clock)
    {
        Clock = clock;
    }

    public IClock Clock { get; }

    public IInitializationTimeRecord[] InitializationTimeRecords => 
        _records.Values.OfType<IInitializationTimeRecord>().ToArray();

    private void ThrowWhenTypeMismatch(Type keyType, Type valueType)
    {
        if (keyType == typeof(Type) && typeof(IInitializationTimeRecord).IsAssignableFrom(valueType))
        {
            return;
        }
        
        throw new InvalidOperationException("Type mismatch. Expected: Key type: Type, value type: IInitializationTimeRecord.");
    }

    private void CheckedAdd(object? key, object? value)
    {
        if (key is null || value is null)
        {
            return;
        }
        
        ThrowWhenTypeMismatch(key.GetType(), value.GetType());
        _records.Add(key, value);
    }
    
    public IInitializationTimeRecord? GetInitializationTimeRecord<T>()
    {
        var record = _records[typeof(T)];
        if (record is not IInitializationTimeRecord recordRecord)
        {
            throw new HsManInternalException("The record is not a IInitializationTimeRecord.");
        }
        
        return recordRecord;
    }

    
    public void RecordInitializationTime(IInitializationTimeRecord record)
    {
        CheckedAdd(record.ComponentType, record);
    }

    public void RecordInitializationTime<T>(TimeSpan time)
    {
        CheckedAdd(typeof(T), new InitializationTimeRecord(typeof(T), time));
    }

    public void RecordInitializationTimeWithCurrentTime<T>()
    {
        CheckedAdd(typeof(T), new InitializationTimeRecord(typeof(T), Clock.ElapsedTime));
    }
}