using HsManCommonLibrary.SyntaxParsers.Lexers.Tokens;

namespace HsManCommonLibrary.NestedValues.JsonLikeFormats;

public class JsonLikeFormatParser
{
    private ValueType? MatchBestNumber(string str)
    {
        if (long.TryParse(str, out var longResult))
        {
            return longResult > int.MaxValue ? longResult : (int) longResult;
        }

        if (double.TryParse(str, out var doubleResult))
        {
            return doubleResult;
        }

        return null;
    }
    public INestedValueStore Parse(IToken[] tokens)
    {
        var rootDict = new Dictionary<string, object>();
        INestedValueStore root = new CommonNestedValueStore(rootDict);
        Stack<Dictionary<string, object>> levels = new Stack<Dictionary<string, object>>();
        levels.Push(rootDict);
        IToken? lastToken = null;
        for (var i = 0; i < tokens.Length; i++)
        {
            var currentToken = tokens[i];
            switch (currentToken)
            {
                case StringToken:
                {
                    if (tokens[i + 1] is StringToken)
                    {
                        var val = tokens[i + 1].TokenValue;
                        if (!val.Contains(' '))
                        {
                            levels.Peek().Add(currentToken.TokenValue, new CommonNestedValueStore(val));
                            i++;
                            break;
                        }
                        
                        List<object> members = new List<object>();
                        var split = val.Split(' ');
                            
                        var numberValues = (from s in split select MatchBestNumber(s)).ToArray();
                        if (numberValues.Any(v => v == null))
                        {
                            members.AddRange(split);
                        }
                        else
                        {
                            members.AddRange(numberValues);
                        }
                        
                        levels.Peek().Add(currentToken.TokenValue, new CommonNestedValueStore(members));
                        i++;
                    }

                    break;
                }
                case SymbolToken:
                    switch (currentToken.TokenValue)
                    {
                        case "{":
                            if (lastToken is not StringToken)
                            {
                                throw new Exception("Expected string token before {");
                            }
                        
                            var newDict = new Dictionary<string, object>();
                            if (levels.Peek().ContainsKey(lastToken.TokenValue))
                            {
                                levels.Peek().Remove(lastToken.TokenValue);
                            }
                            
                            levels.Peek().Add(lastToken.TokenValue, new CommonNestedValueStore(newDict));
                            levels.Push(newDict);
                            break;
                        case "}":
                            levels.Pop();
                            break;
                    }

                    break;
            }


            lastToken = currentToken;


        }

        return root;
    }
}