namespace HsManCommonLibrary.NameStyleConverters;

public class SnackCaseNameStyle : INameStyle
{
    public string NameStyleName => "SnackCase";
    public bool IsThisNameStyle(string name)
    {
        bool hasEmpty = false;
        bool isLastUnderline = false;
        foreach (var ch in name)
        {
            if (ch != '_')
            {
                continue;
            }
            
            if (!isLastUnderline)
            {
                isLastUnderline = true;
            }
            else
            {
                hasEmpty = true;
            }
        }

        if (hasEmpty)
        {
            throw new FormatException("This name is not valid");
        }

        return true;
    }

    public string[] SplitName(string name)
    {
        return name.Split('_');
    }

    public string Normalize(IEnumerable<string> names)
    {
        return string.Join("_", names);
    }
}