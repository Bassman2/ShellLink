namespace ShellLink;

public sealed partial class Shortcut
{
    private const int ShellLinkHeaderSize = 0x4C;
    private readonly Guid ShellLinkCLSID = new("00021401-0000-0000-C000-000000000046");


    private LinkFlags linkFlags;
    private FileAttributes fileAttributes;
    private ShowCommand showCommand;

    private int fileSize;
    private int iconIndex;

    private DateTime? creationTime;
    private DateTime? accessTime;
    private DateTime? writeTime;

    private int hotKey;

    private void AnalyseShellLinkHeader(BinaryReader reader)
    {
        Console.WriteLine();
        Console.WriteLine($"ShellLinkHeader");

        int headerStart = reader.Position;

        int headerSize = reader.ReadInt32();
        if (headerSize == ShellLinkHeaderSize)
        {
            Console.WriteLine($"  HeaderSize: 0x{headerSize:X}");
        }
        else
        {
            Console.Error.WriteLine($"Invalid HeaderSize: 0x{headerSize:X} instead of 0x{ShellLinkHeaderSize:X}");
            return;
        }

        Guid linkCLSID = reader.ReadGuid();
        if (linkCLSID == ShellLinkCLSID)
        {
            Console.WriteLine($"  LinkCLSID: {linkCLSID}");
        }
        else
        {
            Console.Error.WriteLine($"Invalid LinkCLSID: {linkCLSID} instead of {ShellLinkCLSID}");
            return;
        }

        linkFlags = (LinkFlags)reader.ReadInt32();
        Console.WriteLine($"  LinkFlags: 0x{linkFlags:X}{linkFlags.ToDetailedString()}");

        fileAttributes = (FileAttributes)reader.ReadInt32();
        Console.WriteLine($"  FileAttributes: 0x{fileAttributes:X}{fileAttributes.ToDetailedString()}");

        Console.WriteLine($"  CreationTime: {reader.ReadFileTimeString()}");
        Console.WriteLine($"  AccessTime: {reader.ReadFileTimeString()}");
        Console.WriteLine($"  WriteTime: {reader.ReadFileTimeString()}");
        Console.WriteLine($"  FileSize: 0x{reader.ReadInt32():X}");

        Console.WriteLine($"  IconIndex: {reader.ReadInt32()}");
        showCommand = (ShowCommand)reader.ReadInt32();
        Console.WriteLine($"  ShowCommand: {showCommand}");

        Console.WriteLine($"  HotKey: {reader.ReadInt16()}");

        Console.WriteLine($"  Reserved1: {reader.ReadInt16()}");
        Console.WriteLine($"  Reserved2: {reader.ReadInt32()}");
        Console.WriteLine($"  Reserved3: {reader.ReadInt32()}");

        int headerEnd = reader.Position;
        if (headerStart + headerSize == headerEnd)
        {
            Console.WriteLine($"  End: {headerStart + headerSize} == {headerEnd}");
        }
        else
        {
            Console.Error.WriteLine($"Invalid HeaderSize: {headerSize} instead of actual size {headerEnd - headerStart}");
        }
    }

    private void ReadShellLinkHeader(BinaryReader reader)
    {
        int headerSize = reader.ReadInt32();
        if (headerSize != ShellLinkHeaderSize)
        {
            throw new ShortcutException($"Invalid HeaderSize: {headerSize} instead of {ShellLinkHeaderSize}");
        }

        Guid linkCLSID = reader.ReadGuid();
        if (linkCLSID != ShellLinkCLSID)
        {
            throw new ShortcutException($"  Invalid LinkCLSID: {linkCLSID} instead of {ShellLinkCLSID}");
        }

        LinkFlags linkFlags = (LinkFlags)reader.ReadInt32();
    }

    private void WriteShellLinkHeader(BinaryWriter writer)
    {
        
        writer.Write(ShellLinkHeaderSize);
        writer.Write(ShellLinkCLSID);
        writer.Write((int)linkFlags);
        writer.Write((int)fileAttributes);
        writer.Write(creationTime);
        writer.Write(accessTime);
        writer.Write(writeTime);
        writer.Write(fileSize);
        writer.Write(iconIndex);
        writer.Write((int)showCommand);
        writer.Write(hotKey);
        writer.Write((Int16)0);
        writer.Write((Int32)0);
        writer.Write((Int32)0);

        if (writer.Position != ShellLinkHeaderSize)
        {
            throw new ShortcutException("Failed to write ShellLinkHeader");
        }
    }
}
