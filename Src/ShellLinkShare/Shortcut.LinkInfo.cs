using ShellLink.Internal;

namespace ShellLink;

public sealed partial class Shortcut
{
    private LinkInfoFlags linkInfoFlags;
    private uint linkInfoHeadersize;

    private void AnalyseLinkInfo(BinaryReader reader)
    {
        using var linkInfoTag = new Size32Tag(reader, "LinkInfo");
        
        AnalyseLinkInfoHeader(reader);
        AnalyseLinkInfoVolumeID(reader);

        Console.WriteLine($"  LocalBasePate: {reader.ReadNullTerminatedString()}");

        AnalyseLinkInfoCommonNetworkRelativeLink(reader);   
        
        Console.WriteLine($"  CommonPathSuffix: {reader.ReadNullTerminatedString()}");

        if (linkInfoHeadersize >= 0x24)
        {
            Console.WriteLine($"  LocalBasePathUnicode: {reader.ReadNullTerminatedUnicodeString()}");
            Console.WriteLine($"  CommonPathSuffixUnicode: {reader.ReadNullTerminatedUnicodeString()}");
        }
    }

    private void AnalyseLinkInfoHeader(BinaryReader reader)
    {
        int offset = -4; // including linkInfoStart
        using var linkInfoHeaderTag = new Size32Tag(reader, "  LinkInfoHeader", offset);
        linkInfoHeadersize = linkInfoHeaderTag.Size;

        linkInfoFlags = (LinkInfoFlags)reader.ReadInt32();
        Console.WriteLine($"    LinkInfoFlags: {linkInfoFlags:X} {linkInfoFlags.ToDetailedString()}");

        Console.WriteLine($"    VolumeIDOffset: {reader.ReadInt32()}");
        Console.WriteLine($"    LocalBasePathOffset: {reader.ReadInt32()}");
        Console.WriteLine($"    CommonNetworkRelativeLinkOffset: {reader.ReadInt32()}");
        Console.WriteLine($"    CommonPathSuffixOffset: {reader.ReadInt32()}");

        if (linkInfoHeaderTag.Size >= 0x20)
        {
            Console.WriteLine($"    LocalBasePathOffsetUnicode: {reader.ReadInt32()}");
        }
        if (linkInfoHeaderTag.Size >= 0x24)
        {
            Console.WriteLine($"    CommonPathSuffixOffsetUnicode: {reader.ReadInt32()}");
        }
    }

    // Firmware 10112006
    // Seriennummer 20038P441210
    private void AnalyseLinkInfoVolumeID(BinaryReader reader)
    {
        if (linkInfoFlags.HasFlag(LinkInfoFlags.VolumeIDAndLocalBasePath))
        {
            using var linkInfoVolumeIDTag = new Size32Tag(reader, "  VolumeID");

            DriveType driveType = (DriveType)reader.ReadInt32();
            Console.WriteLine($"    DriveType: {driveType:X} {driveType}");

            Console.WriteLine($"    DriveSerialNumber: {reader.ReadUInt32()}");

            uint volumeLabelOffset = reader.ReadUInt32();
            Console.WriteLine($"    VolumeLabelOffset: {volumeLabelOffset} from begin 0x{linkInfoVolumeIDTag.Start + volumeLabelOffset:X}");
            uint volumeLabelOffsetUnicode = reader.ReadUInt32();
            Console.WriteLine($"    VolumeLabelOffsetUnicode: {volumeLabelOffsetUnicode} from begin 0x{linkInfoVolumeIDTag.Start + volumeLabelOffsetUnicode:X}");

            //string data = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            //Console.WriteLine($"    Data: {data}");

            reader.Position += linkInfoVolumeIDTag.Size - 5 * 4;
        }
    }

    private void AnalyseLinkInfoCommonNetworkRelativeLink(BinaryReader reader)
    {
        if (linkInfoFlags.HasFlag(LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix))
        {
            using var commonNetworkRelativeLinkTag = new Size32Tag(reader, "  CommonNetworkRelativeLink");
            
            CommonNetworkRelativeLinkFlags commonNetworkRelativeLinkFlags = (CommonNetworkRelativeLinkFlags)reader.ReadInt32();
            Console.WriteLine($"    CommonNetworkRelativeLinkFlags: 0x{commonNetworkRelativeLinkFlags:X8} {commonNetworkRelativeLinkFlags.ToDetailedString()}");

            Console.WriteLine($"    NetNameOffset: {reader.ReadInt32()}");
            Console.WriteLine($"    DeviceNameOffset: {reader.ReadInt32()}");

            NetworkProviderType networkProviderType = (NetworkProviderType)reader.ReadInt32();
            Console.WriteLine($"    NetworkProviderType: 0x{networkProviderType:X8} {networkProviderType.ToDetailedString()}");

            Console.WriteLine($"    NetNameOffsetUnicode: {reader.ReadInt32()}");
            Console.WriteLine($"    DeviceNameOffsetUnicode: {reader.ReadInt32()}");

            Console.WriteLine($"    NetName: {reader.ReadNullTerminatedString()}");
            if (commonNetworkRelativeLinkFlags.HasFlag(CommonNetworkRelativeLinkFlags.ValidDevice))
            {
                Console.WriteLine($"    DeviceName: {reader.ReadNullTerminatedString()}");
            }
            Console.WriteLine($"    NetNameUnicode: {reader.ReadNullTerminatedUnicodeString()}");
            if (commonNetworkRelativeLinkFlags.HasFlag(CommonNetworkRelativeLinkFlags.ValidDevice))
            {
                Console.WriteLine($"    DeviceNameUnicode: {reader.ReadNullTerminatedUnicodeString()}");
            }
        }
    }

    private void ReadLinkInfo(BinaryReader reader)
    {
        if (linkInfoFlags.HasFlag(LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix))
        {
        }
    }

    private void WriteLinkInfo(BinaryWriter writer)
    {
        if (linkInfoFlags.HasFlag(LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix))
        {
        }
    }
}
