namespace HsManCommonLibrary.NameStyleConverters;

public class NameStyleInfo
{
    public NameStyleInfo(string nameStyle, string[] nameParts)
    {
        NameStyle = nameStyle;
        NameParts = nameParts;
    }

    public string NameStyle { get; set; }
    public string[] NameParts { get; }
}