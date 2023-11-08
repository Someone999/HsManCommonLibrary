namespace HsManCommonLibrary.NestedValues.SaveStrategies;

public interface INestedValueStoreSaveStrategy
{
    void Initialize();
    bool Validate(INestedValueStore nestedValueStore);
    void Log();
    void Save(INestedValueStore nestedValueStore, string path);
    Task SaveAsync(INestedValueStore nestedValueStore, string path);
}