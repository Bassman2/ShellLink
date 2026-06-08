namespace ShellLink.Internal;

internal static class Assert
{
    //public static void StartTag(string name, int Start, int size)
    //{
    //    Console.ForegroundColor = ConsoleColor.Blue;
    //    Console.WriteLine();
    //    Console.WriteLine($"{name} (Start: 0x{Start:X}, Size: 0x{size:X})");
    //    Console.ResetColor();
    //}

    //public static void EndTag(string name, int Start, int size, int end, int offset = 0)
    //{
    //    int calc = Start + size + offset;
    //    Console.ForegroundColor = ConsoleColor.Blue;
    //    Console.WriteLine($"{name} End (Calc: 0x{calc:X}, Position: 0x{end:X})");
    //    if (Start + size + offset != end)
    //    {
    //        Console.ForegroundColor = ConsoleColor.Red;
    //        Console.Error.WriteLine($"Error: Invalid {name} Size, Calc 0x{calc:X} does not match Position: 0x{end:X}");
    //    } 
    //    Console.ResetColor();
    //}

    //public static void StartTag2(string name, int Start, int size)
    //{
    //    StartTag("  " + name, Start, size);
    //}

    //public static void EndTag2(string name, int Start, int size, int end, int offset = 0)
    //{
    //    EndTag("  " + name, Start, size, end, offset);
    //}

    public static void Equal(string name, Guid value, Guid expected)
    {
        if (!value.Equals(expected))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Error: {name} is {value} instead of expected {expected}");
            Console.ResetColor();
        }
    }

    public static void Equal(string name, int value, int expected)
    {
        if (!value.Equals(expected))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Error: {name} is {value} instead of expected {expected}");
            Console.ResetColor();
        }
    }

    public static void Equal(string name, uint value, uint expected)
    {
        if (!value.Equals(expected))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Error: {name} is 0x{value:X} instead of expected 0x{expected:X}");
            Console.ResetColor();
        }
    }

    public static void RangeEqual(string name, int value, int min, int max)
    {
        if (value < min) 
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Error: {name} is {value} less than {min}");
            Console.ResetColor();
        }
        if (value > max)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Error: {name} is {value} larger than {max}");
            Console.ResetColor();
        }
    }

    public static void FileEnding(BinaryReader reader)
    {
        if (reader.Position != reader.BaseStream.Length)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Error: Unexpected data at the end of the file");
            Console.ResetColor();
        }
    }
}
