namespace HsManCommonLibrary.NestedValues.Attributes;

public class AutoAssignCollectionAttribute
{
    public AutoAssignCollectionAttribute(string path)
    {
        Path = path;
    }

    public string Path { get; }
}