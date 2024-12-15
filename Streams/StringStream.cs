using System.Text;

namespace HsManCommonLibrary.Streams;

public class StringStream : Stream
{
    private readonly Stream _underlyingStream;

    public StringStream(Stream underlyingStream)
    {
        _underlyingStream = underlyingStream;
    }

    public override void Flush()
    {
       _underlyingStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _underlyingStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _underlyingStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _underlyingStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _underlyingStream.Write(buffer, offset, count);
    }

    public Encoding Encoding { get; set; } = Encoding.UTF8;
    
    
    public string ReadLine(byte[] delimiter)
    {
        byte[] buffer = new byte[16];
        List<byte> bytes = new List<byte>();
        int readCount;

        int delimiterPos = 0;
        do
        {
            readCount = _underlyingStream.Read(buffer, 0, buffer.Length);
            for (int i = 0; i < readCount; i++)
            {
                if (buffer[i] == delimiter[delimiterPos])
                {
                    delimiterPos++;
                    if (delimiterPos == delimiter.Length)
                    {
                        return Encoding.GetString(bytes.ToArray());
                    }
                }
                else
                {
                    delimiterPos = 0;
                    bytes.Add(buffer[i]);
                }
            }
        } while (readCount > 0);
        
        
        return Encoding.GetString(bytes.ToArray());
    }

    public override bool CanRead => _underlyingStream.CanRead;
    public override bool CanSeek => _underlyingStream.CanSeek;
    public override bool CanWrite => _underlyingStream.CanWrite;
    public override long Length => _underlyingStream.Length;

    public override long Position
    {
        get => _underlyingStream.Position;
        set => _underlyingStream.Position = value;
    }
}