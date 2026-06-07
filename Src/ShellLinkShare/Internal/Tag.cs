namespace ShellLink.Internal;

internal abstract class Tag : IDisposable
{
    protected readonly BinaryReader reader;
    private readonly string name;
    private readonly int offset;

    private readonly int start;
    public readonly int Size;

    public Tag(BinaryReader reader, string name, int offset = 0)
    {
        this.reader = reader;
        this.name = name;
        this.offset = offset;
        this.start = reader.Position;
        this.Size = GetSize();

        Console.ForegroundColor = ConsoleColor.Blue;
        //Console.WriteLine();
        Console.WriteLine($"{name} (Start: 0x{start:X}, Size: 0x{Size:X})");
        Console.ResetColor();
    }

    protected abstract int GetSize();
    
    public void Dispose()
    {
        int end = reader.Position;
        int calc = start + Size + offset;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{name} End (Calc: 0x{calc:X}, Position: 0x{end:X})");
        if (start + Size + offset != end)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Error: Invalid {name} Size, Calc 0x{calc:X} does not match Position: 0x{end:X}");
        }
        Console.ResetColor();
    }
}

internal class Size16Tag(BinaryReader reader, string name, int offset = 0) : Tag(reader, name, offset)
{
    protected override int GetSize() => reader.ReadInt16();
}

internal class Size32Tag(BinaryReader reader, string name, int offset = 0) : Tag(reader, name, offset)
{
    protected override int GetSize() => reader.ReadInt32();
}
