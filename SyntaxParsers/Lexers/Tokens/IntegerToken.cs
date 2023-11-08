namespace HsManCommonLibrary.SyntaxParsers.Lexers.Tokens;

public class IntegerToken : Token<int>
{
    public IntegerToken(string tokenValue) : base(tokenValue, TokenType.Integer)
    {
    }

    public override int ConvertToValue()
    {
        return int.Parse(TokenValue);
    }
}