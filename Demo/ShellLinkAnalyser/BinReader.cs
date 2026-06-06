//using System.Buffers.Binary;

//namespace ShortcutAnalyser;

//public sealed class BinReader : IDisposable
//{
//    private readonly Stream stream;

//    public BinReader(Stream stream)
//    {
//        this.stream = new BufferedStream(stream, 8 * 1024);
//    }

//    public void Dispose()
//    {
//        stream.Dispose();
//    }

//    public Stream BaseStream => stream;

//    public bool IsEnd => stream.Position >= stream.Length;

//    public byte ReadByte() => (byte)stream.ReadByte();

//    public Int16 ReadInt16BigEndian()
//    {
//        Span<byte> buffer = stackalloc byte[2];
//        stream.ReadExactly(buffer);
//        return BinaryPrimitives.ReadInt16BigEndian(buffer);
//    }

//    public Int16 ReadInt16LittleEndian()
//    {
//        Span<byte> buffer = stackalloc byte[2];
//        stream.ReadExactly(buffer);
//        return BinaryPrimitives.ReadInt16LittleEndian(buffer);
//    }

//    public Int32 ReadInt32BigEndian()
//    {
//        Span<byte> buffer = stackalloc byte[4];
//        stream.ReadExactly(buffer);
//        return BinaryPrimitives.ReadInt32BigEndian(buffer);
//    }

//    public Int32 ReadInt32LittleEndian()
//    {
//        Span<byte> buffer = stackalloc byte[4];
//        stream.ReadExactly(buffer);
//        return BinaryPrimitives.ReadInt32LittleEndian(buffer);
//    }

//    public ReadOnlySpan<byte>  ReadChunk()
//    {
//        byte[] buffer = new byte[4];
        
//        stream.ReadExactly(buffer);
        
//        return buffer;
//    }

//    public string ReadString(int length)
//    {
//        byte[] buffer = new byte[length];
//        stream.ReadExactly(buffer);
//        return System.Text.Encoding.UTF8.GetString(buffer);
//    }

//    /// <summary>
//    /// Reads a 16-byte GUID/UUID from the stream and converts it to a string.
//    /// </summary>
//    /// <returns>The GUID as a formatted string (e.g., "00021401-0000-0000-C000-000000000046").</returns>
//    public Guid ReadGuid()
//    {
//        Span<byte> buffer = stackalloc byte[16];
//        stream.ReadExactly(buffer);
//        return new Guid(buffer);
//    }

//    /// <summary>
//    /// Reads a 16-byte GUID/UUID from the stream and returns it as a formatted string.
//    /// </summary>
//    /// <returns>The GUID as a formatted string (e.g., "00021401-0000-0000-C000-000000000046").</returns>
//    public string ReadGuidString()
//    {
//        return ReadGuid().ToString();
//    }

//    /// <summary>
//    /// Reads an 8-byte FILETIME structure from the stream and converts it to a DateTime.
//    /// FILETIME represents the number of 100-nanosecond intervals since January 1, 1601 UTC.
//    /// </summary>
//    /// <returns>A DateTime representing the FILETIME value, or DateTime.MinValue if the FILETIME is 0.</returns>
//    public DateTime ReadFileTime()
//    {
//        Span<byte> buffer = stackalloc byte[8];
//        stream.ReadExactly(buffer);
//        long fileTime = BinaryPrimitives.ReadInt64LittleEndian(buffer);

//        // FILETIME of 0 represents an invalid or uninitialized time
//        if (fileTime == 0)
//        {
//            return DateTime.MinValue;
//        }

//        try
//        {
//            return DateTime.FromFileTimeUtc(fileTime);
//        }
//        catch (ArgumentOutOfRangeException)
//        {
//            // Handle invalid FILETIME values
//            return DateTime.MinValue;
//        }
//    }

//    /// <summary>
//    /// Reads an 8-byte FILETIME structure from the stream and converts it to a formatted string.
//    /// </summary>
//    /// <param name="format">Optional. The DateTime format string. Default is "yyyy-MM-dd HH:mm:ss".</param>
//    /// <returns>A formatted string representation of the FILETIME, or "Not Set" if the FILETIME is 0.</returns>
//    public string ReadFileTimeString(string format = "yyyy-MM-dd HH:mm:ss")
//    {
//        var dateTime = ReadFileTime();

//        if (dateTime == DateTime.MinValue)
//        {
//            return "Not Set";
//        }

//        return dateTime.ToString(format);
//    }

//    public long Seek(long offset, SeekOrigin origin)
//    {
//        return stream.Seek(offset, origin);
//    }
//}
