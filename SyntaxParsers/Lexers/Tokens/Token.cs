namespace HsManCommonLibrary.SyntaxParsers.Lexers.Tokens;


public interface IToken
{
    string TokenValue { get; }
    TokenType TokenType { get; }
    object? ConvertToValue();
}

public interface IToken<out TValue> : IToken
{
    new TValue? ConvertToValue();
}

public abstract class Token<TValue> : IToken<TValue>
{
    protected Token(string tokenValue, TokenType tokenType)
    {
        TokenValue = tokenValue;
        TokenType = tokenType;
    }

    public string TokenValue { get; }
    public TokenType TokenType { get; }
    object? IToken.ConvertToValue()
    {
        return ConvertToValue();
    }

    public abstract TValue? ConvertToValue();
}