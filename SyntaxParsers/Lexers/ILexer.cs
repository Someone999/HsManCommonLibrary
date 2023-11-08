using HsManCommonLibrary.SyntaxParsers.Lexers.Tokens;

namespace HsManCommonLibrary.SyntaxParsers.Lexers;

public interface ILexer
{
    void SetInput(string input);
    string CurrentInput { get; }
    IToken[] Parse();
}