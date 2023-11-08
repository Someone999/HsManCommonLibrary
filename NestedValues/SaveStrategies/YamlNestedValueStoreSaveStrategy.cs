using HsManCommonLibrary.NestedValues.NestedValueConverters;
using YamlDotNet.Serialization;

namespace HsManCommonLibrary.NestedValues.SaveStrategies;

public class YamlNestedValueStoreSaveStrategy : INestedValueStoreSaveStrategy
{
    public void Initialize()
    {
    }

    public bool Validate(INestedValueStore nestedValueStore)
    {
        return true;
    }

    public void Log()
    {
    }

    public void Save(INestedValueStore nestedValueStore, string path)
    {
        DictionaryNestedValueConverter converter = new DictionaryNestedValueConverter();
        var result = converter.Convert(nestedValueStore);
        Serializer serializer = new Serializer();
        File.WriteAllText(path, serializer.Serialize(result));
    }

    public Task SaveAsync(INestedValueStore nestedValueStore, string path)
    {
        return Task.Run(() => Save(nestedValueStore, path));
    }
}