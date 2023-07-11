namespace HsManCommonLibrary.Logger.Renderers;

public enum TextColorFormat
{
    /// <summary>
    /// Formats like #RRGGBB. Numbers are in hex.
    /// </summary>
    SharpHex,
    
    /// <summary>
    /// Formats like RR,GG,BB. Numbers are in hex.
    /// </summary>
    SplitHex,
    
    /// <summary>
    /// Formats like RR,GG,BB. Numbers are in decimal.
    /// </summary>
    SplitDecimal
}