using HsManCommonLibrary.NestedValues.NestedValueConverters;
using Newtonsoft.Json;

namespace HsManCommonLibrary.NestedValues.SaveStrategies;

public class JsonNestedValueStoreSaveStrategy : INestedValueStoreSaveStrategy
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
        string json = JsonConvert.SerializeObject(result);
        File.WriteAllText(path, json);
    }

    public Task SaveAsync(INestedValueStore nestedValueStore, string path)
    {
        return Task.Run(() => Save(nestedValueStore, path));
    }
}