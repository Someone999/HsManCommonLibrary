using System.Globalization;

namespace HsManCommonLibrary.Logger.Renderers;

public struct TextColor : IEquatable<TextColor>
{
    public TextColor(byte red, byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public override bool Equals(object? obj)
    {
        return obj is TextColor other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Red.GetHashCode();
            hashCode = (hashCode * 397) ^ Blue.GetHashCode();
            hashCode = (hashCode * 397) ^ Green.GetHashCode();
            return hashCode;
        }
    }

    public byte Red { get; set; } = 0x80;
    public byte Blue { get; set; } = 0x80;
    public byte Green { get; set; } = 0x80;
    

    public static bool operator ==(TextColor left, TextColor right)
    {
        return left.Blue == right.Blue && left.Green == right.Green && left.Green == right.Green;
    }

    public static bool operator !=(TextColor left, TextColor right)
    {
        return !(left == right);
    }

    public string ToSharpHexString()
    {
        return $"#{Red:X2}{Green:X2}{Blue:X2}";
    }
    public bool Equals(TextColor other)
    {
        return Red == other.Red && Blue == other.Blue && Green == other.Green;
    }

    private static TextColor ParseHex(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2),NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        return new TextColor(r, g, b);
    }
    
    private static TextColor ParseHexSplit(string hex)
    {
        string[] split = hex.Split(',');
        byte r = byte.Parse(split[0], NumberStyles.HexNumber);
        byte g = byte.Parse(split[1], NumberStyles.HexNumber);
        byte b = byte.Parse(split[2], NumberStyles.HexNumber);
        return new TextColor(r, g, b);
    }
    
    private static TextColor ParseDecimalSplit(string dec)
    {
        string[] split = dec.Split(',');
        byte r = byte.Parse(split[0]);
        byte g = byte.Parse(split[1]);
        byte b = byte.Parse(split[2]);
        return new TextColor(r, g, b);
    }
    
    public static TextColor Parse(string str, TextColorFormat format)
    {
        return format switch
        {
            TextColorFormat.SharpHex => ParseHex(str.Substring(1)),
            TextColorFormat.SplitDecimal => ParseDecimalSplit(str),
            TextColorFormat.SplitHex => ParseHexSplit(str),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }
}