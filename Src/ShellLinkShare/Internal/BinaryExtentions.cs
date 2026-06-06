using System.Buffers.Binary;
using System.Text;

namespace ShellLink;

internal static class BinaryExtentions
{

    public static string ReadString(this BinaryReader reader, bool isUnicode)
    {
        short length = reader.ReadInt16();
        if (isUnicode)
        {
            var chars = new char[length / 2];
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
        Span<byte> buffer = stackalloc byte[8];
        reader.ReadExactly(buffer);
        long fileTime = BinaryPrimitives.ReadInt64LittleEndian(buffer);

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