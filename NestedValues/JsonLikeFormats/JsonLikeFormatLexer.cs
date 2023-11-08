using System.Globalization;
using System.Text;
using HsManCommonLibrary.Reader;
using HsManCommonLibrary.SyntaxParsers.Lexers;
using HsManCommonLibrary.SyntaxParsers.Lexers.Tokens;

namespace HsManCommonLibrary.NestedValues.JsonLikeFormats;

public class JsonLikeFormatLexer : ILexer
{
    private SeekableStringReader _reader;

    public JsonLikeFormatLexer(string input)
    {
        CurrentInput = input;
        _reader = new SeekableStringReader(input);
    }

    public void SetInput(string input)
    {
        if (_isParsing)
        {
            throw new InvalidOperationException("Can not change input when parsing");
        }
        _reader = new SeekableStringReader(input);
        CurrentInput = input;

    }

    private char Peek()
    {
        var current = _reader.Peek();
        if (current == -1)
        {
            return '\0';
        }

        return (char)current;
    }
    
    private char Read()
    {
        var current = _reader.Read();
        if (current == -1)
        {
            return '\0';
        }

        return (char)current;
    }

    private char? ParseOctEscape(string input, SeekableStringReader reader)
    {
        if (input[reader.Position] != '\\')
        {
            return null;
        }

        var hex = input.Substring(reader.Position + 1, 3);
        var value = int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result);
        if (!value)
        {
            return null;
        }
        
        reader.Position += 4;
        return (char)result;

    }
    private char? ParseAsciiEscape(string input, SeekableStringReader reader)
    {
        if (input[reader.Position] != '\\')
        {
            return null;
        }
            
        if (input[reader.Position + 1] != 'x')
        {
            return null;
        }

        var hex = input.Substring(reader.Position + 2, 2);
        var value = int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result);
        if (value)
        {
            return (char)result;
        }

        return null;
    }
    
    private char? ParseUnicodeEscape(string input, SeekableStringReader reader)
    {
        if (input[reader.Position] != '\\')
        {
            return null;
        }
            
        if (input[reader.Position + 1] != 'u')
        {
            return null;
        }
        
        var hex = input.Substring(reader.Position + 2, 4);
        var value = int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result);
        if (!value)
        {
            return null;
        }
        
        reader.Position += 6;
        return (char)result;
    }

    char? ParseEscape(string input, SeekableStringReader reader)
    {
        switch (input[reader.Position + 1])
        {
            case 'u':
            {
                var unicode = ParseUnicodeEscape(input, reader);
                if (unicode != null)
                {
                    return unicode;
                }

                break;
            }
            case 'x':
            {
                var ascii = ParseAsciiEscape(input, reader);
                if (ascii != null)
                {
                    return ascii;
                }

                break;
            }
            default:
            {
                var oct = ParseOctEscape(input, reader);
                if (oct != null)
                {
                    return oct;
                }

                break;
            }
        }

        return null;
    }

    private bool IsComment(string input, SeekableStringReader reader)
    {
        bool isComment = input[reader.Position] == '/' && input[reader.Position + 1] == '/';
        if (!isComment)
        {
            return false;
        }
        while (Peek() != '\n')
        {
            Read();
        }

        return true;
    }
    public string CurrentInput { get; private set; }

    private bool _isParsing;
    public IToken[] Parse()
    {
        _isParsing = true;
        List<IToken> tokens = new List<IToken>();
        StringBuilder currentToken = new StringBuilder();
        while (Peek() != '\0')
        {
            var cur = Peek();
            switch (cur)
            {
                case '"':
                    Read();
                    while (Peek() != '"')
                    {
                        if (Peek() == '\\')
                        {
                            currentToken.Append(ParseEscape(CurrentInput, _reader));
                        }
                        else
                        {
                            currentToken.Append(Read());
                        }
                    }

                    Read();
                    tokens.Add(new StringToken(currentToken.ToString()));
                    currentToken.Clear();
                    break;
                
                case '{':
                    tokens.Add(new SymbolToken("{"));
                    Read();
                    break;
                case '}':
                    tokens.Add(new SymbolToken("}"));
                    Read();
                    break;
                case '/':
                    if (IsComment(CurrentInput, _reader))
                    {
                        break;
                    }

                    throw new FormatException();
                case '\n':
                case '\r':
                case '\t':
                case ' ':
                    Read();
                    break;
            }
        }

        _isParsing = false;
        return tokens.ToArray();
    }
}