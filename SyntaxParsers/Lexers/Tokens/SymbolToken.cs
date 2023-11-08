namespace HsManCommonLibrary.SyntaxParsers.Lexers.Tokens;

public class SymbolToken : Token<string>
{
    public SymbolToken(string tokenValue) : base(tokenValue, TokenType.Symbol)
    {
    }

    public override string ConvertToValue()
    {
        return TokenValue;
    }
}