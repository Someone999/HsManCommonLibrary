namespace HsManCommonLibrary.NameStyleConverters;

public interface INameStyle
{
    string NameStyleName { get; }
    bool IsThisNameStyle(string name);
    string[] SplitName(string name);
    string Normalize(IEnumerable<string> names);
}