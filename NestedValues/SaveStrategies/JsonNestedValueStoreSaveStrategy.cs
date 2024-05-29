using System.Text;
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

    public void Save(INestedValueStore nestedValueStore, Stream stream, Encoding? encoding)
    {
        DictionaryNestedValueConverter converter = new DictionaryNestedValueConverter();
        var result = converter.Convert(nestedValueStore);
        string json = JsonConvert.SerializeObject(result, Formatting.Indented);
        encoding ??= Encoding.UTF8;
        var bts = encoding.GetBytes(json);
        stream.Write(bts, 0, bts.Length);
    }

    public Task SaveAsync(INestedValueStore nestedValueStore, Stream stream, Encoding? encoding)
    {
        return Task.Run(() => Save(nestedValueStore, stream, encoding));
    }
}