using System.Text;

namespace HsManCommonLibrary.Utils;

public static class HashUtils
{
    /// <summary>
    /// Converts a byte sequence to a hexadecimal string representation.
    /// </summary>
    /// <param name="hash">The byte sequence to convert.</param>
    /// <param name="isUpper">Specifies whether to use uppercase letters in the hexadecimal representation (default is false).</param>
    /// <returns>A hexadecimal string representation of the byte sequence.</returns>
    public static string ToHexString(this IEnumerable<byte> hash, bool isUpper = false)
    {
        StringBuilder stringBuilder = new StringBuilder();
        
        foreach (var hashByte in hash)
        {
            if (isUpper)
            {
                stringBuilder.Append($"{hashByte:X2}");
            }
            else
            {
                stringBuilder.Append($"{hashByte:x2}");
            }
        }

        return stringBuilder.ToString();
    }
}