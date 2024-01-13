using System.Text;
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

    public void Save(INestedValueStore nestedValueStore, Stream stream, Encoding? encoding = null)
    {
        DictionaryNestedValueConverter converter = new DictionaryNestedValueConverter();
        var result = converter.Convert(nestedValueStore);
        Serializer serializer = new Serializer();
        var content = serializer.Serialize(result);
        encoding ??= Encoding.UTF8;
        var bts = encoding.GetBytes(content);
        stream.Write(bts, 0, bts.Length);
    }

    public Task SaveAsync(INestedValueStore nestedValueStore, Stream stream, Encoding? encoding = null)
    {
        return Task.Run(() => Save(nestedValueStore, stream, encoding));
    }
}