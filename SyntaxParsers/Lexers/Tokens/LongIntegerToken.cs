namespace HsManCommonLibrary.SyntaxParsers.Lexers.Tokens;

public class LongIntegerToken : Token<long>
{
    public LongIntegerToken(string tokenValue) : base(tokenValue, TokenType.LongInteger)
    {
    }

    public override long ConvertToValue()
    {
        return long.Parse(TokenValue);
    }
}