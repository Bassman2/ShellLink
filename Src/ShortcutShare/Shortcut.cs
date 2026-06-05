
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ShortcutAnalyser")]

namespace ShortcutShare;

public class Shortcut
{
    private const int ShellLinkHeaderSize = 0x4C;   
    private readonly Guid ShellLinkCLSID = new("00021401-0000-0000-C000-000000000046");

    private Shortcut()
    {
        // Private constructor to prevent direct instantiation
    }

    public static Shortcut CreateShortcut() => new();

    internal static void Analyse(string pathLink)
    {
        var shortcut = new Shortcut();
        using var file = File.OpenRead(pathLink);
        using var reader = new BinaryReader(file);
        shortcut.Analyse(reader);
    }

    public static Shortcut Load(string pathLink)
    {
        var shortcut = new Shortcut();
        using var file = File.OpenRead(pathLink);
        using var reader = new BinaryReader(file);
        shortcut.Read(reader);
        return shortcut;
    }
    
    public void Save(string pathLink)
    {
        using var file = File.Create(pathLink);
        using var writer = new BinaryWriter(file);
        Write(writer);
    }

    internal void Analyse(BinaryReader reader)
    {
        #region ShellLinkHeader

        Console.WriteLine($"ShellLinkHeader");

        int headerSize = reader.ReadInt32();
        if (headerSize == ShellLinkHeaderSize)
        {
            Console.WriteLine($"  HeaderSize: {headerSize}");
        }
        else
        { 
            Console.Error.WriteLine($"Invalid HeaderSize: {headerSize} instead of {ShellLinkHeaderSize}");
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

        LinkFlags linkFlags = (LinkFlags)reader.ReadInt32();
        Console.WriteLine($"  LinkFlags: {linkFlags:X}");
        Console.WriteLine(linkFlags.ToDetailedString());

        FileAttributes fileAttributes = (FileAttributes)reader.ReadInt32();
        Console.WriteLine($"  FileAttributes: {fileAttributes:X}");
        Console.WriteLine(fileAttributes.ToDetailedString());


        Console.WriteLine($"  CreationTime: {reader.ReadFileTimeString()}");
        Console.WriteLine($"  AccessTime: {reader.ReadFileTimeString()}");
        Console.WriteLine($"  WriteTime: {reader.ReadFileTimeString()}");
        Console.WriteLine($"  FileSize: {reader.ReadInt32()} bytes");

        Console.WriteLine($"  IconIndex: {reader.ReadInt32()} bytes");
        Console.WriteLine($"  ShowCommand: 0x{reader.ReadInt32():X8}");

        Console.WriteLine($"  HotKey: {reader.ReadInt16()}");

        Console.WriteLine($"  Reserved1: {reader.ReadInt16()}");
        Console.WriteLine($"  Reserved2: {reader.ReadInt32()}");
        Console.WriteLine($"  Reserved3: {reader.ReadInt32()}");

        #endregion

        if (linkFlags.HasFlag(LinkFlags.HasLinkTargetIDList))
        {
            Console.WriteLine("LinkTargetIDList");
            Console.WriteLine($"  IDListSize: {reader.ReadInt16()}");

            while (true)
            {
                short itemIDSize = reader.ReadInt16();
                Console.WriteLine($"  itemIDSize: {itemIDSize}");
                if (itemIDSize == 0) break;

                reader.BaseStream.Seek(itemIDSize, SeekOrigin.Current);
            }
        }

        Console.WriteLine("LinkInfo");
        Console.WriteLine($"  LinkInfoSize: {reader.ReadInt32()} bytes");
        Console.WriteLine($"  LinkInfoHeaderSize: {reader.ReadInt32()} bytes");


        Console.WriteLine("StringData");

        Console.WriteLine("ExtraData");
    }

    private void Read(BinaryReader reader)
    {
        #region ShellLinkHeader

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

        #endregion
    }

    private void Write(BinaryWriter writer)
    {
        #region ShellLinkHeader

        writer.Write(ShellLinkHeaderSize);
        writer.Write(ShellLinkCLSID);
        #endregion
    }



    /*

    public string FullName { get; }


    public string Arguments { get; set; }


    public string Description { get; set; }


    public string Hotkey { get; set; }


    public string IconLocation { get; set; }


    public string RelativePath { set; }


    public string TargetPath { get; set; }


    public int WindowStyle { get; set; }


    public string WorkingDirectory { get; set; }

    */
}
