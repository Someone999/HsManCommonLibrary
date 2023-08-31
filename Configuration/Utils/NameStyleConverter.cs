using System.Text;

namespace HsManCommonLibrary.Configuration.Utils;

public static class NameStyleConverter
{
    public enum NameStyle
    {
        LowerCamelCase,
        UpperCamelCase,
        LowerSnakeCase,
        UpperSnakeCase,
        MixedSnakeCase,
        Unknown
    }

    public static bool IsUpperString(string str) => str.All(char.IsUpper);
    public static bool IsLowerString(string str) => str.All(char.IsLower);

    public static string[] SplitByUpperChar(string str)
    {
        List<string> parts = new List<string>();
        bool isFirst = true;
        StringBuilder namePart = new StringBuilder();
        foreach (var ch in str)
        {
            
            if (char.IsUpper(ch) && !isFirst)
            {
                parts.Add(namePart.ToString());
                namePart.Clear();
                namePart.Append(ch);
                continue;
            }
            
            namePart.Append(ch);
            if (isFirst)
            {
                isFirst = false;
            }
        }

        if (namePart.Length != 0)
        {
            parts.Add(namePart.ToString());
        }

        return parts.ToArray();
    }
    public static string[] SplitByUnderline(string str) => str.Split('_');

    public static string ToUpper(string str)
    {
        StringBuilder builder = new StringBuilder();
        foreach (var ch in str)
        {
            if (char.IsLetter(ch))
            {
                builder.Append(char.ToUpper(ch));
            }
            else if(char.IsDigit(ch))
            {
                builder.Append(ch);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        return builder.ToString();
    }
    
    public static string ToLower(string str)
    {
        StringBuilder builder = new StringBuilder();
        foreach (var ch in str)
        {
            if (char.IsLetter(ch))
            {
                builder.Append(char.ToLower(ch));
            }
            else if(char.IsDigit(ch))
            {
                builder.Append(ch);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        return builder.ToString();
    }
    
    public static string ToLowerCamelCase(IEnumerable<string> nameParts)
    {
        StringBuilder builder = new StringBuilder();
        bool isFirst = true;
        foreach (var str in nameParts)
        {
            if (char.IsUpper(str[0]) && isFirst)
            {
                builder.Append(ToLower(str));
                isFirst = false;
            }

            builder.Append(str);
        }

        return builder.ToString();
    }
    
    public static string ToUpperCamelCase(IEnumerable<string> nameParts)
    {
        StringBuilder builder = new StringBuilder();
        bool isFirst = true;
        foreach (var str in nameParts)
        {
            if (char.IsLower(str[0]) && isFirst)
            {
                builder.Append(ToUpper(str));
                isFirst = false;
            }

            builder.Append(str);
        }

        return builder.ToString();
    }

    public static string[] MixedSnakeToLowerNames(string[] nameParts)
        => nameParts.Select(ToLower).ToArray();
    
    public static NameStyle DetectNameStyle(string name)
    {
        if (name.All(c => !char.IsLetterOrDigit(c) && c != '_'))
        {
            return NameStyle.Unknown;
        }
            
        if (name.Contains('_'))
        {
            string[] nameParts = SplitByUnderline(name);
            bool isAllUpper = nameParts.All(IsUpperString);
            bool isAllLower = nameParts.All(IsLowerString);

            if (nameParts.Length == 1 && isAllLower)
            {
                return NameStyle.LowerCamelCase;
            }
            
            return isAllLower ? NameStyle.LowerSnakeCase : isAllUpper ? NameStyle.UpperSnakeCase : 
                NameStyle.MixedSnakeCase;
        }

        string[] caseNameParts = SplitByUpperChar(name);
        bool isUpperCase = char.IsUpper(caseNameParts[0][0]);
        bool isLowerCase = char.IsLower(caseNameParts[0][0]);

        return isUpperCase ? NameStyle.UpperCamelCase : isLowerCase ? NameStyle.LowerCamelCase : NameStyle.Unknown;
    }

    public static string ConvertToUpperSnake(string name)
    {
        var nameStyle = DetectNameStyle(name);
        switch (nameStyle)
        {
            case NameStyle.UpperSnakeCase:
                return name;
            case NameStyle.MixedSnakeCase:
                return string.Join("_", MixedSnakeToLowerNames(SplitByUnderline(name)).Select(ToUpper));
            case NameStyle.LowerSnakeCase:
                return string.Join("_", SplitByUnderline(name).Select(ToUpper));
            case NameStyle.LowerCamelCase:
            case NameStyle.UpperCamelCase:
                return string.Join("_", SplitByUpperChar(name).Select(ToUpper));
            case NameStyle.Unknown:
            default:
                throw new NotSupportedException();
        }
    }
    
    public static string ConvertToLowerSnake(string name)
    {
        var nameStyle = DetectNameStyle(name);
        switch (nameStyle)
        {
            case NameStyle.UpperSnakeCase:
                return string.Join("_", SplitByUnderline(name).Select(ToLower));
            case NameStyle.MixedSnakeCase:
                return string.Join("_", MixedSnakeToLowerNames(SplitByUnderline(name)));
            case NameStyle.LowerSnakeCase:
                return name;
            case NameStyle.LowerCamelCase:
            case NameStyle.UpperCamelCase:
                return string.Join("_", SplitByUpperChar(name).Select(ToLower));
            case NameStyle.Unknown:
            default:
                throw new NotSupportedException();
        }
    }
    
    public static string ConvertToLowerCamel(string name)
    {
        var nameStyle = DetectNameStyle(name);
        switch (nameStyle)
        {
            case NameStyle.MixedSnakeCase:
                return ToLowerCamelCase(MixedSnakeToLowerNames(SplitByUnderline(name)));
            case NameStyle.UpperSnakeCase:
            case NameStyle.LowerSnakeCase:
                return ToLowerCamelCase( SplitByUnderline(name));
            case NameStyle.LowerCamelCase:
                return name;
            case NameStyle.UpperCamelCase:
                return ToLowerCamelCase(SplitByUpperChar(name));
            case NameStyle.Unknown:
            default:
                throw new NotSupportedException();
        }
    }
    
    public static string ConvertToUpperCamel(string name)
    {
        var nameStyle = DetectNameStyle(name);
        switch (nameStyle)
        {
            case NameStyle.MixedSnakeCase:
                return ToUpperCamelCase(MixedSnakeToLowerNames(SplitByUnderline(name)));
            case NameStyle.UpperSnakeCase:
            case NameStyle.LowerSnakeCase:
                return ToUpperCamelCase( SplitByUnderline(name));
            case NameStyle.LowerCamelCase:
                return ToUpperCamelCase(SplitByUpperChar(name));
            case NameStyle.UpperCamelCase:
                return name;
            case NameStyle.Unknown:
            default:
                throw new NotSupportedException();
        }
    }

    public static string ConvertNameStyle(string name, NameStyle nameStyle)
    {
        switch (nameStyle)
        {
            case NameStyle.LowerCamelCase:
                return ConvertToLowerCamel(name);
            case NameStyle.UpperCamelCase:
                return ConvertToUpperCamel(name);
            case NameStyle.UpperSnakeCase:
                return ConvertToUpperSnake(name);
            case NameStyle.LowerSnakeCase:
                return ConvertToLowerSnake(name);
            case NameStyle.Unknown:
            default:
                throw new NotSupportedException();
            
        }
        
    }
}