using ShellLink.Internal;
using System.Reflection.PortableExecutable;

namespace ShellLink;

public sealed partial class Shortcut
{
    private const uint ShellLinkHeaderSize = 0x4C;
    private readonly Guid ShellLinkCLSID = new("00021401-0000-0000-C000-000000000046");

    private LinkFlags linkFlags;
    private FileAttributesFlags fileAttributes;

    private uint fileSize;
    
    private DateTime? creationTime;
    private DateTime? accessTime;
    private DateTime? writeTime;
    
    private void AnalyseShellLinkHeader(BinaryReader reader)
    {
        using var shellLinkHeaderTag = new Size32Tag(reader, "AnalyseShellLinkHeader");

        Assert.Equal("HeaderSize", shellLinkHeaderTag.Size, ShellLinkHeaderSize);
        
        Guid linkCLSID = reader.ReadGuid();
        Console.WriteLine($"  LinkCLSID: {linkCLSID}");
        Assert.Equal("LinkCLSID", linkCLSID, ShellLinkCLSID);

        linkFlags = (LinkFlags)reader.ReadUInt32();
        Console.WriteLine($"  LinkFlags: 0x{linkFlags:X}{linkFlags.ToDetailedString()}");

        fileAttributes = (FileAttributesFlags)reader.ReadUInt32();
        Console.WriteLine($"  FileAttributes: 0x{fileAttributes:X}{fileAttributes.ToDetailedString()}");

        Console.WriteLine($"  CreationTime: {reader.ReadFileTimeString()}");
        Console.WriteLine($"  AccessTime: {reader.ReadFileTimeString()}");
        Console.WriteLine($"  WriteTime: {reader.ReadFileTimeString()}");
        Console.WriteLine($"  FileSize: 0x{reader.ReadInt32():X}");

        IconIndex = reader.ReadInt32();
        Console.WriteLine($"  IconIndex: {IconIndex}");

        ShowCommand = (ShowCommand)reader.ReadUInt32();
        Console.WriteLine($"  ShowCommand: {ShowCommand}");

        HotKey = (HotKeys)reader.ReadByte();
        HotKeyModifiers = (HotKeyModifierKeys)reader.ReadByte();
        Console.WriteLine($"  HotKey: 0x{HotKey:X} {HotKey.ToFlagsString()} 0x{HotKeyModifiers:X} {HotKeyModifiers.ToFlagsString()}");

        Console.WriteLine($"  Reserved1: {reader.ReadInt16()}");
        Console.WriteLine($"  Reserved2: {reader.ReadInt32()}");
        Console.WriteLine($"  Reserved3: {reader.ReadInt32()}");

        ShortcutException.ThrowIfWrongReadPosition("ShellLinkHeader", reader.Position, ShellLinkHeaderSize);
    }

    private void ReadShellLinkHeader(BinaryReader reader)
    {
        uint headerSize = reader.ReadUInt32();
        ShortcutException.ThrowIfNotEqual("ShellLinkHeader", "HeaderSize", headerSize, ShellLinkHeaderSize);
        
        Guid linkCLSID = reader.ReadGuid();
        ShortcutException.ThrowIfNotEqual("ShellLinkHeader", "LinkCLSID", linkCLSID, ShellLinkCLSID);
        
        linkFlags = (LinkFlags)reader.ReadUInt32();
        fileAttributes = (FileAttributesFlags)reader.ReadUInt32();
        creationTime = reader.ReadFileTime();
        accessTime = reader.ReadFileTime();
        writeTime = reader.ReadFileTime();
        fileSize = reader.ReadUInt32();
        IconIndex = reader.ReadInt32();
        ShowCommand = (ShowCommand)reader.ReadUInt32();
        HotKey = (HotKeys)reader.ReadByte();
        HotKeyModifiers = (HotKeyModifierKeys)reader.ReadByte();
        UInt16 reserved1 = reader.ReadUInt16();
        UInt32 reserved2 = reader.ReadUInt32();
        UInt32 reserved3 = reader.ReadUInt32();

        ShortcutException.ThrowIfWrongReadPosition("ShellLinkHeader", reader.Position, ShellLinkHeaderSize);
    }

    private void WriteShellLinkHeader(BinaryWriter writer)
    {
        writer.Write(ShellLinkHeaderSize);      // fix header size
        writer.Write(ShellLinkCLSID);           // fix CLSID
        writer.Write((uint)linkFlags);
        writer.Write((uint)fileAttributes);
        writer.Write(creationTime);
        writer.Write(accessTime);
        writer.Write(writeTime);
        writer.Write(fileSize);
        writer.Write(IconIndex);
        writer.Write((uint)ShowCommand);
        writer.Write((ushort)((ushort)HotKey | (ushort)HotKeyModifiers));
        writer.Write((UInt16)0);
        writer.Write((UInt32)0);
        writer.Write((UInt32)0);

        ShortcutException.ThrowIfWrongWritePosition("ShellLinkHeader", writer.Position, ShellLinkHeaderSize);

    }
}
