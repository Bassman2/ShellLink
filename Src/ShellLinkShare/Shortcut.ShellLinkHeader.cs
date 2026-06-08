using ShellLink.Internal;
using System.Reflection.PortableExecutable;

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
        using var shellLinkHeaderTag = new Size32Tag(reader, "AnalyseShellLinkHeader");

        Assert.Equal("HeaderSize", shellLinkHeaderTag.Size, ShellLinkHeaderSize);
        
        Guid linkCLSID = reader.ReadGuid();
        Console.WriteLine($"  LinkCLSID: {linkCLSID}");
        Assert.Equal("LinkCLSID", linkCLSID, ShellLinkCLSID);

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

        HotKeys hotKeys = (HotKeys)reader.ReadByte();
        HotKeyFlags hotKeyFlags = (HotKeyFlags)reader.ReadByte();
        Console.WriteLine($"  HotKey: {hotKeys} {hotKeyFlags.ToFlagsString()}");

        Console.WriteLine($"  Reserved1: {reader.ReadInt16()}");
        Console.WriteLine($"  Reserved2: {reader.ReadInt32()}");
        Console.WriteLine($"  Reserved3: {reader.ReadInt32()}");
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

        ShortcutException.ThrowIfWrongReadPosition("ShellLinkHeader", reader.Position, ShellLinkHeaderSize);
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

        ShortcutException.ThrowIfWrongWritePosition("ShellLinkHeader", writer.Position, ShellLinkHeaderSize);

    }
}
