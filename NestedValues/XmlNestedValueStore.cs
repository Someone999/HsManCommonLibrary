namespace HsManCommonLibrary.NestedValues;

public class XmlNestedValueStore : CommonNestedValueStore
{
    public XmlNestedValueStore(object? innerVal) : base(innerVal)
    {
    }

    public Dictionary<string, INestedValueStore> XmlAttributes { get; } = new Dictionary<string, INestedValueStore>();

    public Dictionary<string, INestedValueStore> ChildElements { get; } =
        new Dictionary<string, INestedValueStore>();
}