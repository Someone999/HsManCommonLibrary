namespace HsManCommonLibrary.Reader;

public class SeekableStringReader : TextReader
{
    private int _position = 0;
    private readonly string _innerString;

    public SeekableStringReader(string innerString)
    {
        _innerString = innerString;
    }
    
    public char PeekChar()
    {
        var next = Peek();
        return next == -1 ? '\0' : (char)next;
    }

    public char PeekChar(int offset)
    {
        int index = Position + offset;
        return index >= _innerString.Length ? '\0' : _innerString[index];
    }
    
    public char[] PeekChars(int count)
    {
        int maxCount = Math.Min(count, _innerString.Length - Position);
        char[] chars = new char[maxCount];

        for (int i = 0; i < maxCount; i++)
        {
            chars[i] = _innerString[Position + i];
        }

        return chars;
    }

    public string PeekString(int length)
    {
        if (length <= 0 || Position >= _innerString.Length)
        {
            return "";
        }
        
        int maxCount = Math.Min(length, _innerString.Length - Position);
        var sub = _innerString.Substring(Position, maxCount);
        return sub;
    }
    
    public char ReadChar()
    {
        var next = Read();
        return next == -1 ? '\0' : (char)next;
    }

    public string ReadString(int length)
    {
        string str = PeekString(length);
        Position += str.Length;
        return str;
    }

    public void ConsumeChars(int count)
    {
        Position = Math.Min(Position + count, _innerString.Length);
    }
    
    public char[] ReadChars(int count)
    {
        int maxCount = Math.Min(count, _innerString.Length - Position);
        var result = _innerString.Substring(Position, maxCount).ToCharArray();
        Position += maxCount;
        return result;
    }
    
    public override int Peek()
    {
        if (Position >= _innerString.Length)
        {
            return -1;
        }

        return _innerString[Position];
    }
    
    public override int Read()
    {
        if (Position >= _innerString.Length)
        {
            return -1;
        }
        
        return _innerString[Position++];
    }

    public void Reset()
    {
        Position = -1;
    }
    
    
    private void SetPosition(int position)
    {
        if (Position >= _innerString.Length)
        {
            throw new InvalidOperationException($"Position is too large {position}/{_innerString.Length}");
        }
        
        _position = position;
    }

    public int Position
    {
        get => _position;
        set => SetPosition(value);
    }
    
    public bool EndOfString => Position >= _innerString.Length;
}