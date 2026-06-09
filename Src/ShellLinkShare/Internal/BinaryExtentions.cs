using System.Buffers.Binary;

namespace ShellLink;

internal static class BinaryExtentions
{
    private const int MaxStringLength = 260;

    extension(BinaryReader reader)
    {
        public uint Position
        {
            get => (uint)reader.BaseStream.Position;
            set => reader.BaseStream.Position = value;
        }
    }

    extension(BinaryWriter writer)
    {
        public uint Position
        {
            get => (uint)writer.BaseStream.Position;
            set => writer.BaseStream.Position = value;
        }
    }


    //public static int GetPosition(this BinaryReader reader)
    //{
    //    return (int)reader.BaseStream.Position;
    //}

    //public static int GetPosition(this BinaryWriter writer)
    //{
    //    return (int)writer.BaseStream.Position;
    //}

    public static string ReadString(this BinaryReader reader, bool isUnicode)
    {
        short length = reader.ReadInt16();
        if (length > MaxStringLength)
        {
            return string.Empty; // Invalid length, return empty string
        }

        if (isUnicode)
        {
            var chars = new char[length];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)reader.ReadInt16();
            }
            return new string(chars);
        }
        else
        {
            return new string(reader.ReadChars(length));
        }
    }

    public static string ReadString(this BinaryReader reader, int fixedLength)
    {
        //    var chars = new char[fixedLength];
        //    for (int i = 0; i < fixedLength; i++)
        //    {
        //        chars[i] = (char)reader.ReadByte();
        //    }
        //    return new string(chars);
        //}
        //else
        //{
            return new string(reader.ReadChars(fixedLength));
        //}
    }

    

    public static void Write(this BinaryWriter writer, string? str, bool isUnicode)
    {
        if (str == null)
        {
            writer.Write((short)0); // Write length 0 for null string
            return;
        }

        short length = (short)str.Length;
        writer.Write(length);
        if (isUnicode)
        {
            for (int i = 0; i < str.Length; i++)
            {
                writer.Write((short)str[i]);
            }
        }
        else
        {
            writer.Write(str.ToCharArray());
        }
    }

    public static string ReadNullTerminatedString(this BinaryReader reader)
    {
        var chars = new List<char>();
        while (true)
        {
            byte c = reader.ReadByte();
            if (c == 0)
            {
                break;
            }
            chars.Add((char)c);
        }
        return new string(chars.ToArray());
    }

    public static void WriteNullTerminatedString(this BinaryWriter writer, string? str)
    {
        if (str != null)
        {
            writer.Write(str.ToCharArray());
        }
        writer.Write((byte)0);
    }

    public static string ReadNullTerminatedUnicodeString(this BinaryReader reader)
    {
        var chars = new List<char>();
        while (true)
        {
            ushort c = reader.ReadUInt16();
            if (c == 0)
            {
                break;
            }
            chars.Add((char)c);
        }
        return new string(chars.ToArray());
    }

    /// <summary>
    /// Reads a 16-byte GUID/UUID from the stream and converts it to a string.
    /// </summary>
    /// <returns>The GUID as a formatted string (e.g., "00021401-0000-0000-C000-000000000046").</returns>
    public static Guid ReadGuid(this BinaryReader reader)
    {
        Span<byte> buffer = stackalloc byte[16];
        reader.ReadExactly(buffer);
        return new Guid(buffer);
    }

    /// <summary>
    /// Reads a 16-byte GUID/UUID from the stream and returns it as a formatted string.
    /// </summary>
    /// <returns>The GUID as a formatted string (e.g., "00021401-0000-0000-C000-000000000046").</returns>
    public static string ReadGuidString(this BinaryReader reader)
    {
        return reader.ReadGuid().ToString();
    }

    /// <summary>
    /// Reads an 8-byte FILETIME structure from the stream and converts it to a DateTime.
    /// FILETIME represents the number of 100-nanosecond intervals since January 1, 1601 UTC.
    /// </summary>
    /// <returns>A DateTime representing the FILETIME value, or DateTime.MinValue if the FILETIME is 0.</returns>
    public static DateTime ReadFileTime(this BinaryReader reader)
    {
        long fileTime = reader.ReadInt64();
        
        //Span<byte> buffer = stackalloc byte[8];
        //reader.ReadExactly(buffer);
        //long fileTime = BinaryPrimitives.ReadInt64LittleEndian(buffer);

        // FILETIME of 0 represents an invalid or uninitialized time
        if (fileTime == 0)
        {
            return DateTime.MinValue;
        }

        try
        {
            return DateTime.FromFileTimeUtc(fileTime);
        }
        catch (ArgumentOutOfRangeException)
        {
            // Handle invalid FILETIME values
            return DateTime.MinValue;
        }
    }

    public static void Write(this BinaryWriter writer, DateTime? value)
    {
        if (value == null)
        {
            writer.Write((long)0); // Write 8 zero bytes for null FILETIME
            return;
        }
        
        long fileTime = value.Value.ToFileTimeUtc();
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteInt64LittleEndian(buffer, fileTime);
        writer.Write(buffer);
    }

    /// <summary>
    /// Reads an 8-byte FILETIME structure from the stream and converts it to a formatted string.
    /// </summary>
    /// <param name="format">Optional. The DateTime format string. Default is "yyyy-MM-dd HH:mm:ss".</param>
    /// <returns>A formatted string representation of the FILETIME, or "Not Set" if the FILETIME is 0.</returns>
    public static string ReadFileTimeString(this BinaryReader reader, string format = "yyyy-MM-dd HH:mm:ss")
    {
        var dateTime = reader.ReadFileTime();

        if (dateTime == DateTime.MinValue)
        {
            return "Not Set";
        }

        return dateTime.ToString(format);
    }

    



    public static void Write(this BinaryWriter writer, Guid value)
    {
        writer.Write(value.ToByteArray());
    }

    
}