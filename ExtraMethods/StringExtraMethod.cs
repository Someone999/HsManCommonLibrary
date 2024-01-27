using System.Text;

namespace HsManCommonLibrary.ExtractMethods;

public static class StringExtraMethod
{
    /// <summary>
    /// Converts a string to a byte array in an efficient manner.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <param name="encoding">The encoding to use; if null, UTF-8 will be used as the default encoding.</param>
    /// <returns>A byte array representing the string.</returns>
    public static byte[] GetBytes(this string str, Encoding? encoding = null)
    {
        // If encoding is null, default to UTF-8
        encoding ??= Encoding.UTF8;

        // Convert the string to a byte array using the specified or default encoding
        return encoding.GetBytes(str);
    }
}