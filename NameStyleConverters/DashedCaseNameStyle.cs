namespace HsManCommonLibrary.NameStyleConverters;

public class DashedCaseNameStyle : INameStyle
{
    public string NameStyleName => "DashedCase";
    public bool IsThisNameStyle(string name)
    {
        bool hasEmpty = false;
        bool isLastUnderline = false;
        foreach (var ch in name)
        {
            if (ch != '-')
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
        return name.Split('-');
    }

    public string Normalize(IEnumerable<string> names)
    {
        return string.Join("-", names);
    }
}