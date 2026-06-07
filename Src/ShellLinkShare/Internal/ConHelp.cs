namespace ShellLink.Internal;

internal static class ConHelp
{
    public static void StartTag(string name, int start, int size)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine();
        Console.WriteLine($"{name} (Start: 0x{start:X}, Size: 0x{size:X})");
        Console.ResetColor();
    }

    public static void EndTag(string name, int start, int size, int end, int offset = 0)
    {
        int calc = start + size + offset;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{name} End (Calc: 0x{calc:X}, Position: 0x{end:X})");
        if (start + size + offset != end)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Error: Invalid {name} Size, Calc 0x{calc:X} does not match Position: 0x{end:X}");
        } 
        Console.ResetColor();
    }

    public static void StartTag2(string name, int start, int size)
    {
        StartTag("  " + name, start, size);
    }

    public static void EndTag2(string name, int start, int size, int end, int offset = 0)
    {
        EndTag("  " + name, start, size, end, offset);
    }

    public static void Equal(string name, object value, object expected)
    {
        if (!value.Equals(expected))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Error: {name} is {value} instead of expected {expected}");
            Console.ResetColor();
        }
    }
}
