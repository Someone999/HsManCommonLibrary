namespace HsManCommonLibrary.Reader;

public class SeekableStringReader : TextReader
{
    private string _baseString;
    public int Position { get; set; }
    public SeekableStringReader(string baseString)
    {
        _baseString = baseString;
    }

    public override int Read()
    {
        if (Position == _baseString.Length)
        {
            return -1;
        }

        return _baseString[Position++];
    }
    
    public override int Peek()
    {
        if (Position == _baseString.Length)
        {
            return -1;
        }

        return _baseString[Position];
    }
    
    public char PeekChar()
    {
        var cur = Peek();
        return cur == -1 ? '\0' : (char)cur;
    }
    
    public char ReadChar()
    {
        var cur = Read();
        return cur == -1 ? '\0' : (char)cur;
    }

    public override int Read(char[] buffer, int index, int count)
    {
        if (count > _baseString.Length - Position)
        {
            count = _baseString.Length - Position;
        }

        if (count <= 0)
        {
            return 0;
        }

        char[] charArray = _baseString.ToCharArray(Position, count);

        int readCount = Math.Min(count, buffer.Length - index);
        Array.Copy(charArray, 0, buffer, index, readCount);
        Position += readCount;
        return readCount;
    }
}