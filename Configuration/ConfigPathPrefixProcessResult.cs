namespace HsManCommonLibrary.Configuration;

public class ConfigPathPrefixProcessResult
{
    public ConfigPathPrefixProcessResult(Type searchType, string memberName)
    {
        SearchType = searchType;
        MemberName = memberName;
    }

    public Type SearchType { get; set; }
    public string MemberName { get; set; }
    
}