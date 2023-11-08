namespace HsManCommonLibrary.SyntaxParsers.Lexers.Tokens;

public class StringToken : Token<string>
{
    public StringToken(string tokenValue) : base(tokenValue, TokenType.String)
    {
    }

    public override string ConvertToValue()
    {
        return TokenValue;
    }
}