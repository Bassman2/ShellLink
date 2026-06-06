namespace ShellLink;

public sealed partial class Shortcut
{
    private LinkInfoFlags linkInfoFlags;

    private void AnalyseLinkInfo(BinaryReader reader)
    {
        int linkInfoStart = reader.Position;
        int linkInfoSize = reader.ReadInt32();

        Console.WriteLine();
        Console.WriteLine($"LinkInfo (Start: 0x{linkInfoStart:X}, Size: 0x{linkInfoSize:X})");

        AnalyseLinkInfoHeader(reader);
        AnalyseLinkInfoVolumeID(reader);

        Console.WriteLine($"  LocalBasePate: {reader.ReadNullTerminatedString()}");
        AnalyseLinkInfoCommonNetworkRelativeLink(reader);   
               
        
        Console.WriteLine($"  CommonPathSuffix: {reader.ReadNullTerminatedString()}");


        Console.WriteLine($"  LocalBasePathUnicode: {reader.ReadNullTerminatedUnicodeString()}");
        Console.WriteLine($"  CommonPathSuffixUnicode: {reader.ReadNullTerminatedUnicodeString()}");

        int linkInfoEnd = reader.Position;
        Console.WriteLine($"LinkInfo End: 0x{linkInfoStart + linkInfoSize:X} == 0x{linkInfoEnd:X}");
    }

    private void AnalyseLinkInfoHeader(BinaryReader reader)
    {
        int linkInfoHeaderStart = reader.Position;
        int linkInfoHeaderSize = reader.ReadInt32();
        Console.WriteLine($"  LinkInfoHeader (Start: 0x{linkInfoHeaderStart:X}, Size: 0x{linkInfoHeaderSize:X})");

        linkInfoFlags = (LinkInfoFlags)reader.ReadInt32();
        Console.WriteLine($"    LinkInfoFlags: {linkInfoFlags:X} {linkInfoFlags.ToDetailedString()}");

        Console.WriteLine($"    VolumeIDOffset: {reader.ReadInt32()}");
        Console.WriteLine($"    LocalBasePathOffset: {reader.ReadInt32()}");
        Console.WriteLine($"    CommonNetworkRelativeLinkOffset: {reader.ReadInt32()}");
        Console.WriteLine($"    CommonPathSuffixOffset: {reader.ReadInt32()}");

        if (linkInfoHeaderSize >= 0x24)
        {
            Console.WriteLine($"    LocalBasePathOffsetUnicode: {reader.ReadInt32()}");
            Console.WriteLine($"    CommonPathSuffixOffsetUnicode: {reader.ReadInt32()}");
        }

        int linkInfoHeaderEnd = reader.Position;
        Console.WriteLine($"  LinkInfoHeader End: {linkInfoHeaderStart + linkInfoHeaderSize} == {linkInfoHeaderEnd}");

        if (linkInfoHeaderStart + linkInfoHeaderSize != linkInfoHeaderEnd)
        {
            Console.Error.WriteLine($"Error: Invalid LinkInfoHeaderSize: {linkInfoHeaderSize} instead of actual size {linkInfoHeaderEnd - linkInfoHeaderStart}");
        }
    }

    // Firmware 10112006
    // Seriennummer 20038P441210
    private void AnalyseLinkInfoVolumeID(BinaryReader reader)
    {
        if (linkInfoFlags.HasFlag(LinkInfoFlags.VolumeIDAndLocalBasePath))
        {
            int volumeIDStart = reader.Position;
            int volumeIDSize = reader.ReadInt32();
            Console.WriteLine($"  VolumeID: (Start: 0x{volumeIDStart:X}, Size: 0x{volumeIDSize:X})");

            DriveType driveType = (DriveType)reader.ReadInt32();
            Console.WriteLine($"    DriveType: {driveType:X} {driveType}");

            Console.WriteLine($"    DriveSerialNumber: {reader.ReadInt32()}");
            Console.WriteLine($"    VolumeLabelOffset: {reader.ReadInt32()}");
            Console.WriteLine($"    VolumeLabelOffsetUnicode: {reader.ReadInt32()}");

            string data = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"    Data: {data}");

            int volumeIDEnd = reader.Position;
            Console.WriteLine($"  VolumeID End: 0x{volumeIDStart + volumeIDSize:X} == 0x{volumeIDEnd:X}");
        }
    }

    private void AnalyseLinkInfoCommonNetworkRelativeLink(BinaryReader reader)
    {
        if (linkInfoFlags.HasFlag(LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix))
        {
            int commonNetworkRelativeLinkStart = reader.Position;
            int commonNetworkRelativeLinkSize = reader.ReadInt32();

            Console.WriteLine($"  CommonNetworkRelativeLink: (Start: 0x{commonNetworkRelativeLinkStart:X}, Size: 0x{commonNetworkRelativeLinkSize:X})");

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

            int commonNetworkRelativeLinkEnd = reader.Position;
            Console.WriteLine($"  CommonNetworkRelativeLink End: {commonNetworkRelativeLinkStart + commonNetworkRelativeLinkSize} == {commonNetworkRelativeLinkEnd}");

            if (commonNetworkRelativeLinkStart + commonNetworkRelativeLinkSize != commonNetworkRelativeLinkEnd)
            {
                Console.Error.WriteLine($"Error: Invalid CommonNetworkRelativeLink Size: {commonNetworkRelativeLinkSize} instead of actual size {commonNetworkRelativeLinkEnd - commonNetworkRelativeLinkStart}");
            }
        }
    }

    private void ReadLinkInfo(BinaryReader reader)
    {
    }

    private void WriteLinkInfo(BinaryWriter writer)
    {
    }
}
