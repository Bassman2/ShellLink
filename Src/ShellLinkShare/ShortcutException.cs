using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ShellLink;

public class ShortcutException : ApplicationException
{
    public ShortcutException() : base()
    { }

    public ShortcutException(string message) : base(message)
    { }

    public ShortcutException(string message, Exception innerException) : base(message, innerException)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, "message");
    }

    [DoesNotReturn]
    private static void ThrowException(string message)
    {
        throw new ArgumentException(message);
    }

    public static void ThrowIfWrongReadPosition(string tag, uint position, uint expectedPosition)
    {
        if (position != expectedPosition)
        {
            ThrowException($"Wrong read position in '{tag}': expected 0x{expectedPosition:X}, got 0x{position:X}");
        }
    }

    public static void ThrowIfWrongWritePosition(string tag, uint position, uint expectedPosition)
    {
        if (position != expectedPosition)
        {
            ThrowException($"Wrong write position in '{tag}': expected 0x{expectedPosition:X}, got 0x{position:X}");
        }
    }

}
