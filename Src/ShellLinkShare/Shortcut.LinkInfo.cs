namespace ShellLink;

public sealed partial class Shortcut
{
    private void AnalyseLinkInfo(BinaryReader reader)
    {
        Console.WriteLine();
        Console.WriteLine($"LinkInfo (Start: 0x{reader.Position:X})");

        int linkInfoStart = reader.Position;
        int linkInfoSize = reader.ReadInt32();
        Console.WriteLine($"  LinkInfoSize: {linkInfoSize}");

        int linkInfoHeaderStart = reader.Position;
        int linkInfoHeaderSize = reader.ReadInt32();
        Console.WriteLine($"    LinkInfoHeaderSize: {linkInfoHeaderSize}");

        LinkInfoFlags linkInfoFlags = (LinkInfoFlags)reader.ReadInt32();
        Console.WriteLine($"    LinkInfoFlags: {linkInfoFlags:X}");
        Console.WriteLine(linkInfoFlags.ToDetailedString());

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
        if (linkInfoHeaderStart + linkInfoHeaderSize == linkInfoHeaderEnd)
        {
            Console.WriteLine($"  End: {linkInfoHeaderStart + linkInfoHeaderSize} == {linkInfoHeaderEnd}");
        }
        else
        {
            Console.Error.WriteLine($"Invalid LinkInfoHeaderSize: {linkInfoHeaderSize} instead of actual size {linkInfoHeaderEnd - linkInfoHeaderStart}");
        }


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
    }

    private void ReadLinkInfo(BinaryReader reader)
    {
    }

    private void WriteLinkInfo(BinaryWriter writer)
    {
    }
}
