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
        int headerStart = reader.Position;
        int headerSize = reader.ReadInt32();

        Console.WriteLine();
        Console.WriteLine($"ShellLinkHeader (Start: 0x{headerStart:X}, Size: 0x{headerSize:X})");
                
        if (headerSize != ShellLinkHeaderSize)
        {
            Console.Error.WriteLine($"Error: Invalid HeaderSize: 0x{headerSize:X} instead of 0x{ShellLinkHeaderSize:X}");
            return;
        }

        Guid linkCLSID = reader.ReadGuid();
        Console.WriteLine($"  LinkCLSID: {linkCLSID}");
        if (linkCLSID != ShellLinkCLSID)
        {
            Console.Error.WriteLine($"Error: Invalid LinkCLSID: {linkCLSID} instead of {ShellLinkCLSID}");
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
        Console.WriteLine($"ShellLinkHeader End: 0x{headerStart + headerSize:X} == 0x{headerEnd:X}");
       
        if (headerStart + headerSize != headerEnd)
        {
            Console.Error.WriteLine($"Error: Invalid HeaderSize: 0x{headerSize:X} instead of actual size 0x{headerEnd - headerStart:X}");
        }
    }

    private void ReadShellLinkHeader(BinaryReader reader)
    {
        int headerSize = reader.ReadInt32();
        if (headerSize != ShellLinkHeaderSize)
        {
            throw new ShortcutException($"Error: Invalid HeaderSize: {headerSize} instead of {ShellLinkHeaderSize}");
        }

        Guid linkCLSID = reader.ReadGuid();
        if (linkCLSID != ShellLinkCLSID)
        {
            throw new ShortcutException($"Error: Invalid LinkCLSID: {linkCLSID} instead of {ShellLinkCLSID}");
        }

        linkFlags = (LinkFlags)reader.ReadInt32();
        fileAttributes = (FileAttributes)reader.ReadInt32();
        creationTime = reader.ReadFileTime();
        accessTime = reader.ReadFileTime();
        writeTime = reader.ReadFileTime();
        fileSize = reader.ReadInt32();
        iconIndex = reader.ReadInt32();
        showCommand = (ShowCommand)reader.ReadInt32();
        hotKey = reader.ReadInt16();
        Int16 reserved1 = reader.ReadInt16();
        Int32 reserved2 = reader.ReadInt32();
        Int32 reserved3 = reader.ReadInt32();

        if (reader.Position != ShellLinkHeaderSize)
        {
            throw new ShortcutException("Failed to read ShellLinkHeader");
        }
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
