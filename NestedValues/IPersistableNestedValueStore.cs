namespace HsManCommonLibrary.NestedValues;

public interface IPersistableNestedValueStore : INestedValueStore
{
    void Persistence(string path);
}