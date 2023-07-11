namespace HsManCommonLibrary.Utils;

public static class PropertyNameUtils
{
    public static string ToLowerCamel(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("name can not be empty or null.", nameof(name));
        }
        
        char firstChar = name[0];
        firstChar = char.ToLower(firstChar);
        return firstChar + name.Remove(0, 1);
    }
}