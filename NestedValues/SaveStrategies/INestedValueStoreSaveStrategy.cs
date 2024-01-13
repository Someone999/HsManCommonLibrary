using System.Text;

namespace HsManCommonLibrary.NestedValues.SaveStrategies;

public interface INestedValueStoreSaveStrategy
{
    void Initialize();
    bool Validate(INestedValueStore nestedValueStore);
    void Log();
    void Save(INestedValueStore nestedValueStore, Stream stream, Encoding? encoding = null);
    Task SaveAsync(INestedValueStore nestedValueStore, Stream stream, Encoding? encoding = null);
}