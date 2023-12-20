using System.Text;

namespace HsManCommonLibrary.NameStyleConverters;

public class UpperCamelCaseNameStyle : INameStyle
{
    public string NameStyleName => "UpperCamelCase";
    public bool IsThisNameStyle(string name)
    {
        if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
        {
            return false;
        }

        bool allUpperCase = name.All(char.IsUpper);
        if (allUpperCase)
        {
            return false;
        }
        
        return !name.Contains('_') && !name.Contains('-');
        
        
        // bool hasConsecutiveUppercase = false;
        // bool isFirst = true;
        //
        // for (int i = 1; i < name.Length; i++)
        // {
        //     if (char.IsUpper(name[i]))
        //     {
        //         if (i < name.Length - 1 && char.IsUpper(name[i + 1]))
        //         {
        //             // Two consecutive uppercase letters are allowed
        //             continue;
        //         }
        //
        //         if (!char.IsUpper(name[i - 1]) || isFirst)
        //         {
        //             isFirst = false;
        //             continue;
        //         }
        //         
        //         hasConsecutiveUppercase = true;
        //         break;
        //     }
        // }
        //
        // return !hasUnderscoreOrDash && !hasConsecutiveUppercase;
    }

    public string[] SplitName(string name)
    {
        bool isFirst = true;
        StringBuilder builder = new StringBuilder();
        List<string> nameParts = new List<string>();
        foreach (var ch in name)
        {
            if (isFirst)
            {
                isFirst = false;
                builder.Append(ch);
                continue;
            }
            
            if (!char.IsUpper(ch))
            {
                builder.Append(ch);
                continue;
            }
            
            nameParts.Add(builder.ToString());
            builder.Clear();
        }

        if (builder.Length > 0)
        {
            nameParts.Add(builder.ToString());
        }

        return nameParts.ToArray();
    }

    public string Normalize(IEnumerable<string> names)
    {
        StringBuilder builder = new StringBuilder();
        foreach (var name in names)
        {
            var tmpName = name.ToLower();
            char firstCh = tmpName[0];
            tmpName = firstCh + tmpName.Remove(0, 1);
            builder.Append(tmpName);
        }

        return builder.ToString();
    }
}