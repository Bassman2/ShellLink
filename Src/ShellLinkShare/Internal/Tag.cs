namespace ShellLink.Internal;

internal sealed class ExtraTag : IDisposable
{
    private readonly BinaryReader reader;
    private readonly string name;
    public readonly uint Start;
    public readonly uint Size;

    public ExtraTag(BinaryReader reader, string name, uint size)
    {
        this.reader = reader;
        this.name = name;
        this.Start = reader.Position - 8;
        this.Size = size;

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{name} (Start: 0x{Start:X}, Size: 0x{Size:X})");
        Console.ResetColor();
    }

    public void Dispose()
    {
        uint end = (uint)reader.Position;
        uint calc = Start + Size;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{name} End (Calc: 0x{calc:X}, Position: 0x{end:X})");
        if (calc != end)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Error: Invalid {name} Size, Calc 0x{calc:X} does not match Position: 0x{end:X}");
        }
        Console.ResetColor();
    }
}

internal sealed class OpenTag : IDisposable
{
    private readonly BinaryReader reader;
    private readonly string name;
    public readonly uint Start;

    public OpenTag(BinaryReader reader, string name)
    {
        this.reader = reader;
        this.name = name;
        this.Start = reader.Position;

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{name} (Start: 0x{Start:X})");
        Console.ResetColor();
    }

    public void Dispose()
    {
        uint end = reader.Position;
        
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{name} End (Position: 0x{end:X})");
        Console.ResetColor();
    }
}

internal abstract class Tag : IDisposable
{
    protected readonly BinaryReader reader;
    private readonly string name;
    private readonly int offset;

    public readonly uint Start;
    public readonly uint Size;

    public Tag(BinaryReader reader, string name, int offset = 0)
    {
        this.reader = reader;
        this.name = name;
        this.offset = offset;
        this.Start = reader.Position;
        this.Size = GetSize();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{name} (Start: 0x{Start:X}, Size: 0x{Size:X})");
        Console.ResetColor();
    }

    protected abstract uint GetSize();
    
    public void Dispose()
    {
        uint end = reader.Position;
        uint calc = (uint)(Start + Size + offset);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{name} End (Calc: 0x{calc:X}, Position: 0x{end:X})");
        if (calc != end)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Error: Invalid {name} Size, Calc 0x{calc:X} does not match Position: 0x{end:X}");
        }
        Console.ResetColor();
    }
}

internal class Size16Tag(BinaryReader reader, string name, int offset = 0) : Tag(reader, name, offset)
{
    protected override uint GetSize() => reader.ReadUInt16();
}

internal class Size32Tag(BinaryReader reader, string name, int offset = 0) : Tag(reader, name, offset)
{
    protected override uint GetSize() => reader.ReadUInt32();
}
