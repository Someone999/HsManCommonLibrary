using System.Text;
using Org.BouncyCastle.Security;

namespace CommonLibrary.Utils;

public static class StringUtils
{
    public static async Task<string?> GetAsStringAsync(Stream stream, Encoding? encoding = null)
    {
        if (!stream.CanRead)
        {
            return null;
        }
        
        encoding ??= Encoding.UTF8;
        byte[] buffer = new byte[8192];
        int realRead;
        MemoryStream memoryStream = new MemoryStream();
        while ((realRead = await stream.ReadAsync(buffer, 0, 8192)) != 0)
        {
            memoryStream.Write(buffer, 0, realRead);
        }

        return encoding.GetString(memoryStream.ToArray());
    }

    static bool IsAcceptableSaltCharacter(char ch)
    {
        bool InRange(char val, char min, char max) => val >= min && val <= max;
        return InRange(ch, (char) 65, (char) 90)
            || InRange(ch, (char) 97, (char) 122)
            || InRange(ch, (char) 33, (char) 47)
            || InRange(ch, (char) 58, (char) 64)
            || InRange(ch, (char) 91, (char) 96)
            || InRange(ch, (char) 123, (char) 126);
    }
    public static string GenerateRandomString(int size)
    {
        SecureRandom secureRandom = new SecureRandom();
        StringBuilder stringBuilder = new StringBuilder();
        char lastChar = '\0';
        for (int i = 0; i < size; i++)
        {
            char asciiChar = (char)secureRandom.Next(0, 127);
            while (asciiChar == lastChar || !IsAcceptableSaltCharacter(asciiChar))
            {
                asciiChar = (char)secureRandom.Next(0, 127);
            }

            stringBuilder.Append(asciiChar);
        }

        return stringBuilder.ToString();
    }
}