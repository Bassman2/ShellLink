using System.Diagnostics.CodeAnalysis;

namespace ShellLink;

public class ShortcutException : ApplicationException
{
    internal ShortcutException() : base()
    { }

    internal ShortcutException(string message) : base(message)
    { }

    internal ShortcutException(string message, Exception innerException) : base(message, innerException)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, "message");
    }

    [DoesNotReturn]
    internal static void ThrowException(string message)
    {
        throw new ArgumentException(message);
    }

    internal static void ThrowIfWrongReadPosition(string tagName, uint position, uint expectedPosition)
    {
        if (position != expectedPosition)
        {
            ThrowException($"Wrong read position in '{tagName}': expected 0x{expectedPosition:X}, got 0x{position:X}");
        }
    }

    internal static void ThrowIfWrongWritePosition(string tagName, uint position, uint expectedPosition)
    {
        if (position != expectedPosition)
        {
            ThrowException($"Wrong write position in '{tagName}': expected 0x{expectedPosition:X}, got 0x{position:X}");
        }
    }

    internal static void ThrowIfNotEqual(string tagName, string parameterName,uint value, uint expected)
    {
        if (value != expected)
        {
            ThrowException($"Error in '{tagName}' parameter '{parameterName}': expected 0x{expected:X}, got 0x{value:X}");
        }
    }

    internal static void ThrowIfNotEqual(string tagName, string parameterName, Guid value, Guid expected)
    {
        if (value != expected)
        {
            ThrowException($"Error in '{tagName}' parameter '{parameterName}': expected {expected}, got {value}");
        }
    }

}
