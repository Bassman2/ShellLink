
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

        #region LinkTargetIDList

        if (linkFlags.HasFlag(LinkFlags.HasLinkTargetIDList))
        {
            Console.WriteLine("LinkTargetIDList");
            Console.WriteLine($"  IDListSize: {reader.ReadInt16()}");

            while (true)
            {
                short itemIDSize = reader.ReadInt16();
                Console.WriteLine($"  itemIDSize: {itemIDSize}");
                if (itemIDSize == 0) break;

                reader.BaseStream.Seek(itemIDSize - 2, SeekOrigin.Current);
            }
        }

        #endregion

        #region LinkInfo

        Console.WriteLine("LinkInfo");
        Console.WriteLine($"  LinkInfoSize: {reader.ReadInt32()}");
        Console.WriteLine($"  LinkInfoHeaderSize: {reader.ReadInt32()}");

        LinkInfoFlags linkInfoFlags = (LinkInfoFlags)reader.ReadInt32();
        Console.WriteLine($"  LinkInfoFlags: {linkInfoFlags:X}");
        Console.WriteLine(linkInfoFlags.ToDetailedString());

        Console.WriteLine($"  VolumeIDOffset: {reader.ReadInt32()}");
        Console.WriteLine($"  LocalBasePathOffset: {reader.ReadInt32()}");
        Console.WriteLine($"  CommonNetworkRelativeLinkOffset: {reader.ReadInt32()}");
        Console.WriteLine($"  CommonPathSuffixOffset: {reader.ReadInt32()}");
        Console.WriteLine($"  LocalBasePathOffsetUnicode: {reader.ReadInt32()}");
        Console.WriteLine($"  CommonPathSuffixOffsetUnicode: {reader.ReadInt32()}");

        if (linkInfoFlags.HasFlag(LinkInfoFlags.VolumeIDAndLocalBasePath))
        {
            #region VolumeID
            
            Console.WriteLine($"  VolumeIDSize: {reader.ReadInt32()}");

            DriveType driveType = (DriveType)reader.ReadInt32();
            Console.WriteLine($"  DriveType: {driveType:X}");
            Console.WriteLine(driveType.ToDetailedString());

            Console.WriteLine($"  DriveSerialNumber: {reader.ReadInt32()}");
            Console.WriteLine($"  VolumeLabelOffset: {reader.ReadInt32()}");
            Console.WriteLine($"  VolumeLabelOffsetUnicode: {reader.ReadInt32()}");

            // Data

            #endregion

            #region LocalBasePath

            #endregion
        }

        if (linkInfoFlags.HasFlag(LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix))
        {
            #region CommonNetworkRelativeLink


            CommonNetworkRelativeLinkFlags commonNetworkRelativeLinkFlags = (CommonNetworkRelativeLinkFlags)reader.ReadInt32();
            Console.WriteLine($"  CommonNetworkRelativeLinkFlags: {commonNetworkRelativeLinkFlags:X8}");
            Console.WriteLine(commonNetworkRelativeLinkFlags.ToDetailedString());

            Console.WriteLine($"  NetNameOffset: {reader.ReadInt32()}");
            Console.WriteLine($"  DeviceNameOffset: {reader.ReadInt32()}");

            NetworkProviderType networkProviderType = (NetworkProviderType)reader.ReadInt32();
            Console.WriteLine($"  NetworkProviderType: {networkProviderType:X8}");
            Console.WriteLine(networkProviderType.ToDetailedString());

            Console.WriteLine($"  NetNameOffsetUnicode: {reader.ReadInt32()}");
            Console.WriteLine($"  DeviceNameOffsetUnicode: {reader.ReadInt32()}");

            Console.WriteLine($"  NetName: {reader.ReadNullTerminatedString()}");
            if (commonNetworkRelativeLinkFlags.HasFlag(CommonNetworkRelativeLinkFlags.ValidDevice))
            {
                Console.WriteLine($"  DeviceName: {reader.ReadNullTerminatedString()}");
            }
            Console.WriteLine($"  NetNameUnicode: {reader.ReadNullTerminatedUnicodeString()}");
            if (commonNetworkRelativeLinkFlags.HasFlag(CommonNetworkRelativeLinkFlags.ValidDevice))
            {
                Console.WriteLine($"  DeviceNameUnicode: {reader.ReadNullTerminatedUnicodeString()}");
            }

            #endregion
        }
        Console.WriteLine($"  CommonPathSuffix: {reader.ReadNullTerminatedString()}");


        Console.WriteLine($"  LocalBasePathUnicode: {reader.ReadNullTerminatedString()}");
        Console.WriteLine($"  CommonPathSuffixUnicode: {reader.ReadNullTerminatedString()}");

        #endregion

        #region StringData

        Console.WriteLine("StringData");

        if (linkFlags.HasFlag(LinkFlags.HasName))
        {
            string nameString = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"  NameString: {nameString}");
        }
        if (linkFlags.HasFlag(LinkFlags.HasRelativePath))
        {
            string relativePath = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"  RelativePath: {relativePath}");
        }
        if (linkFlags.HasFlag(LinkFlags.HasWorkingDir))
        {
            string workingDir = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"  WorkingDir: {workingDir}");
        }
        if (linkFlags.HasFlag(LinkFlags.HasArguments))
        {
            string commandLineArguments = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"  CommandLineArguments: {commandLineArguments}");
        }
        if (linkFlags.HasFlag(LinkFlags.HasIconLocation))
        {
            string iconLocation = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"  IconLocation: {iconLocation}");
        }

        #endregion

        #region ExtraData

        Console.WriteLine("ExtraData");

        #endregion
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

        LinkFlags linkFlags = (LinkFlags)reader.ReadInt32();

        #endregion

        #region LinkTargetIDList

        #endregion

        #region LinkInfo

        #endregion

        #region StringData

        if (linkFlags.HasFlag(LinkFlags.HasName))
        {
            FullName = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
        }
        if (linkFlags.HasFlag(LinkFlags.HasRelativePath))
        {
            RelativePath = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
        }
        if (linkFlags.HasFlag(LinkFlags.HasWorkingDir))
        {
            WorkingDirectory = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
        }
        if (linkFlags.HasFlag(LinkFlags.HasArguments))
        {
            Arguments = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
        }
        if (linkFlags.HasFlag(LinkFlags.HasIconLocation))
        {
            IconLocation = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
        }

        #endregion

        #region ExtraData



        #endregion
    }

    private void Write(BinaryWriter writer)
    {
        #region ShellLinkHeader

        writer.Write(ShellLinkHeaderSize);
        writer.Write(ShellLinkCLSID);
        #endregion
    }



    

    public string FullName { get; private set; }


    public string Arguments { get; set; }


    public string Description { get; set; }


    public string Hotkey { get; set; }


    public string IconLocation { get; set; }


    public string RelativePath { get;  set; }


    public string TargetPath { get; set; }


    public int WindowStyle { get; set; }


    public string WorkingDirectory { get; set; }

}
