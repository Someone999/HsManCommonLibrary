using System.Text;

namespace HsManCommonLibrary.ExtractMethods;

public static class HashExtraMethod
{
    /// <summary>
    /// Converts a byte array to a hexadecimal string representation.
    /// </summary>
    /// <param name="hash">The byte array to convert.</param>
    /// <param name="isUpper">Specifies whether to use uppercase letters in the hexadecimal representation (default is false).</param>
    /// <returns>A hexadecimal string representation of the byte array.</returns>
    public static string GetAsString(this byte[] hash, bool isUpper = false)
    {
        // Create a StringBuilder to efficiently build the resulting string
        StringBuilder builder = new StringBuilder();

        // Iterate through each byte in the array
        foreach (var b in hash)
        {
            // Append the byte's hexadecimal representation to the string
            if (isUpper)
            {
                builder.Append($"{b:X2}");
            }
            else
            {
                builder.Append($"{b:x2}");
            }
        }

        // Return the final hexadecimal string
        return builder.ToString();
    }
}