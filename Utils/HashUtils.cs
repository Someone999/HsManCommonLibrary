using System.Text;

namespace CommonLibrary.Utils;

public static class HashUtils
{
    public static string GetHexString(this byte[] hash)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var hashByte in hash)
        {
            stringBuilder.Append($"{hashByte:X02}");
        }

        return stringBuilder.ToString();
    }
}