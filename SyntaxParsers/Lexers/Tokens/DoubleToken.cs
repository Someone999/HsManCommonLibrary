namespace HsManCommonLibrary.SyntaxParsers.Lexers.Tokens;

public class DoubleToken : Token<double>
{
    public DoubleToken(string tokenValue) : base(tokenValue, TokenType.Double)
    {
    }

    public override double ConvertToValue()
    {
        return double.Parse(TokenValue);
    }
}