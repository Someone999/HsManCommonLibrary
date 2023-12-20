namespace HsManCommonLibrary.NameStyleConverters;

public class TargetNameStyleAttribute : Attribute
{
    public TargetNameStyleAttribute(string targetNameStyle)
    {
        TargetNameStyle = targetNameStyle;
    }

    public string TargetNameStyle { get; }
}