using System.Text;

namespace HsManCommonLibrary.NameStyleConverters;

public static class NameStyleDetector
{
    public static List<INameStyle> NameStyles { get; } = new List<INameStyle>();

    public static void AddNameStyle(INameStyle nameStyle)
    {
        if (NameStyles.Any(n => n.NameStyleName == nameStyle.NameStyleName))
        {
            return;
        }

        NameStyles.Add(nameStyle);
    }
    
    public static NameStyleInfo? Detect(string name)
    {
        foreach (var nameStyle in NameStyles)
        {
            if (nameStyle.IsThisNameStyle(name))
            {
                return new NameStyleInfo(nameStyle.NameStyleName, nameStyle.SplitName(name));
            }
        }

        return null;
    }
}